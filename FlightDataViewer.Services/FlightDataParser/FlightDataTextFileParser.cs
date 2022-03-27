using FlightDataViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FlightDataViewer.Services.FlightDataParser
{
    public class FlightDataTextFileParser : IFlightDataParser
    {
        private ILogger _logger;
        #region ctor

        public FlightDataTextFileParser(ILogger logger)
        {
            _logger = logger;
        }
        #endregion

        #region Private Methods

        private static string GetCaptureText(CaptureCollection captures)
        {
            return (captures != null && captures.Count > 0) ? captures[0].ToString().Trim() : string.Empty;
        }
        private FlightDataRegExInfo GetRegExInfo()
        {
            //Ideally this comes from database but hard coding now
            return new FlightDataRegExInfo()
            {
                Pattern = @"(\d*)(\s)*([A-Z]{0,3}:?[A-Z0-9]{2,3}\s?[0-9]{3,4})(\s*)(([A-Z][0-9]\s*){0,7})(\/?)([A-Z]{3})(\s*)([A-E]?[0-9]?)(\s*)([A-Z]{3})(\s*)(\d?\s+)([\d]{0,2}[\d]{2})(\s*)([\d]{0,2}[\d]{2})([+|-]\d)?(\s*)((E[0-9].)(\w{3}))(\s*)([0-9]\s|[A-Z])?(\s*)([\d]{0,2}:[\d]{2})",
                LineNumberGroupNumber = 1,
                CarrierGroupNumber = 3,
                ClassesGroupNumber = 5,
                DepartureAirportGroupNumber = 8,
                ArrivalAirportGroupNumber = 12,
                DepartureTimeGroupNumber = 15,
                ArrivalTimeGroupNumber = 17,
                ArrivalTimeShiftGroupNumber = 18,
                EquipmentGroupNumber = 22,
                OnTimeGroupNumber = 24,
                DurationGroupNumber = 26,
                TotalGroups = 26
            };
        }
        private static char[] ParseClasses(FlightDataRegExInfo regExInfo, Match regExMatch)
        {
            //Sample data "F7 A4 Y7 B7 M7 H7 K7 "
            string rawClassesText = GetCaptureText(regExMatch.Groups[regExInfo.ClassesGroupNumber].Captures);
            string[] classesSplit = rawClassesText.Split(' ');
            List<char> classes = new List<char>();
            foreach (string classItem in classesSplit)
            {
                if (string.IsNullOrWhiteSpace(classItem) == false)
                {
                    classes.Add(classItem.Trim()[0]);
                }
            }
            return classes.ToArray();
        }

        private static Flight ParseFlightInfo(FlightDataRegExInfo regExInfo, Match regExMatch)
        {
            //Sample data UA:US6440
            Flight flight = new Flight();

            string rawCarrierText = GetCaptureText(regExMatch.Groups[regExInfo.CarrierGroupNumber].Captures);
            if (rawCarrierText.Contains(":"))
            {
                string[] carrierSplit = rawCarrierText.Split(':');
                flight.Carrier = carrierSplit[0];
                flight.OperatingCarrier = carrierSplit[1].Substring(0, 2).Trim();
                flight.FlightNumber = carrierSplit[1].Substring(2).Trim();
            }
            else
            {
                flight.Carrier = rawCarrierText.Substring(0, 2).Trim();
                flight.FlightNumber = rawCarrierText.Substring(2).Trim();
            }
            flight.Classes = ParseClasses(regExInfo, regExMatch);

            //Sample data: 735
            flight.Equipment = GetCaptureText(regExMatch.Groups[regExInfo.EquipmentGroupNumber].Captures);
            return flight;
        }

        private static FlightAirport ParseArrivalInfo(FlightDataRegExInfo regExInfo, Match regExMatch)
        {
            FlightAirport flightAirport = new FlightAirport();
            flightAirport.AirportCode = GetCaptureText(regExMatch.Groups[regExInfo.ArrivalAirportGroupNumber].Captures);

            //sample 1445
            if (TimeSpan.TryParseExact(GetCaptureText(regExMatch.Groups[regExInfo.ArrivalTimeGroupNumber].Captures), "hhmm", CultureInfo.InvariantCulture, out TimeSpan transitTime))
            {
                flightAirport.TransitTime = transitTime;
            }

            //sample +1
            if (Int32.TryParse(GetCaptureText(regExMatch.Groups[regExInfo.ArrivalTimeShiftGroupNumber].Captures), out int timeShift))
            {
                flightAirport.TimeShift = timeShift;
            }

            //Ontime
            //ASSUMPTION: Not user about the meaning of numeric in OnTime column. Displaying as such
            string rawOnTime = GetCaptureText(regExMatch.Groups[regExInfo.OnTimeGroupNumber].Captures);
            if (string.IsNullOrWhiteSpace(rawOnTime) == false && rawOnTime.Length > 0)
            {
                flightAirport.OnTime = rawOnTime[0];
            }

            return flightAirport;
        }

       

        private static FlightAirport ParseDepartureInfo(FlightDataRegExInfo regExInfo, Match regExMatch)
        {
            FlightAirport flightAirport = new FlightAirport();
            flightAirport.AirportCode = GetCaptureText(regExMatch.Groups[regExInfo.DepartureAirportGroupNumber].Captures);
            if (TimeSpan.TryParseExact(GetCaptureText(regExMatch.Groups[regExInfo.DepartureTimeGroupNumber].Captures), "hhmm", CultureInfo.InvariantCulture, out TimeSpan transitTime))
            {
                flightAirport.TransitTime = transitTime;
            }
            return flightAirport;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textFilePath"></param>
        /// <returns></returns>
        private List<string> ReadFlightData(string textFilePath)
        {
            //It is possible to implement this batches. meaning fetch N number of lines per batch and process
            //It helps in memory consumption. Go ahead without batch implementation is essence of time
            List<string> flightData = new List<string>();

            using (FileStream fs = new FileStream(textFilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs, detectEncodingFromByteOrderMarks: true))
            {
                string lineItem = string.Empty;
                while ((lineItem = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(lineItem) == false)
                    {
                        flightData.Add(lineItem);
                    }
                }
            }

            return flightData;
        }

        #endregion

        #region Public methods

        public FlightTransit[] Parse(FlightDataSource dataSource)
        {
            List<FlightTransit> parsedData = new List<FlightTransit>();

            List<string> flightData = ReadFlightData(dataSource.FilePath);

            FlightDataRegExInfo regExInfo = GetRegExInfo();
            Regex regExEngine = new Regex(regExInfo.Pattern, RegexOptions.Singleline);

            int parsedLines = 0;

            foreach (string lineItem in flightData)
            {
                try
                {
                    Match regExMatch = regExEngine.Match(lineItem);
                    if (regExMatch.Success)
                    {
                        if (regExInfo.TotalGroups <= regExMatch.Groups.Count)
                        {
                            FlightTransit flightTransit = new FlightTransit();
                            flightTransit.LineNumber = Convert.ToInt32(GetCaptureText(regExMatch.Groups[regExInfo.LineNumberGroupNumber].Captures));

                            flightTransit.Flight = ParseFlightInfo(regExInfo, regExMatch);
                            flightTransit.DepartureInfo = ParseDepartureInfo(regExInfo, regExMatch);
                            flightTransit.ArrivalInfo = ParseArrivalInfo(regExInfo, regExMatch);

                            if (TimeSpan.TryParse(GetCaptureText(regExMatch.Groups[regExInfo.DurationGroupNumber].Captures), out TimeSpan duration))
                            {
                                flightTransit.TransitDuration = duration;
                            }
                            parsedData.Add(flightTransit);
                        }
                        parsedLines++;
                    }
                    else
                    {
                        //using a counter variable instead of linenumber available in data to include the scenario where parsing fails on line number
                        parsedLines++;
                        FlightTransit flightTransit = new FlightTransit();
                        flightTransit.LineNumber = parsedLines;
                        flightTransit.HasParsingError = true;
                        parsedData.Add(flightTransit);
                        throw new Exception($"Regular expression matching failed for line {parsedLines + 1}");
                    }
                }
                catch(Exception ex)
                {
                    _logger.Log($"Error encountered when parsing line number: {parsedLines + 1}");
                    _logger.Log(ex);
                }
            }

            return parsedData.ToArray();
        }

        #endregion
    }
}
