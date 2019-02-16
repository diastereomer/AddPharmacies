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

namespace AddPharmacy
{
    /// <summary>
    /// Interaction logic for Set_Value.xaml
    /// </summary>
    public partial class Set_Value : Window
    {
        public Set_Value(string columnName, string originalValue)
        {
            columnName1 = columnName;
            originalValue1 = originalValue;
           // MessageBox.Show(columnName + originalValue);
            InitializeComponent();
            setTextBox.Text = originalValue;
            setLabel.Content = "Set " + columnName;
        }
        private string columnName1;
        private string originalValue1;

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            setTextBox.Text = originalValue1;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            setTextBox.Text = "";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            MainWindow.setValue = setTextBox.Text;
            this.Close();
        }
    }
}
