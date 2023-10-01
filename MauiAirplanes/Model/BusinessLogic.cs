using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//jwlJ2cSIuqax2x1LCBYnog
namespace Airport;
public class BusinessLogic : IBusinessLogic {
    private IDatabase database = new Database();

    public ObservableCollection<Airport> Airports
    {
        get
        {
            return database.Airports;
        }
    }

    /// <summary>
    /// Adds an airport with the specific fields to the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <param name="dateVisited"></param>
    /// <param name="rating"></param>
    public void AddAirport(string id, string city, DateTime dateVisited, int rating)
    {
        Airport airport = new Airport(id, city, dateVisited, rating);
        database.InsertAirport(airport);
    }

    /// <summary>
    /// Delete the specified airport from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteAirport(string id)
    {
        return database.DeleteAirport(id);
    }
    /// <summary>
    /// Edits the spefified airport with the new values. Does not modify the id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <param name="dateVisited"></param>
    /// <param name="rating"></param>
    /// <returns></returns>
    public bool EditAirport(string id, string city, DateTime dateVisited, int rating)
    {
        Airport airport = new Airport(id, city, dateVisited, rating);
        return database.UpdateAirport(airport);
    }
    /// <summary>
    /// Finds the airport object, null if not found.
    /// </summary>
    /// <param name="id"></param>
    public Airport FindAirport(string id)
    {
        return database.SelectAirport(id);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string CalculateStatistics()
    {
        return "";
    }
    /// <summary>
    /// Returns a list of all airports
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Airport> GetAirports()
    {
        return database.SelectAllAirports();
    }
}
