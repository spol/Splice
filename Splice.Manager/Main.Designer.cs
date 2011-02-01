namespace Splice.Manager
{
    partial class Main
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
            this.DebugOutput = new System.Windows.Forms.TextBox();
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.CollectionsListPanel = new Splice.Manager.Controls.CollectionList();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // DebugOutput
            // 
            this.DebugOutput.Location = new System.Drawing.Point(150, 56);
            this.DebugOutput.Multiline = true;
            this.DebugOutput.Name = "DebugOutput";
            this.DebugOutput.Size = new System.Drawing.Size(461, 288);
            this.DebugOutput.TabIndex = 1;
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.CollectionsListPanel);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer.Panel2.Controls.Add(this.DebugOutput);
            this.SplitContainer.Size = new System.Drawing.Size(1001, 454);
            this.SplitContainer.SplitterDistance = 280;
            this.SplitContainer.TabIndex = 3;
            // 
            // CollectionsListPanel
            // 
            this.CollectionsListPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CollectionsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollectionsListPanel.Location = new System.Drawing.Point(0, 0);
            this.CollectionsListPanel.Name = "CollectionsListPanel";
            this.CollectionsListPanel.Size = new System.Drawing.Size(280, 454);
            this.CollectionsListPanel.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 454);
            this.Controls.Add(this.SplitContainer);
            this.Name = "Main";
            this.Text = "Splice Manager";
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            this.SplitContainer.Panel2.PerformLayout();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox DebugOutput;
        private Splice.Manager.Controls.CollectionList CollectionsListPanel;
        private System.Windows.Forms.SplitContainer SplitContainer;
    }
}

