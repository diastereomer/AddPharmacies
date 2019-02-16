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
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace AddPharmacy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            searchPhamName = "";
            searchPhoneNum = "";
            searchStoreNum = "";
            searchZip = "";
            searchCity = "";
            company = "";
            mrn = "";
            address = "";
            state = "";

        }
        internal static string searchPhamName;
        internal static string searchPhoneNum;
        internal static string searchStoreNum;
        internal static string searchZip;
        internal static string searchCity;
        internal static string company;
        internal static string mrn;
        internal static string address;
        internal static string state;
        internal static string setValue;
        private void SearchPharm_Click(object sender, RoutedEventArgs e)
        {
            SearchParmacyWindow spw = new SearchParmacyWindow(SearchGrid, searchPhamName, searchPhoneNum, searchStoreNum, searchZip, searchCity);
            if ((bool)spw.ShowDialog())
            {
                SearchTab.IsSelected = true;
                AddingTab.IsSelected = false;
            }
        }

        private void AddPharm_Click(object sender, RoutedEventArgs e)
        {
            AddPharmacyWindow apw = new AddPharmacyWindow(AddingGrid, searchPhamName, searchPhoneNum, searchStoreNum, searchZip, searchCity, company, mrn, address, state);
            if ((bool)apw.ShowDialog())
            {
                AddingTab.IsSelected = true;
                SearchTab.IsSelected = false;
            }
        }

        public static DataGridCell GetDataGridCell(DataGridCellInfo cellInfo)
        {
            if (cellInfo.IsValid == false)
            {
                return null;
            }
            var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
            if (cellContent == null)
            {
                return null;
            }
            return cellContent.Parent as DataGridCell;
        }

        private void SearchGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SearchGrid.SelectedItem != null && SearchGrid.Items.Count != 0 && 
                e.GetPosition(SearchGrid).X < GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).X+GetDataGridCell(SearchGrid.CurrentCell).ActualWidth
                && e.GetPosition(SearchGrid).X > GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).X
                && e.GetPosition(SearchGrid).Y>GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).Y
                && e.GetPosition(SearchGrid).Y < GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).Y + GetDataGridCell(SearchGrid.CurrentCell).ActualHeight)
            {
                //MessageBox.Show(GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).X + "   " + GetDataGridCell(SearchGrid.CurrentCell).TranslatePoint(new Point(0, 0), SearchGrid).Y
                //    + "   " + GetDataGridCell(SearchGrid.CurrentCell).PointToScreen(new Point(0, 0)).X + " " + GetDataGridCell(SearchGrid.CurrentCell).PointToScreen(new Point(0, 0)).Y + "      " + e.GetPosition(SearchGrid).X + "   " + e.GetPosition(SearchGrid).Y);
                DataRowView dr = (DataRowView)SearchGrid.SelectedItem;
                string title = dr["Title"].ToString();
                string firstName = dr["FirstName"].ToString();
                string middleInitial = dr["MiddleInitial"].ToString();
                string lastName = dr["LastName"].ToString();
                string title1 = dr["Title1"].ToString();
                string storeName = dr["StoreName"].ToString();
                string address = dr["Address"].ToString();
                string city = dr["City"].ToString();
                string phone = dr["Phone"].ToString();
                string state = dr["State"].ToString();
                string zip = dr["Zip"].ToString();
                string company = dr["Company"].ToString();
                string pharmacyType = dr["PharmacyType"].ToString();
                string parsedStoreName = dr["ParsedStoreName"].ToString();
                string storeNumber = dr["StoreNumber"].ToString();
                string columnName = SearchGrid.CurrentCell.Column.Header.ToString();
                setValue = dr[columnName].ToString();
                //MessageBox.Show(title + firstName + middleInitial + lastName + title1 + storeName + address + city + phone + state + zip + company + pharmacyType + parsedStoreName + storeNumber);
                Set_Value sv = new Set_Value(columnName, dr[columnName].ToString());
                if (sv.ShowDialog() == true)
                {
                    if (updateRecords(columnName, storeName, address, city, phone, state, zip, company, pharmacyType))
                    {

                        SqlConnection searchCon = new SqlConnection();
                        searchCon.ConnectionString = @"Data Source=DSI-CPR-01;Initial Catalog= DSICPR; Integrated Security= true";
                        // try the connection manager
                        try
                        {
                            if (searchCon != null)
                            {
                                searchCon.Open();
                                //  MessageBox.Show("good");
                            }
                        }
                        catch
                        {
                            System.Windows.MessageBox.Show("Connection string is wrong!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        string searchText = @"declare @PharmacyName varchar(80)='" + searchPhamName + @"';
                                    declare @PhoneNumber varchar(20)='" + searchPhoneNum + @"';
                                    Declare @StoreNumber varchar(20)='" + searchStoreNum + @"';
                                    Declare @Zip varchar(20)='" + searchZip + @"';
                                    declare @City varchar(80)='" + searchCity + @"';
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
                            SearchGrid.ItemsSource = searchTable.DefaultView;
                            searchCon.Close();
                        }
                        catch
                        {
                            searchCon.Close();
                        }
                    };
                }
            }
            //  MessageBox.Show(SearchGrid.CurrentCell.Column.Header.ToString());
        }

        private static bool updateRecords(string columnName, string storeName, string address, string city, string phone, string state, string zip, string company, string pharmacyType)
        {
            SqlConnection update = new SqlConnection();
            update.ConnectionString = @"Data Source= DSI-CPR-01; Initial Catalog= DSICPR; Integrated Security =true;";
            try
            {
                if (update != null)
                {
                    update.Open();
                    //  MessageBox.Show("good");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Cannot connect to the databases, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            string sqlQ=""; 
            if (columnName != "Phone")
            {
                sqlQ = @"Declare @sqltext NVarchar(max)=N'update dbo.PharmacyList
                                set " + columnName + @" = ''" + setValue + @"''
                                where ltrim(rtrim([StoreName]))=ltrim(rtrim(''" + storeName + @"''))
                                       and ltrim(rtrim([Address]))=ltrim(rtrim(''" + address + @"'' ))
                                      and ltrim(rtrim([City]))=ltrim(rtrim(''" + city + @"'' ))
                                      and ltrim(rtrim( [Phone]))=ltrim(rtrim(''" + phone + @"'' ))
                                      and ltrim(rtrim([State]))=ltrim(rtrim(''" + state + @"'' ))
                                      and ltrim(rtrim([Zip]))=ltrim(rtrim(''" + zip + @"'' ))
                                      and ltrim(rtrim([Company]))=ltrim(rtrim(''" + company + @"'' ))
                                      and ltrim(rtrim([PharmacyType]))=ltrim(rtrim(''" + pharmacyType + @"'' ))
                                ';
                                Execute sp_executesql @sqltext";
            }
            else
            {
                sqlQ = @"
                                Execute sp_executesql @sqltext1;
                                Declare @sqltext NVarchar(max)=N'update dbo.PharmacyList
                                set " + columnName + @" = ''" + setValue + @"''
                                where ltrim(rtrim([StoreName]))=ltrim(rtrim(''" + storeName + @"''))
                                       and ltrim(rtrim([Address]))=ltrim(rtrim(''" + address + @"'' ))
                                      and ltrim(rtrim([City]))=ltrim(rtrim(''" + city + @"'' ))
                                      and ltrim(rtrim( [Phone]))=ltrim(rtrim(''" + phone + @"'' ))
                                      and ltrim(rtrim([State]))=ltrim(rtrim(''" + state + @"'' ))
                                      and ltrim(rtrim([Zip]))=ltrim(rtrim(''" + zip + @"'' ))
                                      and ltrim(rtrim([Company]))=ltrim(rtrim(''" + company + @"'' ))
                                      and ltrim(rtrim([PharmacyType]))=ltrim(rtrim(''" + pharmacyType + @"'' ))
                                ';
                                Execute sp_executesql @sqltext;
                                ";
            }
            SqlCommand updateQuery = new SqlCommand(sqlQ, update);
            try
            {
                updateQuery.ExecuteNonQuery();
                update.Close();
                // if( updateQuery.ExecuteNonQuery()==1)
                //StreamWriter sw = new StreamWriter(@"q:\test.txt");
                //sw.WriteLine(updateQuery.CommandText);
                //sw.Close();
            }

            catch
            {
                update.Close();
                System.Windows.MessageBox.Show("You do hot have update permission, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            switch (columnName)
            {    
                case "StoreName":
                    searchPhamName = setValue;
                    break;
                case "Phone":
                    searchPhoneNum = setValue;
                    break;
                case "StoreNumber":
                    searchStoreNum = setValue;
                    break;
                case "Zip":
                    searchZip = setValue;
                    break;
                case "City":
                    searchCity = setValue;
                    break;
            }
            return true;
        }

        private void AddingGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AddingGrid.SelectedItem != null && AddingGrid.Items.Count != 0 &&
                e.GetPosition(AddingGrid).X < GetDataGridCell(AddingGrid.CurrentCell).TranslatePoint(new Point(0, 0), AddingGrid).X + GetDataGridCell(AddingGrid.CurrentCell).ActualWidth
                && e.GetPosition(AddingGrid).X > GetDataGridCell(AddingGrid.CurrentCell).TranslatePoint(new Point(0, 0), AddingGrid).X
                && e.GetPosition(AddingGrid).Y > GetDataGridCell(AddingGrid.CurrentCell).TranslatePoint(new Point(0, 0), AddingGrid).Y
                && e.GetPosition(AddingGrid).Y < GetDataGridCell(AddingGrid.CurrentCell).TranslatePoint(new Point(0, 0), AddingGrid).Y + GetDataGridCell(AddingGrid.CurrentCell).ActualHeight)
            {
                DataRowView dr = (DataRowView)AddingGrid.SelectedItem;
                string title = dr["Title"].ToString();
                string firstName = dr["FirstName"].ToString();
                string middleInitial = dr["MiddleInitial"].ToString();
                string lastName = dr["LastName"].ToString();
                string title1 = dr["Title1"].ToString();
                string storeName = dr["StoreName"].ToString();
                string address = dr["Address"].ToString();
                string city = dr["City"].ToString();
                string phone = dr["Phone"].ToString();
                string state = dr["State"].ToString();
                string zip = dr["Zip"].ToString();
                string company = dr["Company"].ToString();
                string pharmacyType = dr["PharmacyType"].ToString();
                string parsedStoreName = dr["ParsedStoreName"].ToString();
                string storeNumber = dr["StoreNumber"].ToString();
                string columnName = AddingGrid.CurrentCell.Column.Header.ToString();
                //MessageBox.Show(title + firstName + middleInitial + lastName + title1 + storeName + address + city + phone + state + zip + company + pharmacyType + parsedStoreName + storeNumber);
                Set_Value sv = new Set_Value(columnName, dr[columnName].ToString());
                if (sv.ShowDialog() == true)
                {
                    if (updateRecords(columnName, storeName, address, city, phone, state, zip, company, pharmacyType))
                    {

                        SqlConnection searchCon = new SqlConnection();
                        searchCon.ConnectionString = @"Data Source=DSI-CPR-01;Initial Catalog= DSICPR; Integrated Security= true";
                        // try the connection manager
                        try
                        {
                            if (searchCon != null)
                            {
                                searchCon.Open();
                                //  MessageBox.Show("good");
                            }
                        }
                        catch
                        {
                            System.Windows.MessageBox.Show("Connection string is wrong!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        string searchText = @"declare @PharmacyName varchar(80)='" + searchPhamName + @"';
                                    declare @PhoneNumber varchar(20)='" + searchPhoneNum + @"';
                                    Declare @StoreNumber varchar(20)='" + searchStoreNum + @"';
                                    Declare @Zip varchar(20)='" + searchZip + @"';
                                    declare @City varchar(80)='" + searchCity + @"';
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
                            AddingGrid.ItemsSource = searchTable.DefaultView;
                            searchCon.Close();
                        }
                        catch
                        {
                            searchCon.Close();
                        }
                    };
                }
            }
            //  MessageBox.Show(SearchGrid.CurrentCell.Column.Header.ToString());
        }

        private void addTransfer_Click(object sender, RoutedEventArgs e)
        {
            TransferRX transfer = new TransferRX(searchPhoneNum, mrn );
            transfer.ShowDialog();
        }
    }
}


