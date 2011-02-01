namespace Splice.Manager.Controls
{
    partial class CollectionListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CollectionNameLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CollectionNameLabel
            // 
            this.CollectionNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CollectionNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.CollectionNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CollectionNameLabel.Location = new System.Drawing.Point(31, 4);
            this.CollectionNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.CollectionNameLabel.Name = "CollectionNameLabel";
            this.CollectionNameLabel.Size = new System.Drawing.Size(207, 29);
            this.CollectionNameLabel.TabIndex = 1;
            this.CollectionNameLabel.Text = "Collection Name";
            this.CollectionNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Splice.Manager.Properties.Resources.TelevisionCollectionIcon;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // CollectionListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CollectionNameLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CollectionListItem";
            this.Size = new System.Drawing.Size(241, 36);
            this.Click += new System.EventHandler(this.CollectionListItem_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CollectionNameLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
