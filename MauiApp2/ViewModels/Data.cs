
using System.Reactive;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls;



namespace MauiApp2.ViewModels
{
    [QueryProperty("TargetWeightt", "targetweighttt")]
    [QueryProperty("Name", "namee")]
    public partial class Data : Base

    {
        public BLEService BLEService { get; private set; }


        [ObservableProperty]
        int targetWeightt;

        [ObservableProperty]
        string name;

        Dbservice db;

        //public Dbservice dbservice { get; private set; }
        public IAsyncRelayCommand ConnectToDeviceCandidateAsyncCommand { get; }
        public IAsyncRelayCommand DisconnectFromDeviceAsyncCommand { get; }
        public IAsyncRelayCommand GoBackCommand { get; }
        public IAsyncRelayCommand GoNextCommand { get; }


        
        public IService DataService { get; private set; }
        public ICharacteristic DataCharacteristic { get; private set; }

        private double _progressPercentage;
        public double ProgressPercentage
        {
            get => _progressPercentage;
            set
            {
                SetProperty(ref _progressPercentage, value);
                OnPropertyChanged(nameof(ProgressColor));
            }
        }
       
       public Color ProgressColor => (ProgressPercentage >= 95 && ProgressPercentage <= 105) ? new Color(0, 255, 0) : new Color(255, 0, 0);
        //public bool CanGoNext()
        //{
        //    return ProgressPercentage >= 95 && ProgressPercentage <= 105;
        //}





        //public double Formula.TargetWeight { get; private set; }

        //Formulas repo = new Formulas();

        // double _targetWeight = repo.List()[0].TargetWeight;

        double tareValue = 0;
        double weightv = 0;
        public ICommand TareCommand => new Command(() =>
        {
            tareValue = weightv;
            weightv = 0;
        });


        [ObservableProperty]
        int targetValue;



        [ObservableProperty]
        double weightValue=0;

        //[ObservableProperty]
        //DateTimeOffset timestamp1;


        [ObservableProperty]
        long timestamp2;

        public Data(BLEService bluetoothLEService)
        {
            Title = $"Weight";
            BLEService = bluetoothLEService;
            

            ConnectToDeviceCandidateAsyncCommand = new AsyncRelayCommand(ConnectToDeviceCandidateAsync);

            DisconnectFromDeviceAsyncCommand = new AsyncRelayCommand(DisconnectFromDeviceAsync);
            GoBackCommand = new AsyncRelayCommand(GoBackAsync);
            GoNextCommand = new AsyncRelayCommand(GoNextAsync);

            int _targetWeight = targetWeightt;
            
        }



        
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("///Formulaa");
        }
        async Task GoNextAsync()
        {
            var ingredientsViewModel = new Ingredients_vm();

           // await update(WeightValue);
            if (ProgressPercentage >= 95 && ProgressPercentage <= 105)
            {
                await ingredientsViewModel.next();
                TareCommand.Execute(null);

                await Shell.Current.DisplayAlert($"Switch Ingredient on the Scale", $".", "OK");

            }
            else
            {
                await Shell.Current.DisplayAlert($"Target Weight not reached yet", $"Try Again.", "OK");
            }
            
        }

        //public async Task Tare(double w)
        //{

        //    tareValue = weightv;
        //    var fin = w - targetValue;
        //    weightv = 0;
        //    return fin;
            

        //}
        //public async Task update(double weigh)
        //{

        //    var test = await db.GetFinalAsync(name);
        //    foreach (var ee in test)
        //    {
        //        await db.UpdateActual(ee.Name, weigh, ee.TargetWeight);

        //    }
        //}

        //    var ingredient = await db.GetFinal2Async();
        //    foreach (var ee in ingredient)
        //    {
        //        _batchList.Add(ee);
        //    }
        //}

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
                                //Timestamp1 = DateTimeOffset.Now.LocalDateTime;
                                await DataCharacteristic.StartUpdatesAsync();
                                //var cancellationTokenSource = new CancellationTokenSource();
                                //var cancellationToken = cancellationTokenSource.Token;
                                //while (!cancellationToken.IsCancellationRequested)
                                //{
                                    
                                //    await Task.Delay(1, cancellationToken);
                                //}
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
        
        static Stopwatch sw = Stopwatch.StartNew();
        private async void DataCharacteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)

        {
           // var characteristic = (ICharacteristic)sender;

            var bytes = e.Characteristic.Value;
            //var bytes = await characteristic.ReadAsync();
            //int intValue = BitConverter.ToInt32(bytes, 0);
            string receivedData = Encoding.ASCII.GetString(bytes);
            
            string[] parts = receivedData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            //////Console.WriteLine("Current weight: " + parts[0]);

            double weight = double.Parse(parts[0]);


            int _targetWeight = targetWeightt;

            //int _targetWeight = dbservice.CopyOfFormulaaa;
            //int _targetWeight = int.Parse(Uri.UnescapeDataString(Navigation.Parameters.GetValue<string>("TargetWeight")));
            TargetValue = _targetWeight;
            //TargetValue = 120;
            weightv = weight;
            WeightValue = weightv- tareValue;


            ProgressPercentage = Math.Round(Math.Max(0, (WeightValue / _targetWeight) * 100), 2);


            Timestamp2 = sw.ElapsedMilliseconds;
            sw.Restart();
            //Timestamp2 = DateTimeOffset.Now.LocalDateTime;
            await Task.CompletedTask;
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
                WeightValue = 10;
                TargetValue = 0;
                //Timestamp1 = DateTimeOffset.MinValue;
                Timestamp2 = 0;
                IsBusy = false;
                BLEService.Device?.Dispose();
                BLEService.Device = null;
                await Shell.Current.GoToAsync("//HomePage", true);
            }
        }
    }
}
