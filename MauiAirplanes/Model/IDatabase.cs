using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Airport;
public interface IDatabase
{
    ObservableCollection<Airport> Airports { get; set; } // Stores a list of all airports.


    /// <summary>
    /// Get all airports 
    /// </summary>
    /// <returns></returns>
    public abstract ObservableCollection<Airport> SelectAllAirports();
    /// <summary>
    /// Returns airport with given id, null if not found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public abstract Airport SelectAirport(string id);
    /// <summary>
    /// Adds airport to list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns></returns>
    public abstract bool InsertAirport(Airport airport);
    /// <summary>
    /// Removes airport from list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public abstract bool DeleteAirport(string id);
    /// <summary>
    /// Updates airport in list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns></returns>
    public abstract bool UpdateAirport(Airport airport);


}
