using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Models
{
    /// <summary>
    /// Represents Flight details
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Carrier name
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Operating carrier name
        /// </summary>
        public string OperatingCarrier { get; set; }
        /// <summary>
        /// FlightNumber. Assuming datasource is external, it is best to store in string because no math operations will be 
        /// intended with Flight number
        /// </summary>
        public string FlightNumber { get; set; }
        /// <summary>
        /// Available classes
        /// </summary>
        public char[] Classes { get; set; }
        /// <summary>
        /// Equipment. Assuming datasource is external, it is best to store Equipment number in string so it is flexible when 
        /// another carrier uses a non numeric value
        /// </summary>
        public string Equipment { get; set; }
    }
}
