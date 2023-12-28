using Microsoft.Maui.Devices.Sensors;

namespace FindMeMAUIApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        string baseUrl = "https://bing.com/maps/default.aspx?cp=";

        public string UserName { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFindMeClicked(object sender, EventArgs e)
        {
            var permissions = await Permissions.CheckStatusAsync <Permissions.LocationWhenInUse>();

            if (permissions == PermissionStatus.Granted)
            {
                await ShareLocation();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Permissions Error", "You have not granted the app permission to access your location.", "Ok");
                var requested = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (requested == PermissionStatus.Granted)
                {
                    await ShareLocation();
                }
                else
                {
                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform==DevicePlatform.MacCatalyst)
                    {
                        await App.Current.MainPage.DisplayAlert("Location Required"," Location is required to share it. Please enable location for this app in Settings.","Ok");

                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Location Required", " Location is required to share it. We'll ask again next time", "Ok");

                    }
                }
            }
        }
        private async Task ShareLocation()
        {
            UserName = UserNameEntry.Text;
            var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(locationRequest);
            await Share.RequestAsync(new ShareTextRequest { 
            Subject="Find Me!",
            Title="Find Me!",
            Text=$"{UserName} is sharing their location with you",
            Uri= $"{baseUrl}{location.Latitude}~{location.Longitude}"
            });
        }
    }
}