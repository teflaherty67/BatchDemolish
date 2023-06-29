﻿using Autodesk.Revit.DB;
using System;
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


namespace BatchDemolish
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class frmBatchDemolish : Window
    {
        public frmBatchDemolish(Document doc)
        {
            FilteredElementCollector colPhases = new FilteredElementCollector(doc);
            colPhases.OfCategory(BuiltInCategory.OST_Phases);

            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Utils.ShowForm = true;
            this.Close();
        }       

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Utils.ShowForm = false;
            this.Close();
        }
    }
}
