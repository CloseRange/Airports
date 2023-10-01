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
        /*
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.SslMode = SslMode.VerifyFull;
        string? databaseUrlEnv = Environment.GetEnvironmentVariable("DATABASE_URL");
        if(databaseUrlEnv == null)
        {
            connStringBuilder.Host = "minty-hisser-13095.5xj.cockroachlabs.cloud";
            connStringBuilder.Port = 26257;
            connStringBuilder.Username = "michael";
            connStringBuilder.Password = "jwlJ2cSIuqax2x1LCBYnog";
        } else
        { // postgresql://michael:<ENTER-SQL-USER-PASSWORD>@minty-hisser-13095.5xj.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full
            Uri databaseUrl = new Uri(databaseUrlEnv);
            connStringBuilder.Host = databaseUrl.Host;
            connStringBuilder.Port = databaseUrl.Port;
            var items = databaseUrl.UserInfo.Split(new[] { ':' });
            if(items.Length > 0) connStringBuilder.Username = items[0];
            if(items.Length > 1) connStringBuilder.Password = items[1];
        }
        connStringBuilder.Database = "defaultdb";
        Simple(connStringBuilder.ConnectionString);*/
    }
    static void Simple(string connString)
    {
        using(var conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            // Create the "accounts" table.
            using(var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS accounts (id INT PRIMARY KEY, balance INT)", conn))
            {
                cmd.ExecuteNonQuery();
            }
            // Insert two rows into the "accounts" table.
            using(var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "UPSERT INTO accounts(id, balance) VALUES(@id1, @val1), (@id2, @val2)";
                cmd.Parameters.AddWithValue("id1", 1);
                cmd.Parameters.AddWithValue("val1", 1000);
                cmd.Parameters.AddWithValue("id2", 2);
                cmd.Parameters.AddWithValue("val2", 250);
                cmd.ExecuteNonQuery();
            }

            // Print out the balances.
            System.Console.WriteLine("Initial balances:");
            using(var cmd = new NpgsqlCommand("SELECT id, balance FROM accounts", conn))
            using(var reader = cmd.ExecuteReader())
                while(reader.Read())
                    Console.Write("\taccount {0}: {1}\n", reader.GetValue(0), reader.GetValue(1));
        }
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

