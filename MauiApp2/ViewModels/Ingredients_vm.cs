
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MvvmCross.Binding.ExpressionParse.MvxParsedExpression;

namespace MauiApp2.ViewModels
{
    [QueryProperty("Id", "id")]
    [QueryProperty("Size", "sizee")]


    public partial class Ingredients_vm : Base
    {
        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public int size;

        //double val;
        

        static int ty;
        public static ObservableCollection<Ingredients_model> tt { get; set; }
        public ObservableCollection<Ingredients_model> _ingredientsList { get; set; } = new ObservableCollection<Ingredients_model>();
        //public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();
        public IAsyncRelayCommand GoToDatPageAsyncCommand { get; }
        public AsyncCommand RefreshCommand2 { get; }
        private static int _currentIndex = 0;
        private static int iindex = 0;

        public Ingredients_vm()
        {
            
            GoToDatPageAsyncCommand = new AsyncRelayCommand<Ingredients_model>(async (Ind) => await GoToDatPageAsync(Ind));
            RefreshCommand2 = new AsyncCommand(Refresh);
           
         

        }
        

        async Task Refresh()
        {
            IsBusy = true;

#if DEBUG
            await Task.Delay(500);

#endif
            _ingredientsList.Clear();

            var db = new Dbservice();
            int _id = id;
            var ingredients = await db.GetIngredientsAsync(_id);
            //var ingredients = await db.GetIngredientsAsync();
            foreach (var e in ingredients)
            {
                double ff = 0;
                double val = e.Quantity;
                ff = ((val / 100) * size);
                await db.UpdateIngreQuantity(e.FormulaId, ff, e.Id);
                //_ingredientsList.Add(e);
                
            }

            var ingredient = await db.GetIngredients2Async(_id);
            foreach (var ee in ingredient)
            {
                _ingredientsList.Add(ee);
            }

            ty = _ingredientsList.Count;
            tt = _ingredientsList;

        }
        public async Task next()
        {

            if (_currentIndex < ty-1)
            {

                _currentIndex++;
                iindex = _currentIndex;
                await GoToDatPageAsync(tt[_currentIndex]);

            }
            if (iindex == ty - 1)
            {
                await Shell.Current.DisplayAlert($"This is the Last Ingredient in the formula", $"Please click finish.", "OK");
            }

                
            


        }

        private async Task GoToDatPageAsync(Ingredients_model Ind)
        {
            if (Ind == null)
            {
                return;
            }
            else
            {
                
                await Shell.Current.GoToAsync($"{nameof(DataPage)}?targetweighttt={Ind.TargetWeight}&namee={Ind.Name}");

            }

            //}
        }
    }
}
