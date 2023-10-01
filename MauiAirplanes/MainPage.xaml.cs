namespace MauiAirplanes;

using Airport;
using Npgsql;

public partial class MainPage : ContentPage
{
	int count = 0;
    IBusinessLogic logic = new BusinessLogic();

	public MainPage()
	{
		InitializeComponent();

        BindingContext = logic;
    }
    /// <summary>
    /// Adds airport to the database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	public void AddAirport(object sender, EventArgs e)
    {
        try
		{
            string id = AirportVerification.VerifyID(AirportIDENT.Text);
            string city = AirportVerification.VerifyCity(AirportCityENT.Text);
            DateTime date = AirportVerification.VerifyDate(AirportDateENT.Text);
            int rating = AirportVerification.VerifyRating(AirportRatingENT.Text);
            logic.AddAirport(id, city, date, rating);
            ClearEntrys();
        } catch(AirportException ae)
        {
            DisplayAlert("Add Failed!", ae.Message, "OK");
        }
    }
    /// <summary>
    /// Edit pre-existing airport
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	private void EditAirport(object sender, EventArgs e)
    {
        try
        {
            string id = AirportVerification.VerifyID(AirportIDENT.Text);
            string city = AirportVerification.VerifyCity(AirportCityENT.Text);
            DateTime date = AirportVerification.VerifyDate(AirportDateENT.Text);
            int rating = AirportVerification.VerifyRating(AirportRatingENT.Text);
            logic.EditAirport(id, city, date, rating);
            ClearEntrys();
        } catch(AirportException ae)
        {
            DisplayAlert("Edit Failed!", ae.Message, "OK");
        }
    }
    /// <summary>
    /// Delete existing airport
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DeleteAirport(object sender, EventArgs e)
    {
        try {
            Airport airport = CV.SelectedItem as Airport;
            bool delete = await DisplayAlert("Delete Airport?", "Are you sure you want to remove this airport?\nAction cannot be undone!", "Delete", "Cancel");
            if(delete)
            {
                logic.DeleteAirport(airport.Id);
            }
        } catch(AirportException ae)
            {
            DisplayAlert("Delete Failed!", ae.Message, "OK");
        } catch(Exception)
        {
            DisplayAlert("Delete Failed!", "Invalid selection", "Ok");
        }
    }
    /// <summary>
    /// Display a popup message with statistics.
    /// Note: Lab3 never gave clear guide on how to display the statistics so I just went generic DisplayAlert
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CalculateStats(object sender, EventArgs e)
    {
        DisplayAlert("Statistics", logic.CalculateStatistics(), "OK");
    }
    /// <summary>
    /// Clears all input entrys
    /// </summary>
    private void ClearEntrys()
    {
        AirportIDENT.Text = "";
        AirportCityENT.Text = "";
        AirportDateENT.Text = "";
        AirportRatingENT.Text = "";
    }
}

