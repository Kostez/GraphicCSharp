namespace CGL3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.originalImage = new System.Windows.Forms.PictureBox();
            this.transformedImage = new System.Windows.Forms.PictureBox();
            this.openImageButton = new System.Windows.Forms.Button();
            this.transformButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transformedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // originalImage
            // 
            this.originalImage.BackColor = System.Drawing.Color.White;
            this.originalImage.Location = new System.Drawing.Point(12, 12);
            this.originalImage.Name = "originalImage";
            this.originalImage.Size = new System.Drawing.Size(300, 300);
            this.originalImage.TabIndex = 0;
            this.originalImage.TabStop = false;
            this.originalImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OriginalImageClick);
            // 
            // transformedImage
            // 
            this.transformedImage.BackColor = System.Drawing.Color.White;
            this.transformedImage.Location = new System.Drawing.Point(320, 12);
            this.transformedImage.Name = "transformedImage";
            this.transformedImage.Size = new System.Drawing.Size(300, 300);
            this.transformedImage.TabIndex = 1;
            this.transformedImage.TabStop = false;
            this.transformedImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TransformedImageClick);
            // 
            // openImageButton
            // 
            this.openImageButton.Location = new System.Drawing.Point(12, 318);
            this.openImageButton.Name = "openImageButton";
            this.openImageButton.Size = new System.Drawing.Size(300, 30);
            this.openImageButton.TabIndex = 2;
            this.openImageButton.Text = "open image";
            this.openImageButton.UseVisualStyleBackColor = true;
            this.openImageButton.Click += new System.EventHandler(this.OpenImageButtonClick);
            // 
            // transformButton
            // 
            this.transformButton.Location = new System.Drawing.Point(320, 319);
            this.transformButton.Name = "transformButton";
            this.transformButton.Size = new System.Drawing.Size(300, 29);
            this.transformButton.TabIndex = 3;
            this.transformButton.Text = "transform";
            this.transformButton.UseVisualStyleBackColor = true;
            this.transformButton.Click += new System.EventHandler(this.TransformButtonClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(638, 360);
            this.Controls.Add(this.transformButton);
            this.Controls.Add(this.openImageButton);
            this.Controls.Add(this.transformedImage);
            this.Controls.Add(this.originalImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "CGL3 Nalobin MA group 1493";
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transformedImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox originalImage;
        private System.Windows.Forms.PictureBox transformedImage;
        private System.Windows.Forms.Button openImageButton;
        private System.Windows.Forms.Button transformButton;
    }
}

