namespace Splice.Manager.Controls
{
    partial class CollectionList
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
            this.AddCollectionButton = new System.Windows.Forms.Button();
            this.LayoutPanel = new System.Windows.Forms.Panel();
            this.DeleteCollectionButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddCollectionButton
            // 
            this.AddCollectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddCollectionButton.Location = new System.Drawing.Point(4, 406);
            this.AddCollectionButton.Name = "AddCollectionButton";
            this.AddCollectionButton.Size = new System.Drawing.Size(75, 23);
            this.AddCollectionButton.TabIndex = 1;
            this.AddCollectionButton.Text = "Add";
            this.AddCollectionButton.UseVisualStyleBackColor = true;
            this.AddCollectionButton.Click += new System.EventHandler(this.AddCollectionButton_Click);
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.AutoScroll = true;
            this.LayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(252, 403);
            this.LayoutPanel.TabIndex = 3;
            // 
            // DeleteCollectionButton
            // 
            this.DeleteCollectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteCollectionButton.Location = new System.Drawing.Point(88, 406);
            this.DeleteCollectionButton.Name = "DeleteCollectionButton";
            this.DeleteCollectionButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteCollectionButton.TabIndex = 4;
            this.DeleteCollectionButton.Text = "Delete";
            this.DeleteCollectionButton.UseVisualStyleBackColor = true;
            this.DeleteCollectionButton.Click += new System.EventHandler(this.DeleteCollectionButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EditButton.Enabled = false;
            this.EditButton.Location = new System.Drawing.Point(174, 406);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(75, 23);
            this.EditButton.TabIndex = 5;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            // 
            // CollectionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.DeleteCollectionButton);
            this.Controls.Add(this.LayoutPanel);
            this.Controls.Add(this.AddCollectionButton);
            this.Name = "CollectionList";
            this.Size = new System.Drawing.Size(252, 432);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddCollectionButton;
        private System.Windows.Forms.Panel LayoutPanel;
        private System.Windows.Forms.Button DeleteCollectionButton;
        private System.Windows.Forms.Button EditButton;
    }
}
