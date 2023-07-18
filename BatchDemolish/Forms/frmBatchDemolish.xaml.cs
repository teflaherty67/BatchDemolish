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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


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

            foreach (object obj in Enum.GetValues(typeof(ChoicesEnum)))
            {
                RadioButton rb = new RadioButton() { Content = obj, Height = 25, Width = 100 };
                sp.Children.Add(rb);
                rb.Checked += new RoutedEventHandler(rb_Checked);
                rb.Unchecked += new RoutedEventHandler(rb_Unchecked);
            }
        }

        void rb_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.Write((sender as RadioButton).Content.ToString() + " checked.");
        }

        void rb_Checked(object sender, RoutedEventArgs e)
        {
            Console.Write((sender as RadioButton).Content.ToString() + " unchecked.");
        }

        private void showChoice_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton rb in sp.Children)
            {
                if (rb.IsChecked == true)
                {
                    MessageBox.Show(rb.Content.ToString());
                    break;
                }
            }
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

    public enum ChoicesEnum
    {        
        Choice1,
        Choice2,
        Choice3,
    }
}
