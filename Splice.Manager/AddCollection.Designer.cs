namespace Splice.Manager
{
    partial class AddCollection
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
            this.label1 = new System.Windows.Forms.Label();
            this.CollectionName = new System.Windows.Forms.TextBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.PathsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddPathButton = new System.Windows.Forms.Button();
            this.RemovePathButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ArtworkPreview = new System.Windows.Forms.PictureBox();
            this.ChooseArtworkButton = new System.Windows.Forms.Button();
            this.ClearArtworkButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.CollectionType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Collection Name";
            // 
            // CollectionName
            // 
            this.CollectionName.Location = new System.Drawing.Point(103, 12);
            this.CollectionName.Name = "CollectionName";
            this.CollectionName.Size = new System.Drawing.Size(270, 20);
            this.CollectionName.TabIndex = 1;
            this.CollectionName.TextChanged += new System.EventHandler(this.CollectionName_TextChanged);
            // 
            // AddButton
            // 
            this.AddButton.Enabled = false;
            this.AddButton.Location = new System.Drawing.Point(189, 372);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(98, 23);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add Collection";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(293, 372);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // PathsListBox
            // 
            this.PathsListBox.FormattingEnabled = true;
            this.PathsListBox.Location = new System.Drawing.Point(12, 83);
            this.PathsListBox.Name = "PathsListBox";
            this.PathsListBox.Size = new System.Drawing.Size(361, 121);
            this.PathsListBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Paths";
            // 
            // AddPathButton
            // 
            this.AddPathButton.Location = new System.Drawing.Point(12, 210);
            this.AddPathButton.Name = "AddPathButton";
            this.AddPathButton.Size = new System.Drawing.Size(75, 23);
            this.AddPathButton.TabIndex = 6;
            this.AddPathButton.Text = "Add Path...";
            this.AddPathButton.UseVisualStyleBackColor = true;
            this.AddPathButton.Click += new System.EventHandler(this.AddPathButton_Click);
            // 
            // RemovePathButton
            // 
            this.RemovePathButton.Location = new System.Drawing.Point(103, 210);
            this.RemovePathButton.Name = "RemovePathButton";
            this.RemovePathButton.Size = new System.Drawing.Size(75, 23);
            this.RemovePathButton.TabIndex = 7;
            this.RemovePathButton.Text = "Remove";
            this.RemovePathButton.UseVisualStyleBackColor = true;
            this.RemovePathButton.Click += new System.EventHandler(this.RemovePathButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Artwork";
            // 
            // ArtworkPreview
            // 
            this.ArtworkPreview.Image = global::Splice.Manager.Properties.Resources.DefaultVideoCollectionArtwork;
            this.ArtworkPreview.Location = new System.Drawing.Point(210, 267);
            this.ArtworkPreview.Name = "ArtworkPreview";
            this.ArtworkPreview.Size = new System.Drawing.Size(158, 81);
            this.ArtworkPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ArtworkPreview.TabIndex = 9;
            this.ArtworkPreview.TabStop = false;
            // 
            // ChooseArtworkButton
            // 
            this.ChooseArtworkButton.Location = new System.Drawing.Point(12, 294);
            this.ChooseArtworkButton.Name = "ChooseArtworkButton";
            this.ChooseArtworkButton.Size = new System.Drawing.Size(75, 23);
            this.ChooseArtworkButton.TabIndex = 10;
            this.ChooseArtworkButton.Text = "Choose...";
            this.ChooseArtworkButton.UseVisualStyleBackColor = true;
            this.ChooseArtworkButton.Click += new System.EventHandler(this.ChooseArtworkButton_Click);
            // 
            // ClearArtworkButton
            // 
            this.ClearArtworkButton.Location = new System.Drawing.Point(12, 325);
            this.ClearArtworkButton.Name = "ClearArtworkButton";
            this.ClearArtworkButton.Size = new System.Drawing.Size(75, 23);
            this.ClearArtworkButton.TabIndex = 11;
            this.ClearArtworkButton.Text = "Clear";
            this.ClearArtworkButton.UseVisualStyleBackColor = true;
            this.ClearArtworkButton.Click += new System.EventHandler(this.ClearArtworkButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Collection Type";
            // 
            // CollectionType
            // 
            this.CollectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollectionType.FormattingEnabled = true;
            this.CollectionType.Items.AddRange(new object[] {
            "Shows"});
            this.CollectionType.Location = new System.Drawing.Point(103, 39);
            this.CollectionType.Name = "CollectionType";
            this.CollectionType.Size = new System.Drawing.Size(130, 21);
            this.CollectionType.TabIndex = 13;
            // 
            // AddCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 405);
            this.Controls.Add(this.CollectionType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ClearArtworkButton);
            this.Controls.Add(this.ChooseArtworkButton);
            this.Controls.Add(this.ArtworkPreview);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RemovePathButton);
            this.Controls.Add(this.AddPathButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PathsListBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.CollectionName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddCollection";
            this.Text = "Add Collection";
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CollectionName;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ListBox PathsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddPathButton;
        private System.Windows.Forms.Button RemovePathButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox ArtworkPreview;
        private System.Windows.Forms.Button ChooseArtworkButton;
        private System.Windows.Forms.Button ClearArtworkButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CollectionType;
    }
}