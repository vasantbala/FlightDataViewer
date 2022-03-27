using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Models
{
    /// <summary>
    /// Contains the Regular expression configuration
    /// Idea is to store regex config in a database so it becomes easy to tweak regex without having to create a build
    /// </summary>
    public class FlightDataRegExInfo
    {
        /// <summary>
        /// Reg Ex pattern
        /// </summary>
        public string Pattern { get; set; }
        /// <summary>
        /// Reg Ex Group number to match Line Number
        /// </summary>
        public int LineNumberGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match Carrier
        /// </summary>
        public int CarrierGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match Classes
        /// </summary>
        public int ClassesGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match DepartureAirport
        /// </summary>
        public int DepartureAirportGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match ArrivalAirport
        /// </summary>
        public int ArrivalAirportGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match DepartureTime
        /// </summary>
        public int DepartureTimeGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match ArrivalTime
        /// </summary>
        public int ArrivalTimeGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match ArrivalTimeShift
        /// </summary>
        public int ArrivalTimeShiftGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match Equipment
        /// </summary>
        public int EquipmentGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match OnTime
        /// </summary>
        public int OnTimeGroupNumber { get; set; }
        /// <summary>
        /// Reg Ex Group number to match Duration
        /// </summary>
        public int DurationGroupNumber { get; set; }
        /// <summary>
        /// Total Groups in reg ex pattern
        /// </summary>
        public int TotalGroups { get; set; }
    }
}
