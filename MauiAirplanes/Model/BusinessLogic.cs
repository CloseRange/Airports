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
    /// Returns a detailed message about how many airports were visited and how far until your next level!
    /// </summary>
    /// <returns></returns>
    public string CalculateStatistics()
    {
        // return variable
        string stats = "";

        // TODO: Put these as class variables or put them in their own class dealing with rankings
        string[] rankNames = { "Bronze", "Silver", "Gold" };
        int[] rankNumbers = { 42, 84, 125 };
        int visited = database.SelectAllAirports().Count(); // number of airports visited


        // what id is the next rank 
        int nextRankID = 0;
        // increment next rank if visited is more than the required for that rank
        for(int i = 0; i < rankNames.Length; i++)
        {
            if(visited >= rankNumbers[i]) nextRankID++;
            else break;
        }
        // get how many left until next rank
        int untilNextRank = rankNumbers[nextRankID] - visited;

        // print # visited
        stats += String.Format("{0} airport{1} visited\n", visited, visited == 1 ? "" : "s");
        // print current rank
        if(nextRankID > 0)
            stats += String.Format("You are {0} rank!\n", rankNames[nextRankID - 1]);
        // print remaining till next rank
        if(nextRankID != rankNames.Length)
            stats += String.Format("{0} airport{2} until {1} rank!\n", untilNextRank, rankNames[nextRankID], untilNextRank == 1 ? "" : "s");
        return stats;
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
