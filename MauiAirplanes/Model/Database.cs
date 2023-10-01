using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airport;

public class Database : IDatabase {
    // TODO: These variables should absoultly not be hardcoded into the code directly this way.
    // Big boy security risk. 
    private NpgsqlConnectionStringBuilder sqlConnectString = new NpgsqlConnectionStringBuilder {
        SslMode = SslMode.VerifyFull,
        Host = "minty-hisser-13095.5xj.cockroachlabs.cloud",
        Port = 26257,
        Username = "michael",
        Password = "jwlJ2cSIuqax2x1LCBYnog",
        Database = "defaultdb"
    };

    private NpgsqlConnection connection = null; // Conection object. Only initialize once!

    private ObservableCollection<Airport> airports = new ObservableCollection<Airport>();
    public ObservableCollection<Airport> Airports
    {
        get { return airports; }
        set { airports = value; }
    }
    public Database() // load db immidiatly on creating
    {
        connection = new NpgsqlConnection(sqlConnectString.ConnectionString);
        connection.Open();

        using(NpgsqlCommand cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS airports (id VARCHAR(4) PRIMARY KEY, city VARCHAR(25), visitDate DATE, rating SMALLINT)", connection))
        {
            cmd.ExecuteNonQuery();
        }

        // Inital selection. Loads DB into cache immidiatly 
        // Make command to select every airport from db
        using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM airports", connection))
        {
            // iterate over every airport, adding to list
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                    Airports.Add(new Airport((string)reader["id"], (string)reader["city"], reader.GetDateTime(2), (short)reader["rating"]));
            }

        }
    }

    /// <summary>
    /// Get all airports
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Airport> SelectAllAirports()
    {
        return Airports; // return the db struct
    }
    /// <summary>
    /// Returns airport with given id, null if not found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Airport SelectAirport(string id)
    {
        VerifyConnection();
        Airport airport = null;
        // create command to find airport with the provided id
        using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT id, city, visitDate, rating FROM airports WHERE id='" + id + "'"))
        {
            // iterate over every airport found (there should only be 1) and return it
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                    airport = new Airport((string)reader["id"], (string)reader["city"], (DateTime)reader["visitDate"], (int)reader["rating"]);
            }
        }
        return airport;
    }
    /// <summary>
    /// Adds airport to list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns></returns>
    public bool InsertAirport(Airport airport)
    {
        VerifyConnection();
        // throws error if already exists
        try
        {
            // Command to add airport into database
            using(NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO airports(id, city, visitDate, rating) VALUES(@id, @city, @visitDate, @rating)", connection))
            {
                // Populate the paramaters of the command with the airport variables
                cmd.Parameters.AddWithValue("id", airport.Id);
                cmd.Parameters.AddWithValue("city", airport.City);
                cmd.Parameters.AddWithValue("visitDate", airport.DateVisited.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("rating", (short)airport.Rating);
                cmd.ExecuteNonQuery();
            }
            Airports.Add(airport); // Add the airport directly into our local cached list. Prevents having to do a second lookup
            return true;
        } catch {
            throw new AirportException("Duplicate airport!");
        }
    }
    /// <summary>
    /// Removes airport from list
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if successfully removed</returns>
    public bool DeleteAirport(string id)
    {
        VerifyConnection();
        try
        {
            // create command to find delete with the provided id
            using(NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM airports where id='" + id + "'", connection))
            {
                cmd.ExecuteNonQuery();
            }
            // iterate over every airpirt. remove the selected airport from cache
            foreach(Airport airport in Airports)
            {
                if(airport.Id == id)
                {
                    Airports.Remove(airport);
                    return true;
                }
            }
            return false;
        } catch
        {
            throw new AirportException("No id found!");
        }
    }
    /// <summary>
    /// Updates airport in list
    /// </summary>
    /// <param name="airport"></param>
    /// <returns>true if successfully modified</returns>
    public bool UpdateAirport(Airport airport)
    {
        VerifyConnection();

        try
        {
            // Command to update airport using provided data
            using(NpgsqlCommand cmd = new NpgsqlCommand("UPDATE airports SET city=@city, visitDate=@visitDate, rating=@rating WHERE id=@id", connection))
            {
                // Populate the paramaters of the command with the airport variables
                cmd.Parameters.AddWithValue("id", airport.Id);
                cmd.Parameters.AddWithValue("city", airport.City);
                cmd.Parameters.AddWithValue("visitDate", airport.DateVisited.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("rating", (short)airport.Rating);
                cmd.ExecuteNonQuery();
            }
            // Update local cache
            int index = Airports.IndexOf(airport); // get index of airport to edit
            Airports[index].CopyData(airport); // no need to verify index < 0. Will throw error, resulting in popup message.
            return true;
        } catch
        {
            throw new AirportException("No airport exists!");
        }
    }



    /// <summary>
    /// Ensures that the database is connected! Throws error if not
    /// </summary>
    private void VerifyConnection()
    {
        if(connection == null) throw new AirportException("Error! SQL was unable to verify connection!");
    }
}
