using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Splice.Manager
{
    public partial class AddCollection : Form
    {
        public string NewCollectionName { get; set; }
        public AddCollection()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NewCollectionName = CollectionName.Text;
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CollectionName_TextChanged(object sender, EventArgs e)
        {
            UpdateAddButtonStatus();
        }

        private void UpdateAddButtonStatus()
        {
            AddButton.Enabled = CollectionName.Text.Length > 0 && PathsListBox.Items.Count > 0;
        }

        private void AddPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog PathSelector = new FolderBrowserDialog();
            if (PathSelector.ShowDialog() == DialogResult.OK)
            {
                if (!PathsListBox.Items.Contains(PathSelector.SelectedPath))
                {
                    PathsListBox.Items.Add(PathSelector.SelectedPath);
                }
                
            }
            UpdateAddButtonStatus();
        }

        private void RemovePathButton_Click(object sender, EventArgs e)
        {
            if (PathsListBox.SelectedItem != null)
            {
                PathsListBox.Items.RemoveAt(PathsListBox.SelectedIndex);
            }
            UpdateAddButtonStatus();
        }

    }
}
