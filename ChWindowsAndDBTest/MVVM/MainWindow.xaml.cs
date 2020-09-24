using MahApps.Metro.Controls;
using MVVM.Model;
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

namespace MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }


        //int id;
        //int discount;

        //private void WriteToPlc_btn_Click(object sender, RoutedEventArgs e)
        //{
        //    Car carToPlc = new Car();
        //    carToPlc.ID = id;
        //    carToPlc.Discount = discount;

        //}















    }
}
