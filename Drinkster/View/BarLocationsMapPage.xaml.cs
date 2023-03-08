using System;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Drinkster;

public partial class BarLocationsMapPage : ContentPage
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?view=net-maui-7.0&tabs=android
    // Geolocation
    public BarLocationsMapPage()
	{
		InitializeComponent();

        //MockStartup();
        OnMapLoaded();

	}

    private void MockStartup()
    {
        var location = new Location(56.1496278, 10.2134046);
        var radiusKm = Distance.FromKilometers(10);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, radiusKm));

        var pin = new Pin()
        {
            Address = "Kannikegade 10, 8000 Aarhus C",
            Label = "Old Irish Pub",
            Type = PinType.Place,
            Location = new Location(56.156136, 10.20988)
        };
        map.Pins.Add(pin);
    }

    private async void OnMapLoaded()
    {
        await SetCurrentLocation();

        var pin = new Pin()
        {
            Address = "Downing Street 10",
            Label = "Random Pin",
            Type = PinType.Place,
            Location = new Location(21, 21)
        };
        map.Pins.Add(pin);
    }

    private async Task SetCurrentLocation()
    {
        var permission = await CheckAndRequestLocationPermission();
        if (permission == PermissionStatus.Granted)
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                //Get last known location incase location cant be retrieved
                var lastKnownLocation = await GetCachedLocation();
                var radiusKm = Distance.FromKilometers(1);

                if (location == null)
                {
                    if (lastKnownLocation == null)
                    {
                        Console.WriteLine($"Last known Location cannot be retrieved, value: {lastKnownLocation}");
                    }
                    //Move to lastknow location
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(location, radiusKm));
                };
                map.MoveToRegion(MapSpan.FromCenterAndRadius(location, radiusKm));
                var pin = new Pin()
                {
                    Address = "Kannikegade 10, 8000 Aarhus C",
                    Label = "Old Irish Pub",
                    Type = PinType.Place,
                    Location = new Location(56.156136, 10.20988)
                };
                map.Pins.Add(pin);

            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine($"Last known Location cannot be retrieved, value: {ex}");
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }

    public async Task<Location> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return location;
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            Console.WriteLine($"Handle not supported on device exception, exception: {fnsEx}");
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            Console.WriteLine($"Handle not enabled on device exception, exception: {fneEx}");
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            Console.WriteLine($"Handle permission exception, exception: {pEx}");
        }
        catch (Exception ex)
        {
            // Unable to get location
            Console.WriteLine($"Unable to get location, exception: {ex}");
        }

        return null;
    }

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }
}
