namespace MauiAirplanes;

using Airport;

public partial class MainPage : ContentPage
{
	int count = 0;
    IBusinessLogic logic = new BusinessLogic();

	public MainPage()
	{
		InitializeComponent();

        BindingContext = logic;
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		/*
		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
		*/
	}
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
    private void DeleteAirport(object sender, EventArgs e)
    {
        try {
            Airport airport = CV.SelectedItem as Airport;
            logic.DeleteAirport(airport.Id);
        } catch(AirportException ae)
            {
            DisplayAlert("Delete Failed!", ae.Message, "OK");
        } catch(Exception)
        {
            DisplayAlert("Delete Failed!", "Invalid selection", "Ok");
        }
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

