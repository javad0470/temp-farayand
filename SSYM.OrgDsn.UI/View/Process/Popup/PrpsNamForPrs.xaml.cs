﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace SSYM.OrgDsn.UI.View.Process.Popup
{
    /// <summary>
    /// Interaction logic for PrpsNamForPrs.xaml
    /// </summary>
    public partial class PrpsNamForPrs : UserControl
    {
        public PrpsNamForPrs()
        {
            InitializeComponent();
        }

        private void RadTreeView_Initialized_1(object sender, EventArgs e)
        {
            (sender as RadTreeView).CollapseAll();
        }
    }
}
