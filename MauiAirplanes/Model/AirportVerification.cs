using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Airport;
/// <summary>
/// This static class is souly used for verifying paramaters for airports
/// </summary>
public static class AirportVerification
{



    /// <summary>
    /// Verify airport id, throwing error if invalid
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    /// <exception cref="AirportException">Input came back null</exception>
    public static string VerifyID(string id)
    {
        if(id == null || id.Length < 3 || id.Length > 4) throw new AirportException("Invalid Airport ID"); // throw if invalid input
        return id.ToUpper();
    }
    /// <summary>
    /// Verify airport city, throwing error if invalid
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    /// <exception cref="AirportException">Inputed value is not a valid number</exception>
    public static string VerifyCity(string city)
    {
        if(city == "") throw new AirportException("Invalid Aiport City: Can't be blank");
        if(city.Length > 25) throw new AirportException("Invalid Airport City: Name too long (max 25)");
        if (city == null) throw new AirportException("Invalid Airport City: Value was unexpected");
        // TODO: Verify actual city
        return city;
    }
    /// <summary>
    /// Verify date, throwing error if invalid
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    /// <exception cref="AirportException">input was not a valid DateTime (mm/dd/yyyy)</exception>
    public static DateTime VerifyDate(string date)
    {
        DateTime ddt; // delta date time
        if(!DateTime.TryParse(date, out ddt))
            throw new AirportException("Invalid Date: Bad format");
        return ddt;
    }
    /// <summary>
    /// Verify a rating, throwing if its not valid
    /// </summary>
    /// <param name="rating"></param>
    /// <returns></returns>
    public static int VerifyRating(string rating)
    {
        int drating; // delta rating
        if(!int.TryParse(rating, out drating))
            throw new AirportException("Invalid Rating: Must be 1-5");
        return drating;
    }

}
