
using System.Text;

namespace MauiApp2.ViewModels
{
    public partial class Data : Base

    {

        public BLEService BLEService { get; private set; }

        public IAsyncRelayCommand ConnectToDeviceCandidateAsyncCommand { get; }
        public IAsyncRelayCommand DisconnectFromDeviceAsyncCommand { get; }

        public IService DataService { get; private set; }
        public ICharacteristic DataCharacteristic { get; private set; }
        public Data(BLEService bluetoothLEService)
        {
            Title = $"Weight";


            BLEService = bluetoothLEService;

            ConnectToDeviceCandidateAsyncCommand = new AsyncRelayCommand(ConnectToDeviceCandidateAsync);

            DisconnectFromDeviceAsyncCommand = new AsyncRelayCommand(DisconnectFromDeviceAsync);


        }
        [ObservableProperty]
        int targetValue;

        [ObservableProperty]
        double weightValue;

        [ObservableProperty]
        double progressPercentage;


        [ObservableProperty]
        DateTimeOffset timestamp;


        private async Task ConnectToDeviceCandidateAsync()
        {


            if (IsBusy)
            {
                return;
            }

            if (BLEService.NewDeviceCandidateFromHomePage.Id.Equals(Guid.Empty))
            {
                #region read device id from storage
                var device_name = await SecureStorage.Default.GetAsync("device_name");
                var device_id = await SecureStorage.Default.GetAsync("device_id");
                if (!string.IsNullOrEmpty(device_id))
                {
                    BLEService.NewDeviceCandidateFromHomePage.Name = device_name;
                    BLEService.NewDeviceCandidateFromHomePage.Id = Guid.Parse(device_id);
                }
                #endregion read device id from storage
                else
                {
                    await BLEService.ShowToastAsync($"Select a Bluetooth LE device first. Try again.");
                    return;
                }
            }

            if (!BLEService.BluetoothLE.IsOn)
            {
                await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                return;
            }

            if (BLEService.Adapter.IsScanning)
            {
                await BLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
                return;
            }

            try
            {
                IsBusy = true;

                if (BLEService.Device != null)
                {
                    if (BLEService.Device.State == DeviceState.Connected)
                    {

                        if (BLEService.Device.Id.Equals(BLEService.NewDeviceCandidateFromHomePage.Id))
                        {
                            await BLEService.ShowToastAsync($"{BLEService.Device.Name} is already connected.");
                            return;
                        }

                        if (BLEService.NewDeviceCandidateFromHomePage != null)
                        {
                            #region another device
                            if (!BLEService.Device.Id.Equals(BLEService.NewDeviceCandidateFromHomePage.Id))
                            {
                                Title = $"{BLEService.NewDeviceCandidateFromHomePage.Name}";
                                await DisconnectFromDeviceAsync();
                                await BLEService.ShowToastAsync($"{BLEService.Device.Name} has been disconnected.");
                            }
                            #endregion another device
                        }
                    }
                }

                BLEService.Device = await BLEService.Adapter.ConnectToKnownDeviceAsync(BLEService.NewDeviceCandidateFromHomePage.Id);

                if (BLEService.Device.State == DeviceState.Connected)

                {
                    string input = await Shell.Current.DisplayPromptAsync("Enter target value", "Please enter the target value:");

                    if (!string.IsNullOrEmpty(input))
                    {
                        TargetValue = int.Parse(input);
                    }

                    DataService = await BLEService.Device.GetServiceAsync(DataUuid.DataServiceUuid);
                    if (DataService != null)
                    {
                        DataCharacteristic = await DataService.GetCharacteristicAsync(DataUuid.DataCharacteristicUuid);
                        if (DataCharacteristic != null)
                        {
                            if (DataCharacteristic.CanUpdate)
                            {
                                Title = $"{BLEService.Device.Name}";

                                #region save device id to storage
                                await SecureStorage.Default.SetAsync("device_name", $"{BLEService.Device.Name}");
                                await SecureStorage.Default.SetAsync("device_id", $"{BLEService.Device.Id}");
                                #endregion save device id to storage

                                DataCharacteristic.ValueUpdated += DataCharacteristic_ValueUpdated;
                                await DataCharacteristic.StartUpdatesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to connect to {BLEService.NewDeviceCandidateFromHomePage.Name} {BLEService.NewDeviceCandidateFromHomePage.Id}: {ex.Message}.");
                await Shell.Current.DisplayAlert($"{BLEService.NewDeviceCandidateFromHomePage.Name}", $"Unable to connect to {BLEService.NewDeviceCandidateFromHomePage.Name}.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        //private void OnButtonClicked(object sender, EventArgs e)
        //{
        //        label.Text = "number";

        //}
        //private async Task<int> PromptUserForTargetValueAsync()
        //{
        //    string input = await Shell.Current.DisplayPromptAsync("Enter target value", "Please enter the target value:");

        //    if (!string.IsNullOrEmpty(input))
        //    {

        //        TargetValue = int.Parse(input);
        //    }

        //    return TargetValue;
        //}

        private async void DataCharacteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)

        {
            ////int targetValue = await PromptUserForTargetValueAsync();

            var bytes = e.Characteristic.Value;
            //int intValue = BitConverter.ToInt32(bytes, 0);
            string receivedData = Encoding.ASCII.GetString(bytes);
            
            string[] parts = receivedData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            //////Console.WriteLine("Current weight: " + parts[0]);

            double weight = double.Parse(parts[0]);
            WeightValue = weight;


            ProgressPercentage = Math.Min(100, (weight / TargetValue) * 100);


            Timestamp = DateTimeOffset.Now.LocalDateTime;
        }

        private async Task DisconnectFromDeviceAsync()
        {
            if (IsBusy)
            {
                return;
            }

            if (BLEService.Device == null)
            {
                await BLEService.ShowToastAsync($"Nothing to do.");
                return;
            }

            if (!BLEService.BluetoothLE.IsOn)
            {
                await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                return;
            }

            if (BLEService.Adapter.IsScanning)
            {
                await BLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
                return;
            }

            if (BLEService.Device.State == DeviceState.Disconnected)
            {
                await BLEService.ShowToastAsync($"{BLEService.Device.Name} is already disconnected.");
                return;
            }

            try
            {
                IsBusy = true;

                await DataCharacteristic.StopUpdatesAsync();

                await BLEService.Adapter.DisconnectDeviceAsync(BLEService.Device);

                DataCharacteristic.ValueUpdated -= DataCharacteristic_ValueUpdated;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to disconnect from {BLEService.Device.Name} {BLEService.Device.Id}: {ex.Message}.");
                await Shell.Current.DisplayAlert($"{BLEService.Device.Name}", $"Unable to disconnect from {BLEService.Device.Name}.", "OK");
            }
            finally
            {
                Title = "Weight";
                WeightValue = 0;
                TargetValue = 171;
                Timestamp = DateTimeOffset.MinValue;
                IsBusy = false;
                BLEService.Device?.Dispose();
                BLEService.Device = null;
                await Shell.Current.GoToAsync("//HomePage", true);
            }
        }
    }
}
