using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Splice.Data;

namespace Splice.Manager.Controls
{
    public partial class CollectionList : UserControl
    {

        public CollectionList()
        {
            InitializeComponent();
        }

        private void AddCollectionButton_Click(object sender, EventArgs e)
        {
            AddCollection AddCollectionDialog = new AddCollection();

            if (AddCollectionDialog.ShowDialog() == DialogResult.OK)
            {
                Service.CreateCollection(AddCollectionDialog.NewCollection);
                LoadFromServer();
            }
        }

        public void LoadFromServer()
        {
            List<VideoCollection> Collections = Service.GetCollectionsList();


            LayoutPanel.SuspendLayout();
            LayoutPanel.Controls.Clear();
            foreach (VideoCollection Collection in Collections)
            {
                CollectionListItem CollectionLabel = new CollectionListItem();
                CollectionLabel.CollectionName = Collection.Title;
                CollectionLabel.Collection = Collection;

                CollectionLabel.Width = LayoutPanel.Width;
                CollectionLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                CollectionLabel.Location = new Point(0, LayoutPanel.Controls.Count * 36);
                LayoutPanel.Controls.Add(CollectionLabel);
            }
            LayoutPanel.ResumeLayout();
        }

        public CollectionListItem SelectedItem
        {
            get
            {
                foreach (Control Ctrl in LayoutPanel.Controls)
                {
                    if (((CollectionListItem)Ctrl).Selected)
                    {
                        return (CollectionListItem)Ctrl;
                    }
                }
                return null;
            }
        }

        private void DeleteCollectionButton_Click(object sender, EventArgs e)
        {
            foreach (Control Ctrl in LayoutPanel.Controls)
            {
                if (((CollectionListItem)Ctrl).Selected)
                {
                    Service.DeleteCollection(SelectedItem.Collection.Id);
                    break;

                }
            }
            LoadFromServer();
        }
    }
}
