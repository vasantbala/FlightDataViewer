using FlightDataViewer.Models;
using FlightDataViewer.Services;
using FlightDataViewer.Services.FlightDataParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace FlightDataViewer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseAndDisplayFlightData();

            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadLine();
        }

        private static void ParseAndDisplayFlightData()
        {
            try
            {
                //Dependency injection using unity 
                IUnityContainer unityContainer = DependencyConfigurationManager.Instance().Container;

                //Factory implementation with an assumption that the data source would change in the future
                IFlightDataParserFactory flightDataParserFactory = unityContainer.Resolve<IFlightDataParserFactory>();

                //Get Text file parser
                IFlightDataParser flightDataParser = flightDataParserFactory.Create(FlightDataParserType.Text);

                //Parse flight data
                FlightTransit[] flightTransits = flightDataParser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });

                //Display parsed data
                foreach (FlightTransit item in flightTransits)
                {
                    System.Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error occurred. Check log for details.");
            }
        }
    }
}
