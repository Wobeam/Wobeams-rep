using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.Model
{
    public class CarById : BaseClass
    {

        // SelectedID property
        private long? _SelectedID;
        public long? SelectedID
        {
            get { return _SelectedID; }
            set 
            { 
                if (_SelectedID != value)
                {
                    _SelectedID = value;
                    RaisePropertyChanged(nameof(SelectedID));
                    LoadCarByID(value ?? -1);
                   
                }
            }
        }

        // SelectedCar property with SelectedCar method
        private Car _SelectedCar;
        public Car SelectedCar
        {
            get { return _SelectedCar; }
            set 
            { 
                _SelectedCar = value; 
                SelectedID = value?.ID; 
                RaisePropertyChanged(nameof(SelectedCar));
            }
        }


        // SelectedCar method
        internal void LoadCarByID(long ID)
        {
            try
            {
                using var conn = new SQLiteConnection(ViewModel.ConnectionString, true);
                conn.Open();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Cars WHERE ID=@ID;";

                cmd.Parameters.AddWithValue("@ID", ID);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    SelectedCar = new Car(reader);
                }
                else
                {
                    SelectedCar = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }


    }
}
