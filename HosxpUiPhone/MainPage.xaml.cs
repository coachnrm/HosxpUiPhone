using Newtonsoft.Json;

namespace HosxpUiPhone
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editCustomerId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => listView.ItemsSource = await _dbService.GetCustomers());
        }

   

        //private async void saveButton_Clicked(object sender, EventArgs e)
        //{
        //    if (_editCustomerId == 0)
        //    {
        //        // Add Customer

        //        await _dbService.Create(new Customer
        //        {
        //            CustomerName = nameEntryField.Text,
        //            Email = emailEntryField.Text,
        //            Mobile = mobileEntryField.Text
        //        });
        //    }
        //    else
        //    {
        //        // Edit Customer

        //        await _dbService.Update(new Customer
        //        {
        //            Id = _editCustomerId,
        //            CustomerName = nameEntryField.Text,
        //            Email = emailEntryField.Text,
        //            Mobile = mobileEntryField.Text
        //        });

        //        _editCustomerId = 0;
        //    }

        //    nameEntryField.Text = string.Empty;
        //    emailEntryField.Text = string.Empty;
        //    mobileEntryField.Text = string.Empty;

        //    listView.ItemsSource = await _dbService.GetCustomers();
        //}

        //private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    var customer = (Customer)e.Item;
        //    var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        //    switch (action)
        //    {
        //        case "Edit":

        //            _editCustomerId = customer.Id;
        //            nameEntryField.Text = customer.CustomerName;
        //            emailEntryField.Text = customer.Email;
        //            mobileEntryField.Text = customer.Mobile;

        //            break;
        //        case "Delete":

        //            await _dbService.Delete(customer);
        //            listView.ItemsSource = await _dbService.GetCustomers();

        //            break;
        //    }
        //}

        private async void OnCallApiBtnClicked(object sender, EventArgs e)
        {
            var httpClient = new HttpClient();
            var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                            ? "http://10.0.2.2:5094"
                            : "http://localhost:5094";
            var response = await httpClient.GetAsync($"{baseUrl}/api/Ipd/getward");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                // Assuming the API returns a list of customers as JSON
                var wards = JsonConvert.DeserializeObject<List<Ward>>(data);

                // Bind data to ListView
                listView.ItemsSource = wards;
            }
            else
            {
                await DisplayAlert("Error", "Failed to fetch data from API", "OK");
            }
        }
    }

}
