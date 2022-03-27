using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Models
{
    /// <summary>
    /// Represents Destination and Arrival airport information
    /// </summary>
    public class FlightAirport
    {
        /// <summary>
        /// Destination or Arrival airport code
        /// </summary>
        public string AirportCode { get; set; }
        /// <summary>
        /// Destination or Arrival time 
        /// </summary>
        public TimeSpan TransitTime { get; set; }
        /// <summary>
        /// Arrival time shift
        /// </summary>
        public int TimeShift { get; set; }
        /// <summary>
        /// On time char
        /// </summary>
        public char OnTime { get; set; }
    }
}
