using System;
using System.Windows.Forms;
using System.Drawing;

namespace OpenAP_FileConverter
{
    public interface ILog
    {
        void TraceHeader( string txt );
        void Trace( string text, params object[] args );
        void Trace( Color foreColor, string text, params object[] args );
        void TraceBold( Color foreColor, string text, params object[] args );
        void TraceError( Exception e );
        void TraceFinished();
        void TraceFinished( string text, params object[] args );
    }
    
    public class VisualTask : IDisposable, ILog
    {
        public static void Run( Form owner, string caption, Action<ILog> action )
        {
            using( var log = new VisualTask( owner, caption ) ) {
                try {
                    action( log );
                } catch( OperationCanceledException ) {
                } catch( Exception ex ) {
                    log.TraceError( ex );
                }
            }
        }

        private VisualTask( Form owner, string caption )
        {
            if( view != null ) {
                view.Close();
            }
            view = new LogView( owner, caption );
            richEdit = view.richEdit;
        }

        public void Dispose()
        {
            EndLog();
        }

        public void Trace( string text, params object[] args )
        {
            Application.DoEvents();
            if( view.shouldStop ) {
                throw new System.OperationCanceledException();
            }
            richEdit.SelectionColor = richEdit.ForeColor;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = richEdit.Font;
            richEdit.AppendText( string.Format( text + "\n", args ) );
            Application.DoEvents();
        }

        public void Trace( Color foreColor, string text, params object[] args )
        {
            Application.DoEvents();
            if( view.shouldStop ) {
                throw new System.OperationCanceledException();
            }
            richEdit.SelectionColor = foreColor;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = richEdit.Font;
            richEdit.AppendText( string.Format( text + "\n", args ) );
            Application.DoEvents();
        }

        public void TraceBold( Color foreColor, string text, params object[] args )
        {
            Application.DoEvents();
            if( view.shouldStop ) {
                throw new System.OperationCanceledException();
            }
            richEdit.SelectionColor = foreColor;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = new Font( richEdit.Font, FontStyle.Bold );
            richEdit.AppendText( string.Format( text + "\n", args ) );
            Application.DoEvents();
        }

        public void TraceHeader( string txt )
        {
            richEdit.SelectionColor = Color.Green;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = richEdit.Font;
            richEdit.AppendText( string.Format( "\n=== {0} {1}\n\n", txt, new string( '=', 80 - txt.Length ) ) );
            Application.DoEvents();
        }

        public void TraceError( System.Exception ex )
        {
            richEdit.SelectionColor = Color.Red;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = new Font( richEdit.Font, FontStyle.Bold );
            richEdit.AppendText( string.Format( "\n\n{0}\n\n", ex.Message ) );
            Application.DoEvents();
        }

        public void TraceFinished( string text, params object[] args )
        {
            richEdit.SelectionColor = Color.LightGreen;
            richEdit.SelectionBackColor = richEdit.BackColor;
            richEdit.SelectionFont = richEdit.Font;
            richEdit.AppendText( string.Format( "\n" + text + "\n", args ) );
            Application.DoEvents();
        }

        public void TraceFinished()
        {
            TraceFinished( "Done." );
        }

        private void EndLog()
        {
            Application.DoEvents();
            Application.UseWaitCursor = false;
            Application.RemoveMessageFilter( view );
            view.running = false;
            if( view.shouldStop ) {
                view.Close();
            } else {
                view.Text = view.caption + " - Finished";
            }
        }

        private class LogView : Form, IMessageFilter
        {
            public LogView( Form owner, string _caption )
            {
                caption = _caption;
                Text = caption != null ? caption : "Operation progress (ESC to stop)...";
                Height = 200;
                Width = owner.Width;
                MinimizeBox = false;
                MaximizeBox = false;
                StartPosition = FormStartPosition.Manual;
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                Location = new Point( owner.Left, owner.Bottom );

                richEdit.Font = new Font( "Courier New", 8 );
                richEdit.ForeColor = owner.ForeColor;
                richEdit.BackColor = owner.BackColor;
                richEdit.ReadOnly = true;
                richEdit.WordWrap = false;
                richEdit.Dock = DockStyle.Fill;
                Controls.Add( richEdit );

                FormClosing += new FormClosingEventHandler( ProgressLog_FormClosing );

                KeyDown += new KeyEventHandler( ProgressLog_KeyDown );
                this.KeyPreview = true;

                Show( owner );
                //CenterToParent();

                Application.UseWaitCursor = true;
                Application.AddMessageFilter( this );
                Application.DoEvents();
            }

            void ProgressLog_KeyDown( object sender, KeyEventArgs e )
            {
                switch( e.KeyCode ) {
                    case Keys.Escape:
                        Close();
                        break;
                }
            }

            void ProgressLog_FormClosing( object sender, FormClosingEventArgs e )
            {
                if( running ) {
                    Text = "Cancelling...";
                    e.Cancel = true;
                    shouldStop = true;
                }
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    //const int CS_NOCLOSE = 0x200;
                    const int WS_EX_APPWINDOW = 0x40000;
                    CreateParams cp = base.CreateParams;
                    //cp.ClassStyle |= CS_NOCLOSE;
                    cp.ExStyle &= ~WS_EX_APPWINDOW;
                    return cp;
                }
            }

            private const int WM_MOUSEFIRST = 0x0200;
            private const int WM_MOUSELAST = 0x020A;
            private const int WM_KEYFIRST = 0x0100;
            private const int WM_KEYLAST = 0x0109;
            private const int WM_KEYDOWN = 0x0100;
            private const int VK_ESCAPE = 0x1B;

            bool IMessageFilter.PreFilterMessage( ref Message m )
            {
                if( m.Msg == WM_KEYDOWN && (int)m.WParam == VK_ESCAPE ) {
                    shouldStop = true;
                    return true;
                }
                if( m.Msg >= WM_MOUSEFIRST && m.Msg <= WM_MOUSELAST ) {
                    //if( m.HWnd == cancelButton.Handle && cancelButton.Enabled ) {
                    //    return false;
                    //}
                    return true;
                }
                if( m.Msg >= WM_KEYFIRST && m.Msg <= WM_KEYLAST ) {
                    return true;
                }
                return false;
            }
            public RichTextBox richEdit = new RichTextBox();
            public bool running = true;
            public bool shouldStop = false;
            public string caption;
        }

        static private LogView view;
        private RichTextBox richEdit;
    } 
}
