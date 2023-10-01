using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airport;

public class Database : IDatabase
{
    private ObservableCollection<Airport> airports = new ObservableCollection<Airport>();
    public ObservableCollection<Airport> Airports
    {
        get { return airports; }
        set { airports = value; }
    }
    protected string filename = FileSystem.Current.AppDataDirectory + "/airport.db"; // TODO: filename shouldn't be hardcoded. At least needs a way to modify.
    public Database() // load db immidiatly on creating
    {
        Load();
    }

    /// <summary>
    /// Get all airports, loads from file into db
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Airport> SelectAllAirports()
    {
        Load(); // load db
        return Airports; // return the db struct
    }
    /// <summary>
    /// Returns airport with given id, null if not found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Airport SelectAirport(string id)
    {
        if (Airports == null) return null;
        foreach (Airport airport in Airports) // iterate over list
            if (airport.Id == id) return airport; // if found, return the selected airport
        return null; // null if not found
    }
    /// <summary>
    /// Adds airport to list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns></returns>
    public bool InsertAirport(Airport airport)
    {
        if (Airports == null || SelectAirport(airport.Id) != null) return false; // return if airport already exists
        Airports.Add(airport);
        Save();
        return true;
    }
    /// <summary>
    /// Removes airport from list
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if successfully removed</returns>
    public bool DeleteAirport(string id)
    {
        Airport airport = SelectAirport(id);   // find the airport in the list
        if (airport == null || Airports == null) return false; // return if the airport doesnt exist
        Airports.Remove(airport); // remove the airport
        Save();
        return true;
    }
    /// <summary>
    /// Updates airport in list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns>true if successfully modified</returns>
    public bool UpdateAirport(Airport airport)
    {
        if (Airports == null) return false; // if airport db is null return
        int index = Airports.IndexOf(airport); // get index of airport to edit
        if (index < 0) return false;
        Airports[index].CopyData(airport);

        Save();
        return true;
    }



    // ========== MY HELPER FUNCTIONS ================


    /// <summary>
    /// Save data to the file
    /// </summary>
    private void Save()
    {
        // options object : indented makes file look nicer
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        // write to file
        File.WriteAllText(filename, JsonSerializer.Serialize(Airports, options));
    }
    /// <summary>
    /// Load data from the file.
    /// </summary>
    private void Load()
    {
        if (File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            ObservableCollection<Airport> toload = JsonSerializer.Deserialize<ObservableCollection<Airport>>(json);
            if (toload != null) Airports = toload;
        }
    }
}
