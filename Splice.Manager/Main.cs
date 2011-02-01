using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using Splice.Data;
using Splice.Manager.Controls;

namespace Splice.Manager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            CollectionsListPanel.LoadFromServer();

        }


    }
}
