using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OccTest
{
    /// <summary>
    /// A singleton class containing a list of guests
    /// </summary>
    [XmlRoot("guest_list")]
    public class GuestList
    {
        private static GuestList instance;

        [XmlElement("guest")]
        public ObservableCollection<Guest> items { get; set; }
        
        private GuestList() { items = new ObservableCollection<Guest>(); }

        public static GuestList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuestList();
                }
                return instance;
            }
        }

        public int countGuests()
        {
            return this.items.Count();
        }

        public int countMales()
        {
            int quantity = 0;
            foreach (Guest element in items)
            {
                if ( element.gender == GenderType.Male)
                {
                    quantity++;
                }
            }
            return quantity;
        }

        public int countFemales()
        {
            int quantity = 0;
            foreach (Guest element in items)
            {
                if (element.gender == GenderType.Female)
                {
                    quantity++;
                }
            }
            return quantity;
        }

        public int countConfirmed()
        {
            int quantity = 0;
            foreach (Guest element in items)
            {
                if (element.reply)
                {
                    quantity++;
                }
            }
            return quantity;
        }
    }
}

