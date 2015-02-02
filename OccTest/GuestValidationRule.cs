using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace OccTest
{
    public class GuestValidationRule : ValidationRule
    {
        private int sameGuestCounter;

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            Guest guest = (value as BindingGroup).Items[0] as Guest;
            // Name validation
            try
            {
                // Check if signs other than letters were passed as a name - first letter must be capital
                if (!ContainOnlyLetters(guest.name))
                {
                    return new ValidationResult(false,
                        "Wrong name format.\n - First letter must be capital.\n - Only latin letters allowed.\n - No white spaces allowed.");
                }

                if (guest.name.Length < 2)
                {
                    return new ValidationResult(false,
                        "Name must must have at least 2 signs!");
                }
            }
            catch
            {
                // guest.name.Length == 0 and exception caught
                return new ValidationResult(false,
                        "Name can not be empty!");
            }
            
            // Surname validation
            try
            {

                // Check if signs other than letters were passed as a surname - first letter must be capital
                if (!ContainOnlyLetters(guest.surname))
                {
                    return new ValidationResult(false,
                        "Wrong surname format:\n - First letter must be capital.\n - Only latin letters allowed.\n - No white spaces allowed.");
                }

                if (guest.surname.Length < 2)
                {
                    return new ValidationResult(false,
                        "Surname must must have at least 2 signs!");
                }
            }
            catch
            {
                // guest.surname.Length == 0 and exception caught
                return new ValidationResult(false,
                        "Surname can not be empty!");
            }

            // Phone number validation
            try
            {
                // Check if signs other than numbers were passed as phone number
                if (!ContainOnlyDigits(guest.phoneNumber))
                {
                    return new ValidationResult(false,
                            "Phone number must contain only numbers!");
                }
            }
            catch
            {
                return new ValidationResult(false,
                            "Phone number can not be empty!");
            }

            try
            {
                if (guest.phoneNumber.Length < 9)
                {
                    return new ValidationResult(false,
                        "Phone number must have at least 9 numbers!");
                }
            }
            catch
            {
                // guest.phoneNumber.Length == 0 and exception caught
                return new ValidationResult(false,
                        "Phone number can not be empty!");
            }

            // Duplicates validation
            sameGuestCounter = 0;
            foreach (Guest element in MainWindow.guestList.items)
            {
                if ((element.name == guest.name) && (element.surname == guest.surname) && (element.phoneNumber == guest.phoneNumber))
                {
                    sameGuestCounter++;
                }

                // If counter greater than 1 => more than the current row has same values == duplicated rows
                if (sameGuestCounter > 1)
                {
                    // Duplicate found, same name. surname and phone number
                    return new ValidationResult(false,
                            "Duplicated guest!\n There is already a guest with the same name, surname and phone number!");
                }
            }

            // Set info about guests number if new row is ok
            // TODO: use MVVM to acces window controls
            ((MainWindow)System.Windows.Application.Current.MainWindow).guestNumberTextBox.Text = MainWindow.guestList.countGuests().ToString();
            ((MainWindow)System.Windows.Application.Current.MainWindow).maleNumberTextBox.Text = MainWindow.guestList.countMales().ToString();
            ((MainWindow)System.Windows.Application.Current.MainWindow).femaleNumberTextBox.Text = MainWindow.guestList.countFemales().ToString();
            ((MainWindow)System.Windows.Application.Current.MainWindow).confirmedGuestNumberTextBox.Text = MainWindow.guestList.countConfirmed().ToString();

            return ValidationResult.ValidResult;
        }

        private bool ContainOnlyDigits(string input)
        {
            bool containsNumbers = false;
            if (Regex.IsMatch(input, @"^[0-9]*$"))
            {
                containsNumbers = true;
            }
            
            return containsNumbers;
        }

        private bool ContainOnlyLetters(string input)
        {
            bool containsNumbers = false;
            if (Regex.IsMatch(input, @"^[A-Z][a-z]*$"))
            {
                containsNumbers = true;
            }

            return containsNumbers;
        }
    }
}
