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
using System.IO;
using System.Text.RegularExpressions;
namespace AddPharmacy
{
    /// <summary>
    /// Interaction logic for AddPharmacyWindow.xaml
    /// </summary>
    public partial class AddPharmacyWindow : Window
    {
        public AddPharmacyWindow(DataGrid AddingGrid, string sPhamName, string sPhoneNum,string sStoreNum,string sZip,string sCity,  string sCompany,
         string sMRN,string sAddress,string sState)
        {
            InitializeComponent();
            adg = AddingGrid;
            addName = sPhamName;
            addPNumber = sPhoneNum;
            addSNumber = sStoreNum;
            addZip = sZip;
            addCity = sCity;
            addAddress = sAddress;
            addCompany = sCompany;
            addMRN = sMRN;
            addState = sState;
            AddPharmName.Text = sPhamName;
            AddPhoneNumber.Text = sPhoneNum;
            AddStoreNumber.Text = sStoreNum;
            AddZip.Text = sZip;
            AddCity.Text = sCity;
            AddState.Text = sState;
            AddCompany.Text = sCompany;
            AddAddress.Text = sAddress;
            AddMRN.Text = sMRN;
        }
        internal static string addName;
        internal static string addPNumber;
        internal static string addSNumber;
        internal static string addZip;
        internal static string addCity;
        internal static string addCompany;
        internal static string addMRN;
        internal static string addAddress;
        internal static string addState;
        internal static DataGrid adg; 

        private void AddCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void AddOk_Click(object sender, RoutedEventArgs e)
        {
            addName = AddPharmName.Text;
            addPNumber = AddPhoneNumber.Text;
            addSNumber = AddStoreNumber.Text;
            addZip = AddZip.Text;
            addCity = AddCity.Text;
            addAddress = AddAddress.Text;
            addCompany = AddCompany.Text;
            addMRN = AddMRN.Text;
            addState = AddState.Text;
            if (addName.Contains(@"'")||addPNumber.Contains(@"'")||addSNumber.Contains(@"'")||addZip.Contains(@"'")||addCity.Contains(@"'")||addAddress.Contains(@"'")||addCompany.Contains(@"'"))
            {
                System.Windows.Forms.MessageBox.Show(@"Remove ' please!!!");
                return;
            }
            if (addName.Contains("\"") || addPNumber.Contains("\"") || addSNumber.Contains("\"") || addZip.Contains("\"") || addCity.Contains("\"") || addAddress.Contains("\"") || addCompany.Contains("\""))
            {
                System.Windows.Forms.MessageBox.Show("Remove \" please!!!");
                return;
            }
            if ((addName+" #"+addSNumber).Length>=25)
            {
                System.Windows.Forms.MessageBox.Show("Store name is too long!!!");
                return;
            }
            string phoneFormat =@"^[1-9]\d{2}-\d{3}-\d{4}$";
            Regex rg = new Regex(phoneFormat);
            if (!rg.IsMatch(addPNumber))
            {
                System.Windows.Forms.MessageBox.Show("Phone number is wrong!!!");
                return;
            }
            string pharmType = AddPharmacyType.Text;
            MainWindow.searchPhamName = AddPharmName.Text;
            MainWindow.searchPhoneNum = AddPhoneNumber.Text;
            MainWindow.searchStoreNum = AddStoreNumber.Text;
            MainWindow.searchZip = AddZip.Text;
            MainWindow.searchCity = AddCity.Text;
            MainWindow.address = AddAddress.Text;
            MainWindow.state = AddState.Text;
            MainWindow.mrn = AddMRN.Text;
            MainWindow.company = AddCompany.Text;
            SqlConnection addCon = new SqlConnection();
           // MessageBox.Show(AddState.Text + AddPharmacyType.Text);
            addCon.ConnectionString = @"Data Source=sqlserver;Initial Catalog= dbname; Integrated Security= true";
            // try the connection manager
            try
            {
                if (addCon != null)
                {
                    addCon.Open();
                    //MessageBox.Show("good");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Cannot connect to the databases, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.DialogResult = false;
                this.Close();
            }
            if (addName == "" || addName == null || addPNumber == "" || addPNumber == null || addZip == "" || addZip == null || addCity == "" || addCity == null || addAddress == "" || addAddress == null
            || addCompany == "" || addCompany == null || addState == "" || addState == null)
            {
                MessageBox.Show("Please fill the required items!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string addText = @"Declare @addSNumber varchar(255)= '" + addSNumber + @"';
                                    if ltrim(rtrim(@addSNumber))!='' and  @addSNumber is not null
                                    set @addSNumber='#'+@addSNumber;
                                    else
                                    set @addSNumber='';
                                    DECLARE	@Title VARCHAR(255) = NULL							
		                            ,@FirstName VARCHAR(255)	= NULL						
		                            ,@MiddleInitial VARCHAR(255)	= NULL					
		                            ,@LastName VARCHAR(255) = NULL							
		                            ,@Title1 VARCHAR(255) = NULL							
		                            ,@Phone VARCHAR(255) = ltrim(rtrim('" + addPNumber + @"'))		
		                            ,@StoreName VARCHAR(255) = ltrim(rtrim('" + addName + @" '+ @addSNumber))		
		                            ,@Address VARCHAR(255) = ltrim(rtrim('" + addAddress + @"'))		
		                            ,@City VARCHAR(255) = ltrim(rtrim('" + addCity + @"'))					
		                            ,@State VARCHAR(255) = ltrim(rtrim('" + addState + @"'))						
		                            ,@Zip VARCHAR(255) = ltrim(rtrim('" + addZip + @"'))						
		                            ,@Company VARCHAR(255) = ltrim(rtrim('" + addCompany + @"'))					
		                            ,@PharmacyType VARCHAR(255) = ltrim(rtrim('" + AddPharmacyType.Text + @"'))			
		                            ,@ParsedStoreName VARCHAR(255) = ltrim(rtrim('" + addName + @"'))
		                            ,@StoreNumber VARCHAR(255) = case when ltrim(rtrim('" + addSNumber + @"'))='' then Null else '#'+ltrim(rtrim('" + addSNumber + @"'))  end											
		                            ,@MRN INT =case when ltrim(rtrim('" + addMRN + @"'))='' then Null else cast( ltrim(rtrim('" + addMRN + @"')) as int) end						

                            DECLARE	@CPK_SALESREP INT

                            IF(	ISNULL(@StoreName,'') != '' 
	                            AND ISNULL(@Address,'') != '' 
	                            AND ISNULL(@City,'') != '' 
	                            AND ISNULL(@Phone,'') != '' 
	                            AND ISNULL(@State,'') != '' 
	                            AND ISNULL(@Zip,'') != '' 
	                            AND ISNULL(@Company,'') != '' 
	                            AND ISNULL(@PharmacyType,'') != '' 
	                            AND ISNULL(@ParsedStoreName,'') != ''
	                            AND NOT EXISTS(SELECT Phone FROM DSICPR..PharmacyList WHERE Phone = @Phone)
	                            )

                            BEGIN

                            SET @CPK_SALESREP = (SELECT MAX(CPK_SALESREP) FROM CPRSQL..SalesRep) + 1

                            INSERT	INTO DSICPR..PharmacyList
                            SELECT	ISNULL(@Title,'')
		                            ,ISNULL(@FirstName,'')
		                            ,ISNULL(@MiddleInitial,'')
		                            ,ISNULL(@LastName,'')
		                            ,ISNULL(@Title1,'')
		                            ,@StoreName
		                            ,@Address
		                            ,@City
		                            ,@Phone
		                            ,@State
		                            ,@Zip
		                            ,@Company
		                            ,@PharmacyType
		                            ,@ParsedStoreName
		                            ,ISNULL(@StoreNumber,'')

                            INSERT INTO CPRSQL..SALESREP	(
								                            CPK_SALESREP
								                            ,CODE
								                            ,FNAME
								                            ,LNAME
								                            ,SITENO
								                            ,DELFLAG
								                            )
                            SELECT	@CPK_SALESREP
		                            ,''
		                            ,@Phone
		                            ,@StoreName
		                            ,1
		                            ,0

                            SELECT	*
                            FROM	DSICPR..PharmacyList
                            WHERE	Phone = @Phone

                            SELECT	*
                            FROM	CPRSQL..SALESREP
                            WHERE	FNAME = @Phone

                            END

                            ELSE

                            BEGIN

	                            SET @CPK_SALESREP = (SELECT CPK_SALESREP FROM CPRSQL..SalesRep WHERE FNAME = @Phone)
	                            SELECT	'Pharmacy Already Exists in database'

                            END

                            IF	(@MRN IS NOT NULL)
                            BEGIN

                            UPDATE	OT
                            SET		CFK_SALESREP = @CPK_SALESREP
                            FROM	CPRSQL..OT
                            WHERE	referral LIKE '%Transfer%'
		                            AND MRN = @MRN
		                            AND ISNULL(cfk_salesrep,'') = ''
		                            AND CAST(TOUCHDATE AS date) >= CAST(DATEADD(D, -1, GETDATE()) AS date)
		                            AND CAST(TOUCHDATE AS date) <= CAST(DATEADD(D, 1, GETDATE()) AS date)
		                            AND CAST(CREATEDON AS date) >= CAST(DATEADD(D, -15, GETDATE()) AS date)
		                            AND CAST(CREATEDON AS date) <= CAST(DATEADD(D, 15, GETDATE()) AS date)

                                                END";
                //StreamWriter sw = new StreamWriter(@"C:\Users\pei.wang\Documents\Visual Studio 2013\Projects\AddPharmacy\1.txt");
                //sw.WriteLine(addText);
                //sw.Close();
                try
                {
                    SqlCommand searchCommand = new SqlCommand(addText, addCon);
                    DataTable searchTable = new DataTable();
                    searchTable.Load(searchCommand.ExecuteReader());
                    adg.ItemsSource = searchTable.DefaultView;
                }
                catch
                {
                    System.Windows.MessageBox.Show("You do hot have select permission, please contact your SQL admins", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.DialogResult = false;
                    this.Close();
                }
                addCon.Close();
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
