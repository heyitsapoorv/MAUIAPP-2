using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.ViewModels
{
    public partial class Home : Base
    {
        BLEService BLEService;
        public ObservableCollection<BLEDevice> Devices { get; } = new();
        public IAsyncRelayCommand GoToDataPageAsyncCommand { get; }
        public IAsyncRelayCommand ScanNearbyDevicesAsyncCommand { get; }
        public IAsyncRelayCommand CheckBluetoothAvailabilityAsyncCommand { get; }

        public Home(BLEService bluetoothLEService)
        {
            Title = $"Scan and select device";
            BLEService = bluetoothLEService;
            GoToDataPageAsyncCommand = new AsyncRelayCommand<BLEDevice>(async (device) => await GoToDataPageAsync(device));
            ScanNearbyDevicesAsyncCommand = new AsyncRelayCommand(ScanDevicesAsync);
            CheckBluetoothAvailabilityAsyncCommand = new AsyncRelayCommand(CheckBluetoothAvailabilityAsync);

        }

        async Task GoToDataPageAsync(BLEDevice device)
        {
            if (IsScanning)
            {
                await BLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
                return;
            }
            if (device == null)
            {
                return;
            }

            BLEService.NewDeviceCandidateFromHomePage = device;

            Title = $"{device.Name}";

            await Shell.Current.GoToAsync("//DataPage", true);
        }

        async Task ScanDevicesAsync()
        {
            if (IsScanning)
            {
                return;
            }

            if (!BLEService.BluetoothLE.IsAvailable)
            {
                Debug.WriteLine($"Bluetooth is missing.");
                await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                return;
            }

#if ANDROID
            PermissionStatus permissionStatus = await BLEService.CheckBluetoothPermissions();
            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await BLEService.RequestBluetoothPermissions();
                if (permissionStatus != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert($"Bluetooth LE permissions", $"Bluetooth LE permissions are not granted.", "OK");
                    return;
                }
            }
#elif IOS
#elif WINDOWS
#endif

            try
            {
                if (!BLEService.BluetoothLE.IsOn)
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                    return;
                }

                IsScanning = true;

                List<BLEDevice> device = await BLEService.ScanForDevicesAsync();

                if (device.Count == 0)
                {
                    await BLEService.ShowToastAsync($"Unable to find nearby Bluetooth LE devices. Try again.");
                }

                if (Devices.Count > 0)
                {
                    Devices.Clear();
                }

                foreach (var dev in device)
                {
                    Devices.Add(dev);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get nearby Bluetooth LE devices: {ex.Message}");
                await Shell.Current.DisplayAlert($"Unable to get nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
            }
            finally
            {
                IsScanning = false;
            }
        }

        async Task CheckBluetoothAvailabilityAsync()
        {
            if (IsScanning)
            {
                return;
            }

            try
            {
                if (!BLEService.BluetoothLE.IsAvailable)
                {
                    Debug.WriteLine($"Error: Bluetooth is missing.");
                    await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                    return;
                }

                if (BLEService.BluetoothLE.IsOn)
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is on", $"You are good to go.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to check Bluetooth availability: {ex.Message}");
                await Shell.Current.DisplayAlert($"Unable to check Bluetooth availability", $"{ex.Message}.", "OK");
            }
        }
    }

}
