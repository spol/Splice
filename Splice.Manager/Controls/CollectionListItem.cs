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
    public partial class CollectionListItem : UserControl
    {
        public string CollectionName
        {
            get
            {
                return CollectionNameLabel.Text;
            }
            set
            {
                CollectionNameLabel.Text = value;
            }
        }

        public VideoCollection Collection { get; set; }

        private Boolean _Selected;
        public Boolean Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                UpdateState();
            }
        }

        public CollectionListItem()
        {
            InitializeComponent();
            CollectionNameLabel.Click += new EventHandler(CollectionListItem_Click);
        }

        private void CollectionListItem_Click(object sender, EventArgs e)
        {
            Selected = true;
        }

        private void UpdateState()
        {
            if (Selected)
            {
                BackColor = Color.Aqua;
                foreach (Control Sibling in Parent.Controls)
                {
                    if ((CollectionListItem)Sibling != this)
                    {
                        ((CollectionListItem)Sibling).Selected = false;
                    }
                }
            }
            else {
                BackColor = SystemColors.Control;
            }
        }
    }
}
