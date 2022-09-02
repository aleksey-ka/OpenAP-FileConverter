namespace OpenAP_FileConverter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.srcPathTextBox = new System.Windows.Forms.TextBox();
            this.srcLabel = new System.Windows.Forms.Label();
            this.srcExtComboBox = new System.Windows.Forms.ComboBox();
            this.dstPathTextBox = new System.Windows.Forms.TextBox();
            this.destLabel = new System.Windows.Forms.Label();
            this.dstExtComboBox = new System.Windows.Forms.ComboBox();
            this.convertButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // srcPathTextBox
            // 
            this.srcPathTextBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.srcPathTextBox.BackColor = System.Drawing.Color.DimGray;
            this.srcPathTextBox.ForeColor = System.Drawing.Color.White;
            this.srcPathTextBox.Location = new System.Drawing.Point( 100, 14 );
            this.srcPathTextBox.Name = "srcPathTextBox";
            this.srcPathTextBox.Size = new System.Drawing.Size( 438, 22 );
            this.srcPathTextBox.TabIndex = 0;
            // 
            // srcLabel
            // 
            this.srcLabel.AutoSize = true;
            this.srcLabel.Location = new System.Drawing.Point( 6, 17 );
            this.srcLabel.Name = "srcLabel";
            this.srcLabel.Size = new System.Drawing.Size( 90, 17 );
            this.srcLabel.TabIndex = 1;
            this.srcLabel.Text = "Source Path:";
            // 
            // srcExtComboBox
            // 
            this.srcExtComboBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.srcExtComboBox.BackColor = System.Drawing.Color.DimGray;
            this.srcExtComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.srcExtComboBox.FormattingEnabled = true;
            this.srcExtComboBox.Items.AddRange( new object[] {
            "*.pixels"} );
            this.srcExtComboBox.Location = new System.Drawing.Point( 542, 13 );
            this.srcExtComboBox.Name = "srcExtComboBox";
            this.srcExtComboBox.Size = new System.Drawing.Size( 141, 24 );
            this.srcExtComboBox.TabIndex = 1;
            // 
            // dstPathTextBox
            // 
            this.dstPathTextBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.dstPathTextBox.BackColor = System.Drawing.Color.DimGray;
            this.dstPathTextBox.ForeColor = System.Drawing.Color.White;
            this.dstPathTextBox.Location = new System.Drawing.Point( 100, 42 );
            this.dstPathTextBox.Name = "dstPathTextBox";
            this.dstPathTextBox.Size = new System.Drawing.Size( 438, 22 );
            this.dstPathTextBox.TabIndex = 2;
            // 
            // destLabel
            // 
            this.destLabel.AutoSize = true;
            this.destLabel.Location = new System.Drawing.Point( 22, 45 );
            this.destLabel.Name = "destLabel";
            this.destLabel.Size = new System.Drawing.Size( 74, 17 );
            this.destLabel.TabIndex = 1;
            this.destLabel.Text = "Dest Path:";
            // 
            // dstExtComboBox
            // 
            this.dstExtComboBox.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.dstExtComboBox.BackColor = System.Drawing.Color.DimGray;
            this.dstExtComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dstExtComboBox.FormattingEnabled = true;
            this.dstExtComboBox.Items.AddRange( new object[] {
            "*.tif (16-bit RAW)"} );
            this.dstExtComboBox.Location = new System.Drawing.Point( 542, 41 );
            this.dstExtComboBox.Name = "dstExtComboBox";
            this.dstExtComboBox.Size = new System.Drawing.Size( 141, 24 );
            this.dstExtComboBox.TabIndex = 3;
            // 
            // convertButton
            // 
            this.convertButton.BackColor = System.Drawing.Color.DimGray;
            this.convertButton.Location = new System.Drawing.Point( 100, 70 );
            this.convertButton.Name = "convertButton";
            this.convertButton.Size = new System.Drawing.Size( 134, 33 );
            this.convertButton.TabIndex = 4;
            this.convertButton.Text = "Convert";
            this.convertButton.UseVisualStyleBackColor = false;
            this.convertButton.Click += new System.EventHandler( this.convertButton_Click );
            // 
            // MainForm
            // 
            this.AcceptButton = this.convertButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size( 687, 115 );
            this.Controls.Add( this.convertButton );
            this.Controls.Add( this.dstExtComboBox );
            this.Controls.Add( this.srcExtComboBox );
            this.Controls.Add( this.destLabel );
            this.Controls.Add( this.srcLabel );
            this.Controls.Add( this.dstPathTextBox );
            this.Controls.Add( this.srcPathTextBox );
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenAP - FileConverter";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox srcPathTextBox;
        private System.Windows.Forms.Label srcLabel;
        private System.Windows.Forms.ComboBox srcExtComboBox;
        private System.Windows.Forms.TextBox dstPathTextBox;
        private System.Windows.Forms.Label destLabel;
        private System.Windows.Forms.ComboBox dstExtComboBox;
        private System.Windows.Forms.Button convertButton;
    }
}

