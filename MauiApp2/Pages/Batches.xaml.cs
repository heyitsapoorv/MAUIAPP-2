using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Pages
{
    public partial class Batches : ContentPage
    {
        private Batch _vm;
        //Dbservice dbservice;
        //public IAsyncRelayCommand GoToIngPageAsyncCommand { get; }
        //public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();
        //public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();

        public Batches()
        {
            //GoToIngPageAsyncCommand = new AsyncRelayCommand<Formula>(async (formula) => await GoToIngPageAsync(formula));

            InitializeComponent();
            _vm = new Batch();

            BindingContext = _vm;



            //this.BindingContext = this;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var v2 = (Batch)BindingContext;
            //await vm1.RefreshCommand2.ExecuteAsync();
            await v2.RefreshCommand3.ExecuteAsync();



        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}