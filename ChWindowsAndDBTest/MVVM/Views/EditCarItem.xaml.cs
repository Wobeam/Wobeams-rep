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


namespace MVVM.Views
{
    /// <summary>
    /// Interaction logic for EditCarItem.xaml
    /// </summary>
    public partial class EditCarItem : MetroWindow
    {
        Car car => DataContext as Car;

        public EditCarItem()
        {
            InitializeComponent();
        }

        //private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        //{
        //    car.SaveIntoDB();
        //    DialogResult = true;
        //    Close();
        //}

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }



        // Using a command to save and exit. This let us enable / disable the button automatically based on any logic we like
        RelayCommand _SaveAndExitCommand;
        public RelayCommand SaveAndExitCommand
        {
            get
            {
                return _SaveAndExitCommand ??= new RelayCommand((param) => SaveAndExitCommand_Execute(param), (param) => SaveAndExitCommand_CanExecute(param));
            }
        }



        public void SaveAndExitCommand_Execute(object parameter)
        {
            if (SaveAndExitCommand_CanExecute(car))
            {
                // We can execute another command from here :-)
                //car.SaveCommand.Execute(car);
                car.SaveIntoDB();
                DialogResult = true;
                Close();

            }
        }

        public bool SaveAndExitCommand_CanExecute(object parameter)
        {
            return !(car?.HasErrors) ?? false;
        }

















        }
}
