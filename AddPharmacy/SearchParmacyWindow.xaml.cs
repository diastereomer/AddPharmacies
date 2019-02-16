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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
namespace AddPharmacy
{
    /// <summary>
    /// Interaction logic for SearchParmacyWindow.xaml
    /// </summary>
    public partial class SearchParmacyWindow : Window
    {
        public SearchParmacyWindow(DataGrid dg,  string searchPhamName, string searchPhoneNum, string searchStoreNum, string searchZip, string searchCity)
        {
            InitializeComponent();
            sdg = dg;
            pName = searchPhamName;
            pNumber = searchPhoneNum;
            sNumber = searchStoreNum;
            zip = searchZip;
            city = searchCity;
            SearchPharmName.Text = searchPhamName;
            SearchPhone.Text = searchPhoneNum;
            SearchStoreNumber.Text = searchStoreNum;
            SearchZip.Text = searchZip;
            SearchCity.Text = searchCity;
        }
        internal static string pName;
        internal static string pNumber;
        internal static string sNumber;
        internal static string zip;
        internal static string city;
        internal static DataGrid sdg; 
        
        private void SearchCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        // connect to the sql server and run  the query to show result in the search datagrid
        private void SearchOK_Click(object sender, RoutedEventArgs e)
        {
            string pName = SearchPharmName.Text;
            string pNumber = SearchPhone.Text;
            string sNumber = SearchStoreNumber.Text;
            string zip = SearchZip.Text;
            string city= SearchCity.Text;
            MainWindow.searchPhamName = SearchPharmName.Text;
            MainWindow.searchPhoneNum = SearchPhone.Text;
            MainWindow.searchStoreNum = SearchStoreNumber.Text;
            MainWindow.searchZip = SearchZip.Text;
            MainWindow.searchCity= SearchCity.Text;

            SqlConnection searchCon = new SqlConnection();
            searchCon.ConnectionString = @"Data Source=DSI-CPR-01;Initial Catalog= DSICPR; Integrated Security= true";
        // try the connection manager
            try
            {
                if (searchCon !=null)
                {
                    searchCon.Open();
                  //  MessageBox.Show("good");
                }
            }
            catch 
            {
                System.Windows.MessageBox.Show("Cannot connect to the databases, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.DialogResult=false;
                this.Close();
            }
            string searchText = @"declare @PharmacyName varchar(80)='"+pName+@"';
                                    declare @PhoneNumber varchar(20)='"+pNumber+@"';
                                    Declare @StoreNumber varchar(20)='"+sNumber+@"';
                                    Declare @Zip varchar(20)='"+zip+@"';
                                    declare @City varchar(80)='"+city+@"';
                                    Select * from [dbo].[PharmacyList]
                                    where (@PharmacyName is null or ltrim(rtrim(@PharmacyName))=''or [StoreName] like '%'+ltrim(rtrim(@PharmacyName))+'%')
                                    and (@PhoneNumber  is null  or ltrim(rtrim(@PhoneNumber))=''or [Phone] = ltrim(rtrim(@PhoneNumber)))
                                    and (@StoreNumber  is null  or ltrim(rtrim(@StoreNumber))=''or StoreNumber like '%'+ltrim(rtrim(@StoreNumber))+'%')
                                    and (@Zip  is null  or ltrim(rtrim(@Zip))=''or Zip like '%'+ ltrim(rtrim(@Zip))+'%')
                                    and (@City is null or ltrim(rtrim(@City))=''or [City] like '%'+ltrim(rtrim(@City))+'%')";
            try
            {
                SqlCommand searchCommand = new SqlCommand(searchText, searchCon);
                DataTable searchTable = new DataTable();
                searchTable.Load(searchCommand.ExecuteReader());
                sdg.ItemsSource = searchTable.DefaultView;
            }
            catch 
            {
                System.Windows.MessageBox.Show("You do hot have select permission, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.DialogResult = false;
                this.Close();
            }
            searchCon.Close();
            this.DialogResult = true;
            this.Close();
        }
    }
}
