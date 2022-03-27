using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataViewer.Models
{
    /// <summary>
    /// Flight Transit information. 
    /// </summary>
    public class FlightTransit
    {
        /// <summary>
        /// Line number of the data
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// Flight information
        /// </summary>
        public Flight Flight { get; set; }
        /// <summary>
        /// Flight departure information
        /// </summary>
        public FlightAirport DepartureInfo { get; set; }
        /// <summary>
        /// Flight arrical information
        /// </summary>
        public FlightAirport ArrivalInfo { get; set; }
        /// <summary>
        /// Flight Transit duration
        /// </summary>
        public TimeSpan TransitDuration { get; set; }

        /// <summary>
        /// True when the details has parsing error
        /// </summary>
        public bool HasParsingError { get; set; }

        /// <summary>
        /// Display Flight transit info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{LineNumber}\tLineNumber\r\n{Flight?.Carrier}\tCarrier\r\n{Flight?.OperatingCarrier}\tOperatingCarrier\r\n"
                + $"{Flight?.FlightNumber}\tFlightNumber\r\n{string.Join("",Flight?.Classes)}\tClasses\r\n{DepartureInfo?.AirportCode}\tDepartureAirport\r\n"
                +$"{ArrivalInfo?.AirportCode}\tArrivalAirport\r\n{DepartureInfo?.TransitTime.ToString("hhmm")}\tDepartureTime\r\n{ArrivalInfo?.TransitTime.ToString("hhmm")}\tArrivalTime\r\n"
                + $"{DisplayTimeShift(ArrivalInfo.TimeShift)}\tArrivalTimeShift\r\n{Flight?.Equipment}\tEquipment\r\n{ArrivalInfo?.OnTime}\tOnTime\r\n{TransitDuration.ToString(@"h\:mm")}\tDuration\r\n";
        }

        private string DisplayTimeShift(int timeShift)
        {
            return timeShift != 0 ? (timeShift < 0 ? $"{timeShift}" : $"+{timeShift}") : "";
        }
    }
}
