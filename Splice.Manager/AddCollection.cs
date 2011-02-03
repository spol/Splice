using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Splice.Manager.Properties;
using System.Collections.Specialized;
using System.Collections;

namespace Splice.Manager
{
    public partial class AddCollection : Form
    {
        private String CollectionArtwork { get; set; }

        public Collection NewCollection { get; set; }

        public AddCollection()
        {
            InitializeComponent();

            List<KeyValuePair<String, String>> Types = new List<KeyValuePair<String, String>>();

            Types.Add(new KeyValuePair<String, String>("show", "Television Shows"));
            Types.Add(new KeyValuePair<String, String>("movie", "Movies"));
            CollectionType.DataSource = Types;
            CollectionType.DisplayMember = "Value";
            CollectionType.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NewCollection = new Collection()
            {
                Name = CollectionName.Text,
                Type = ((KeyValuePair<String, String>)CollectionType.SelectedItem).Key,
                Paths = PathsListBox.Items.Cast<String>().ToList()
            };

            if (CollectionArtwork == null)
            {
                NewCollection.Artwork = Resources.DefaultVideoCollectionArtwork;
            }
            else
            {
                NewCollection.Artwork = new Bitmap(CollectionArtwork);
            }

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

        private void ChooseArtworkButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ChooseArtwork = new OpenFileDialog();
            ChooseArtwork.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (ChooseArtwork.ShowDialog() == DialogResult.OK)
            {
                CollectionArtwork = ChooseArtwork.FileName;
                ArtworkPreview.Image = new Bitmap(CollectionArtwork);
            }
        }

        private void ClearArtworkButton_Click(object sender, EventArgs e)
        {
            CollectionArtwork = null;
            ArtworkPreview.Image = Resources.DefaultVideoCollectionArtwork;
        }


    }
}
