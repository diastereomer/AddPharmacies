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
    /// Interaction logic for TransferRX.xaml
    /// </summary>
    public partial class TransferRX : Window
    {
        public TransferRX(string phone, string mrn)
        {
            InitializeComponent();
            transferPhone.Text = phone;
            transferMrn.Text = mrn;
        }

        private void transerOk_Click(object sender, RoutedEventArgs e)
        {
            if (transferPhone.Text == "" || transferPhone.Text == null || transferMrn.Text == "" || transferMrn.Text == null)
            {
                MessageBox.Show("Please fill both textboxes!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlConnection transferCon = new SqlConnection();
                transferCon.ConnectionString = @"Data Source=DSI-CPR-01;Initial Catalog= CPRSQL; Integrated Security= true";
                // try the connection manager
                try
                {
                    if (transferCon != null)
                    {
                        transferCon.Open();
                        //  MessageBox.Show("good");
                    }
                }
                catch
                {
                }
                string transferText = @"  Declare @Phone VARCHAR(255) = ltrim(rtrim('" + transferPhone.Text + @"'))						
		                            ,@MRN INT =case when ltrim(rtrim('" + transferMrn.Text + @"'))='' then Null else cast( ltrim(rtrim('" + transferMrn.Text + @"')) as int) end						
                                         DECLARE	@CPK_SALESREP INT =0;
                                         IF exists (SELECT CPK_SALESREP FROM CPRSQL..SalesRep WHERE FNAME = @Phone)
                                           begin
                                                set @CPK_SALESREP=(SELECT CPK_SALESREP FROM CPRSQL..SalesRep WHERE FNAME = @Phone)
                                           End
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
                try
                {
                    SqlCommand updateCommand = new SqlCommand(transferText, transferCon);
                    updateCommand.ExecuteNonQuery();
                }
                catch
                {
                    transferCon.Close();
                }
                this.Close();
            }
        }

        private void transferCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
