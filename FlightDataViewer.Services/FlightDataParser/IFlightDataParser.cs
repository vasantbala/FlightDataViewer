using FlightDataViewer.Models;

namespace FlightDataViewer.Services.FlightDataParser
{
    /// <summary>
    /// Interface for various implementations of parser
    /// </summary>
    public interface IFlightDataParser
    {
        FlightTransit[] Parse(FlightDataSource dataSource);
    }
}