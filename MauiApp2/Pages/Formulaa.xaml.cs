
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;


namespace MauiApp2.Pages
{
    public partial class Formulaa : ContentPage
    {
        private Formulas _viewmodel;
        //Dbservice dbservice;
        //public IAsyncRelayCommand GoToIngPageAsyncCommand { get; }
        //public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();
        //public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();
        
        public Formulaa()
        {
            //GoToIngPageAsyncCommand = new AsyncRelayCommand<Formula>(async (formula) => await GoToIngPageAsync(formula));
            
                InitializeComponent();
            _viewmodel = new Formulas();
            BindingContext = _viewmodel;



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

        protected async  override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (Formulas)BindingContext;
            await vm.RefreshCommand.ExecuteAsync();



        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

      
    }
}

