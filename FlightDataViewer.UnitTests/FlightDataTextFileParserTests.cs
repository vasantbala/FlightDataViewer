using FlightDataViewer.Models;
using FlightDataViewer.Services;
using FlightDataViewer.Services.FlightDataParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Unity;
using FluentAssertions;

namespace FlightDataViewer.UnitTests
{
    [TestClass]
    public class FlightDataTextFileParserTests
    {
        IFlightDataParser _parser = null;

        public FlightDataTextFileParserTests()
        {
            //Dependency injection using unity 
            IUnityContainer unityContainer = DependencyConfigurationManager.Instance().Container;

            //Factory implementation with an assumption that the data source would change in the future
            IFlightDataParserFactory flightDataParserFactory = unityContainer.Resolve<IFlightDataParserFactory>();

            //Get Text file parser
            _parser = flightDataParserFactory.Create(FlightDataParserType.Text);
            
        }

        [TestMethod]
        public void AllLinesParsedSuccessfully()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            flightTransits.Length.Should().Be(18);
            flightTransits.All(data => data.HasParsingError == false).Should().BeTrue();
        }

        [TestMethod]
        public void DifferenciateOperatingCarrierAndCarrier()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            FlightTransit flightTransitsLine3 = flightTransits.First(q => q.LineNumber == 3);
            flightTransitsLine3.Flight.OperatingCarrier.Should().Be("US");
            flightTransitsLine3.Flight.Carrier.Should().Be("UA");
        }

        [TestMethod]
        public void DisplayTimeShift()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            FlightTransit flightTransitsLine18 = flightTransits.First(q => q.LineNumber == 18);
            flightTransitsLine18.ToString().Contains("+1\tArrivalTimeShift").Should().BeTrue();
        }

        [TestMethod]
        public void ParseAllClasses()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            FlightTransit flightTransitsLine18 = flightTransits.First(q => q.LineNumber == 18);
            flightTransitsLine18.Flight.Classes.Should().Equal("WBQL");
        }


        [TestMethod]
        public void DepatureAirportTest()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            flightTransits.All(data => data.DepartureInfo.AirportCode == "DFW").Should().BeTrue();
        }

        [TestMethod]
        public void ArrivalAirportTest()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            flightTransits.All(data => data.ArrivalInfo.AirportCode == "LAX").Should().BeTrue();
        }

        [TestMethod]
        public void NoOperatingCarrierTest()
        {
            //Parse flight data
            FlightTransit[] flightTransits = _parser.Parse(new FlightDataSource() { FilePath = @".\Data\flightdata.txt" });
            FlightTransit flightTransitsLine18 = flightTransits.First(q => q.LineNumber == 18);
            flightTransitsLine18.Flight.Carrier.Equals("HP").Should().BeTrue();
            (flightTransitsLine18.Flight.OperatingCarrier == null).Should().BeTrue();
        }
    }
}
