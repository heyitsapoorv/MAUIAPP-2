

namespace MauiApp2.ViewModels
{
    public partial class Base : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotScanning))]
        bool isScanning;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        public bool IsNotScanning => !isScanning;
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
    }

}
