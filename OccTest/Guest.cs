using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OccTest
{
    /// <summary>
    /// Class representing a single guest
    /// </summary>
    public class Guest : IDataErrorInfo
    {
        [XmlElement("name")]
        public String name { get; set; }
        
        [XmlElement("surname")]
        public String surname { get; set; }

        [XmlElement("gender")]
        public GenderType gender { get; set; }

        [XmlElement("phoneNumber")]
        public String phoneNumber { get; set; }

        [XmlElement("reply")]
        public Boolean reply { get; set; }

        [XmlElement("comment")]
        public String comment { get; set; }

        [XmlIgnore]
        public string Error
        {
            get 
            {
                return String.Concat(this[name], " ", this[surname], " ",
                                 this[phoneNumber]);
            }
        }

        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrWhiteSpace(name))
                        {
                            errorMessage = "Name is required.";
                        }
                        break;
                }
                return errorMessage;
            }
        }
    }
}




