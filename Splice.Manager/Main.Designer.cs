namespace WinPlexManager
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
            this.CollectionsListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DebugOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CollectionsListPanel
            // 
            this.CollectionsListPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.CollectionsListPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.CollectionsListPanel.Location = new System.Drawing.Point(0, 0);
            this.CollectionsListPanel.Name = "CollectionsListPanel";
            this.CollectionsListPanel.Size = new System.Drawing.Size(200, 454);
            this.CollectionsListPanel.TabIndex = 0;
            // 
            // DebugOutput
            // 
            this.DebugOutput.Location = new System.Drawing.Point(418, 110);
            this.DebugOutput.Multiline = true;
            this.DebugOutput.Name = "DebugOutput";
            this.DebugOutput.Size = new System.Drawing.Size(461, 288);
            this.DebugOutput.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 454);
            this.Controls.Add(this.DebugOutput);
            this.Controls.Add(this.CollectionsListPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel CollectionsListPanel;
        private System.Windows.Forms.TextBox DebugOutput;
    }
}

