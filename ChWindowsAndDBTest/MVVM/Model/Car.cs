using MahApps.Metro.Controls.Dialogs;
using MVVM.Views;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public class Car : BaseClass    
    {

        #region Constructor
        public Car()
        { 
            Validate();

           

        }

        public Car(SQLiteDataReader reader)
        {
            ID = reader.GetNullableLong("ID");
            Model = reader.GetString("Model");
            Manufacturer = reader.GetString("Manufacturer");
            Price = reader.GetNullableDouble("Price");
            Discount = reader.GetInt("Discount", 0);

            Validate();

            NeedsSave = false;
        }
        #endregion

        public new void RaisePropertyChanged(string PropertyName)
        {
            base.RaisePropertyChanged(PropertyName);
            
            if (PropertyName != nameof(NeedsSave))
            {
                NeedsSave = true;
            }
        }


        #region Properties definition

        private long? _ID;
        public long? ID
        {
            get { return _ID; }
            set { _ID = value; RaisePropertyChanged(nameof(ID)); }
        }

        private bool _NeedsSave;
        public bool NeedsSave
        {
            get { return _NeedsSave; }
            set { _NeedsSave = value; RaisePropertyChanged(nameof(NeedsSave)); }
        }

        private string _Manufacturer;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; RaisePropertyChanged(nameof(Manufacturer)); Validate(); }
        }

        private string _Model;
        public string Model
        {
            get { return _Model; }
            set { _Model = value; RaisePropertyChanged(nameof(Model)); Validate(); }
        }

        private double? _Price;
        public double? Price
        {
            get { return _Price; }
            set { _Price = value; RaisePropertyChanged(nameof(Price)); Validate(); }
        }


        private int? _Discount;
        public int? Discount
        {
            get { return _Discount; }
            set { _Discount = value; RaisePropertyChanged(nameof(Discount)); Validate(); }
        }

        #endregion



        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get { return _EditCommand ??= new RelayCommand((param) => Edit()); }
        }

        void Edit()
        {
            var editcardialog = new EditCarItem() { DataContext = this };

            editcardialog.ShowDialog();

            ViewModel.Instance.Requery();
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ??= new RelayCommand((param) => Delete(), (param) => ID.HasValue); }
        }


        // Delete

       
        async void  Delete()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Agree",
                NegativeButtonText = "Abort",
                FirstAuxiliaryButtonText = "Cancel",
                MaximumBodyHeight = 100,
                //ColorScheme = MetroDialogOptions.ColorScheme
            };


            if (await DialogCoordinator.Instance.ShowMessageAsync(this, "Delete this car?", "Do you really want to do this", MessageDialogStyle.AffirmativeAndNegative, mySettings) == MessageDialogResult.Affirmative)
            {
                using var conn = new SQLiteConnection(ViewModel.ConnectionString, true);
                conn.Open();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = "DELETE FROM Cars WHERE ID = @ID;";

                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();

                ViewModel.Instance.Requery();
            }
        }


        #region Validate

        void Validate()
        {
            ClearErrors(nameof(Manufacturer));
            ClearErrors(nameof(Model));
            ClearErrors(nameof(Price));
            ClearErrors(nameof(Discount));

            if (string.IsNullOrWhiteSpace(Manufacturer)) AddError(nameof(Manufacturer), "Please fill in");
            if (string.IsNullOrWhiteSpace(Model)) AddError(nameof(Model), "Please fill in");
            if (! Price.HasValue) AddError(nameof(Price), "Please fill in");
            if (!Discount.HasValue) AddError(nameof(Discount), "Please fill in");

            if (Price <= 0) AddError(nameof(Price), "A car for free :-)");
            if (Discount == 0) AddError(nameof(Discount), "No discount :-(");
        }

        #endregion




        #region Save

        RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ??= new RelayCommand((param) => SaveIntoDB(), (param) => NeedsSave); }
        }


        public void SaveIntoDB()
        {
            using var conn = new SQLiteConnection(ViewModel.ConnectionString, true);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "REPLACE INTO Cars(ID, Model, Manufacturer, Price, Discount) VALUES(@ID, @Model, @Manufacturer, @Price, @Discount);";

            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@Model", Model);
            cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@Discount", Discount);

            cmd.ExecuteNonQuery();

            ID = conn.LastInsertRowId;
            NeedsSave = false;

            conn.Close();
        }

        #endregion


        

    }
}
