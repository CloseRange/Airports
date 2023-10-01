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

public class Database : IDatabase
{
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
        using(NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO airports(id, city, visitDate, rating) VALUES(@id, @city, @visitDate, @rating)", connection)) {
            cmd.Parameters.AddWithValue("id", "ABCD");
            cmd.Parameters.AddWithValue("city", "Oshkosh");
            cmd.Parameters.AddWithValue("visitDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("rating", (short)3);
            try
            {
                cmd.ExecuteNonQuery();
            } catch(Exception ex)
            {
                Console.WriteLine("DUPLICATE");
            }
        }

        using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT id, city FROM airports", connection)) {
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read()) Console.Write("\tairport {0}: {1}\n", reader.GetValue(0), reader.GetValue(1));
            }
        }
        
        SelectAllAirports();
    }

    /// <summary>
    /// Get all airports, loads from file into db
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Airport> SelectAllAirports()
    {
        VerifyConnection();
        try
        {
            // clear old airports
            airports.Clear();
            // Make command to select every airport from db
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM airports", connection);
            // iterate over every airport, adding to list
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    Console.WriteLine("{0} -> {1}: {2} | {3}", (string)reader["id"], (string)reader["city"], reader.GetDateTime(2), (short)reader["rating"]);
                    Airports.Add(new Airport((string)reader["id"], (string)reader["city"], reader.GetDateTime(2), (short)reader["rating"]));
                }
            }

            return Airports; // return the db struct
        } catch
        {
            throw new AirportException("DataBase Error!\nCould not select all airports.");
        }
    }
    /// <summary>
    /// Returns airport with given id, null if not found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Airport SelectAirport(string id)
    {
        VerifyConnection();
        // create command to find airport with the provided id
        NpgsqlCommand cmd = new NpgsqlCommand("SELECT id, city, visitDate, rating FROM airports WHERE id='" + id + "'");

        // iterate over every airport found (there should only be 1) and return it
        var reader = cmd.ExecuteReader();
        Airport airport = null;
        while(reader.Read())
            airport = new Airport((string)reader["id"], (string)reader["city"], (DateTime)reader["visitDate"], (int)reader["rating"]);
        return airport;
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

        return true;
    }



    /// <summary>
    /// Ensures that the database is connected! Throws error if not
    /// </summary>
    private void VerifyConnection()
    {
        if(connection == null) throw new AirportException("Error! SQL was unable to verify connection!");
    }
}
