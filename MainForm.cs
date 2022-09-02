using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenAP_FileConverter
{
    public partial class MainForm : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm() );
        }

        public MainForm()
        {
            InitializeComponent();

            srcExtComboBox.SelectedIndex = 0;
            dstExtComboBox.SelectedIndex = 0;

            srcPathTextBox.Text = Properties.Settings.Default.SrcPath;
            dstPathTextBox.Text = Properties.Settings.Default.DstPath;
        }

        private void convertButton_Click( object sender, EventArgs e )
        {
            var srcPath = srcPathTextBox.Text;
            var dstPath = dstPathTextBox.Text;
            
            Properties.Settings.Default.SrcPath = srcPath;
            Properties.Settings.Default.DstPath = dstPath;
            Properties.Settings.Default.Save();

            dstPath = Path.Combine( dstPath, Path.GetFileName( srcPath ) );

            var srcExt = srcExtComboBox.Text;
            if( srcExt.IndexOf( ' ' ) != -1 ) {
                srcExt = srcExt.Substring( 0, srcExt.IndexOf( ' ' ) );
            }

            if( !Directory.Exists( dstPath ) ) {
                Directory.CreateDirectory( dstPath );
            }
            VisualTask.Run( this, null, log =>
            {
                int count = 0;
                foreach( var filePath in Directory.GetFiles( srcPath, srcExt ) ) {
                    log.Trace( Path.GetFileName( filePath ) );
                    
                    var imageInfo = readImageInfo( filePath );
                    var width = int.Parse( imageInfo["IMAGE_WIDTH"] );
                    var height = int.Parse( imageInfo["IMAGE_HEIGHT"] );
                    System.Diagnostics.Trace.Assert( imageInfo["PIXEL_FORMAT"] == "u16" );
                    log.Trace( "-> {0}x{1}", width, height );

                    var pixels = new ushort[width * height]; 
                    using( var stream = File.OpenRead( filePath ) ) {
                        using( var reader = new BinaryReader( stream ) ) {
                            for( int pos = 0; pos < pixels.Length; pos++ ) {
                                pixels[pos] = reader.ReadUInt16();
                            }
                        }
                    }
                    save16BitGrayTiff( Path.Combine( dstPath, Path.GetFileNameWithoutExtension( filePath ) + ".tif" ),
                        pixels, width, height, imageInfo );

                    log.Trace( "-> ОК", width, height );
                    count++;

                }
                log.TraceFinished( "Converted {0} frames.", count );
            } );
        }

        static Dictionary<string, string> readImageInfo( string filePath )
        {
            var map = new Dictionary<string, string>();
            
            var infoFilePath = filePath.Replace( ".pixels", ".info" );
            foreach( var line in File.ReadAllLines( infoFilePath ) ) {
                int pos = line.IndexOf( ' ' );
                System.Diagnostics.Trace.Assert( pos != -1 );
                map.Add( line.Substring( 0, pos ).Trim(), line.Substring( pos + 1 ) );
            }
            return map;
        }

        static public void save16BitGrayTiff( string filePath, ushort[] pixels, int width, int height, Dictionary<string, string> imageInfo )
        {
            if( File.Exists( filePath ) ) {
                // Otherwise encoder appends data to the existing file
                File.Delete( filePath );
            }
            
            var g16 = new WriteableBitmap( width, height, 96.0, 96.0,
                PixelFormats.Gray16, null );

            g16.WritePixels( new System.Windows.Int32Rect( 0, 0, width, height ), pixels, width * 2, 0 );

            var encoder = new TiffBitmapEncoder();

            if( imageInfo != null ) {
                var metadata = new BitmapMetadata( "tiff" );
                metadata.CameraModel = imageInfo["CAMERA"];
                metadata.SetQuery( "/ifd/exif", new BitmapMetadata( "exif" ) );
                string txt;
                if( imageInfo.TryGetValue( "CAMERA_ISO", out txt ) ||
                    imageInfo.TryGetValue( "CAMERA_GAIN", out txt ) ) {
                    metadata.SetQuery( "/ifd/exif/{ushort=34855}", ushort.Parse( txt ) );
                }
                if( imageInfo.TryGetValue( "CAMERA_EXPOSURE", out txt ) ) {
                    double value = double.Parse( txt, System.Globalization.CultureInfo.InvariantCulture );
                    ulong k = 1;
                    while( k * value != (ulong) ( k * value ) ) {
                        k *= 10;
                    }
                    var exposure = ( k << 32 ) | (ulong) ( k * value );
                    //var exposure = UInt64.Parse( "68719476737" );
                    metadata.SetQuery( "/ifd/exif/{ushort=33434}", exposure );
                }
                if( imageInfo.TryGetValue( "TIMESTAMP", out txt ) ) {
                    DateTime dateTime = new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc );
                    metadata.DateTaken = dateTime.AddSeconds( ulong.Parse( txt ) ).ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
                    
                }
                encoder.Frames.Add( BitmapFrame.Create( g16, null, metadata, null ) );
            } else {
                encoder.Frames.Add( BitmapFrame.Create( g16 ) );
            }

            using( var stream = System.IO.File.OpenWrite( filePath ) ) {
                encoder.Save( stream );
            }
        }

    }
}
