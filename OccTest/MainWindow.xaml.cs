using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace OccTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static GuestList guestList;
        String outputFile;
        String inputFile;
        String defaultGenderRatio = "0,2";

        public enum GenderType { Male, Female }

        public MainWindow()
        {
            InitializeComponent();
            // Create guestList instance
            guestList = GuestList.Instance;

            // Set DataGrid ItemsSource
            guestListDataGrid.ItemsSource = guestList.items;
            guestListDataGrid.DataContext = guestList.items;

            // Set initial gender ratio
            genderRatioTextBox.Text = defaultGenderRatio;
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Guest List.\nAuthor: Maciej Plewko");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Prepare exit message box
            MessageBoxResult key = MessageBox.Show(
            "Do you really want to quit",
            "",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);
            if (key == MessageBoxResult.No)
            {
                // Cancel closing
                e.Cancel = true;
            }
            else
            {
                // Close the application
                Application.Current.Shutdown();
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
                // Closing Event
                Application.Current.Shutdown();
        }

        private void newGuestList_Click(object sender, RoutedEventArgs e)
        {
            // Clear guestList collection
            guestList.items.Clear();
            this.ClearScreen();
        }

        private void saveGuestList_Click(object sender, RoutedEventArgs e)
        {
            if (guestList.items.Count > 0)
            {
                // Create and configure SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.FileName = "guestList"; // Default file name
                saveFileDialog.Filter = "Xml Files (*.xml)|*.xml";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                // Display save file dialog box
                Nullable<bool> result = saveFileDialog.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    // Set the outputFile 
                    outputFile = saveFileDialog.FileName;
                    // Save file as an XML
                    XMLSaver.SaveData(guestList, outputFile);
                }
            }
        }

        private void loadGuestList_Click(object sender, RoutedEventArgs e)
        {
            // Create and configure OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Set filter for file extension and default file extension
            openFileDialog.Filter = "Xml Files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Check if the file was selected
            if (result == true)
            {
                // Get the selected file name
                inputFile = openFileDialog.FileName;
                // Create XML loader instance
                XMLLoader<GuestList> xmlLoader = new XMLLoader<GuestList>();
                try
                {
                    // Load data from selected file into the collection
                    guestList = xmlLoader.LoadData(inputFile);
                    guestListDataGrid.ItemsSource = guestList.items;
                    this.SetGuestNumbers(guestList.countGuests(), guestList.countMales(), guestList.countFemales(), guestList.countConfirmed());
                }
                catch (Exception exception)
                {
                    // TODO: check XML structure properly?
                    // Display a message box if an exepction thrown
                    MessageBox.Show("Can not load this file. Wrong XML structure!\n" + exception.Message);
                }
            }
        }

        public void SetGuestNumbers(int allGuestNumber, int maleNumber, int femaleNumber, int confirmedGuests)
        {
            guestNumberTextBox.Text = allGuestNumber.ToString();
            maleNumberTextBox.Text = maleNumber.ToString();
            femaleNumberTextBox.Text = femaleNumber.ToString();
            confirmedGuestNumberTextBox.Text = confirmedGuests.ToString();
        }

        private void checkGenderRatio_Click(object sender, RoutedEventArgs e)
        {
            double males = guestList.countMales();
            double females = guestList.countFemales();
            double allGuests = guestList.countGuests();
            double genderDesiredRatio; //= double.Parse(genderRatioTextBox.Text);

            try
            {
                // Converse string to double, replace ',' with '.'
                string temporary = genderRatioTextBox.Text.Replace(".", ",");
                genderDesiredRatio = double.Parse(temporary);
            }
            catch
            {
                MessageBox.Show("Wrong Desired Gender Ratio format.\n Can not be greater than 1.0 or less than 0.0.\nDo not enter letters or special characters.");
                return;
            }
            
            if ((genderDesiredRatio < 0.0) | (genderDesiredRatio > 1.0))
            {   // TODO: should be done in validation
                MessageBox.Show("Wrong Desired Gender Ratio format.\n Can not be greater than 1.0 or less than 0.0");
            }
            else
            {
                if (allGuests >= 2)
                {
                    // Calculate gender disproportion, round the output
                    double disproportion = Math.Round(Math.Abs((males - females) / allGuests), 2);

                    // Action if disproportion greater than desired
                    if (disproportion > genderDesiredRatio)
                    {   
                        if (males > females)
                        {
                            MessageBox.Show("Gender disproportion exceeded!\nInvite more women to reach your desired ratio.\n" +
                                "GenderRatio: " + disproportion.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Gender disproportion exceeded!\nInvite more men to reach your desired ratio.\n" +
                                "GenderRatio: " + disproportion.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Everything is ok!\nGender ratio matches your expectations.\n" +
                        "GenderRatio: " + disproportion.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("You can not check gender ratio until number of guests is less than 2!");
                }
            }
        }

        private void ClearScreen()
        {
            guestNumberTextBox.Text = "0";
            maleNumberTextBox.Text = "0";
            femaleNumberTextBox.Text = "0";
            confirmedGuestNumberTextBox.Text = "0";
        }
    }
}