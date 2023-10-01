using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport;
public class Airport : INotifyPropertyChanged {
    private string id;
    private string city;
    private DateTime dateVisited;
    private int rating;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Id {
        get { return id; }
        set { 
            id = value;
            SetDirty(nameof(Id));
        }
    }
    public string City { 
        get { return city; } 
        set { 
            city = value;
            SetDirty(nameof(City));
        }
    }
    public DateTime DateVisited { 
        get { return dateVisited; } 
        set { 
            dateVisited = value;
            SetDirty(nameof(DateVisited));
            SetDirty(nameof(DateVisitedDisplay));
        }
    }
    public string DateVisitedDisplay
    {
        get { return dateVisited.ToString("M/d/yyyy"); }
    }
    public int Rating { 
        get { return rating; } 
        set { 
            rating = value;
            SetDirty(nameof(Rating));
        }
    }


    public Airport() : this("KATW", "Appleton", DateTime.Now, 5) { }
    public Airport(string id, string city, DateTime dateVisited, int rating)
    {
        this.id = id;
        this.city = city;
        this.dateVisited = dateVisited;
        this.rating = rating;
    }

    /// <summary>
    /// Property changed. Notify xaml
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void SetDirty(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    /// <summary>
    /// Set all my data to the proveded airport
    /// </summary>
    /// <param name="airport"></param>
    public void CopyData(Airport airport)
    {
        this.Rating = airport.Rating;
        this.City = airport.City;
        this.DateVisited = airport.DateVisited;
    }
    // ======== Operator Overloading =========
    // 2 Aiprorts are equal iff their id's are equal
    public static bool operator ==(Airport? a, Airport? b)
    {
        if(ReferenceEquals(a, b)) return true;
        if(ReferenceEquals(a, null)) return false;
        if(ReferenceEquals(b, null)) return false;
        return a.id.Equals(b.id);
    }
    public static bool operator !=(Airport? a, Airport? b) { return !(a == b); }

    public override bool Equals(object? obj)
    {
        if(ReferenceEquals(this, obj)) return true;
        if(ReferenceEquals(obj, null)) return false;
        if(obj.GetType() != typeof(Airport)) return false;
        return (Airport)obj == this;
    }
    public override string ToString()
    {
        return string.Format("{0} - {1}, {2}, {3}", id, city, dateVisited.ToString("M/d/yyyy"), rating);
    }

}
/// <summary>
/// Do we need an airport exception? Probobly not.
/// I use it so that I can be particular about what exception I'm catching:
/// If any exception is thrown that I did not intend (aka not an airport excpetion) I want the program to crash in general as thats unintentional.
/// </summary>
[Serializable]
public class AirportException : Exception {
    private string message;
    public string Message { get { return message; } }
    public AirportException(string message) : base(message) { this.message = message; }
    public AirportException() : base("") { this.message = ""; }
}

