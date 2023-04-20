
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;


namespace MauiApp2.Pages
{
    //[QueryProperty("Id", "id")]
    public partial  class Ingredients : ContentPage
    {
        //public ObservableCollection<Ingredients_model> _ingredientsList { get; set; } = new ObservableCollection<Ingredients_model>();
        //[ObservableProperty]
        //public int id{get; set;}
        private Ingredients_vm _vm;
        public Ingredients()
        {
            InitializeComponent();
            _vm = new Ingredients_vm();
            
            BindingContext = _vm;

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
            var vm1 = (Ingredients_vm)BindingContext;
            //await vm1.RefreshCommand2.ExecuteAsync();
            await vm1.RefreshCommand2.ExecuteAsync();
            //var db = new Dbservice();
            //int _id = id;
            //var ingredients = await db.GetIngredientsAsync(_id);
            ////var ingredients = await db.GetIngredientsAsync();
            //foreach (var e in ingredients)
            //{
            //    _ingredientsList.Add(e);
            //}


        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}

