
namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async Task LoginFunctionalityAsync(object sender, EventArgs e)
        {
            // extract the data from the fields 

            // string username = entry.Text;
            // string password = entry2.Text;

            string url = "https://api.example.com/data"; // Replace with your API URL

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send a GET request to the specified URL
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not successful (200-299)

                    // Read the response content as a string
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a C# object
                    var data = JsonConvert.DeserializeObject<MyDataStructure>(jsonResponse);

                    // Output or process the extracted data
                    Console.WriteLine($"ID: {data.Id}, Name: {data.Name}, Description: {data.Description}");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }

            }
    }

}
