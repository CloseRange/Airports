using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport;
public interface IBusinessLogic
{

    public ObservableCollection<Airport> Airports { get; }

    /// <summary>
    /// Adds an airport with the specific fields to the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <param name="dateVisited"></param>
    /// <param name="rating"></param>
    public abstract void AddAirport(string id, string city, DateTime dateVisited, int rating);

    /// <summary>
    /// Delete the specified airport from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public abstract bool DeleteAirport(string id);
    /// <summary>
    /// Edits the spefified airport with the new values. Does not modify the id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <param name="dateVisited"></param>
    /// <param name="rating"></param>
    /// <returns></returns>
    public abstract bool EditAirport(string id, string city, DateTime dateVisited, int rating);
    /// <summary>
    /// Finds the airport object, null if not found.
    /// </summary>
    /// <param name="id"></param>
    public abstract Airport FindAirport(string id);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract string CalculateStatistics();
    /// <summary>
    /// Returns a list of all airports
    /// </summary>
    /// <returns></returns>
    public abstract ObservableCollection<Airport> GetAirports();
}
