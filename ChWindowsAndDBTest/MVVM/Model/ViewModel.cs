using MahApps.Metro.Controls.Dialogs;
using MVVM.Properties;
using MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;

namespace MVVM.Model
{
    public class ViewModel : BaseClass
    {
        public static ViewModel Instance;

        public ViewModel()
        {
            Instance ??= this;
            Requery();
        }


        public ObservableCollection<Car> Cars { get; } = new ObservableCollection<Car>();

        public  static string ConnectionString => $"Data Source=\"{Settings.Default.DataBaseFile}\";Version=3;FailIfMissing=True;";


        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ??= new RelayCommand((param) => AddCar()); }
        }

        void AddCar()
        {
            var car = new Car();

            var editCarDilaog = new EditCarItem() { DataContext = car };

            editCarDilaog.ShowDialog();

            // Let's just requery for now, as there are so many possibilities else.
            Requery();
        }



        #region Sqlite

        private RelayCommand _RequeryCommand;
        public RelayCommand RequeryCommand
        {
            get { return _RequeryCommand ??= new RelayCommand((param) => Requery()); }
        }


        public void Requery()
        {
            try
            {
                Cars.Clear();

                using var conn = new SQLiteConnection(ConnectionString, true);
                conn.Open();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Cars;";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cars.Add(new Car(reader));
                }
            }
            catch  (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            } 
        }

        RelayCommand _EditCarCommand;
        public RelayCommand EditCarCommand
        {
            get
            {
                return _EditCarCommand ??= new RelayCommand((param) => { EditCar(param as Car); }, (param) => param is Car); // We just allow edit if we have a car selected
            }
        }

        void EditCar(Car car)
        {
            car.EditCommand.Execute(null); // no parameter needed here so we leave it null
        }

        #endregion



        // Service command and method
        private RelayCommand _ServiceCommand;
        public RelayCommand ServiceCommand
        {
            get { return _ServiceCommand ??= new RelayCommand((param) => Service_method()); }
        }


        void Service_method()
        {

            using var conn = new SQLiteConnection(ConnectionString, true);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Cars;";
                      
            cmd.ExecuteNonQuery(); // обязательно!!! иначе не будет исполняться delete command

            cmd.CommandText = "UPDATE SQLITE_SEQUENCE SET SEQ = 0 WHERE NAME ='Cars';";

            //cmd.CommandText = "DELETE FROM SQLITE_SEQUENCE WHERE NAME = 'Cars';"; // тоже работает

            cmd.ExecuteNonQuery(); // обязательно!!! иначе не будет исполняться update command


        }


       





    }
}
