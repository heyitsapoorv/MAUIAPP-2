using CommunityToolkit.Maui.Core.Extensions;
using MauiApp2.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Command = MvvmHelpers.Commands.Command;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Xml.Linq;

namespace MauiApp2.ViewModels
{
    public partial class Formulas : Base
    {
        int size;
       

        Dbservice db;
        // public Dbservice dbService { get;  set; }
        // private readonly Dbservice dbService;
        //public ObservableRangeCollection<Formula> Formulaaa { get; set; }
        public ObservableCollection<Formula> Formulaaa { get; set; } = new ObservableCollection<Formula>();
        //public AsyncCommand RefreshCommand { get; }
        public IAsyncRelayCommand GoToIngPageAsyncCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        //IDbservice dbService;
        public Formulas()
        {
            //Formulaaa = new ObservableRangeCollection<Formula>();
            db = new Dbservice();
            GoToIngPageAsyncCommand = new AsyncRelayCommand<Formula>(async (formula) => await GoToIngPageAsync(formula));
            RefreshCommand = new AsyncCommand(Refresh);
            //LoadFormulasAsync();
        }

        //public Formulas(Dbservice dataService)
        //{ 
        //    dbService = dataService;
        //    //Formulas repo = new Formulas();
        //    //dbService = new Dbservice();
        //    //var dbService = serviceProvider.GetService<IDbservice>();
        //    //dbService = DependencyService.Get<IDbservice>();


        //}
        //async Task LoadFormulasAsync()
        //{
        //    try
        //    {
        //        var repo = await db.GetAllFormulasAsync();
        //        foreach (var item in repo)
        //        {
        //            if (item is Formula formula)
        //            {
        //                Formulaaa.Add(formula);
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //    }

        //}
        async Task Refresh()
        {
            IsBusy = true;

#if DEBUG
            await Task.Delay(500);
            Formulaaa.Clear();

#endif
       
            
                var repo = await db.GetAllFormulasAsync();
                foreach (var item in repo)
                {
                    
                        Formulaaa.Add(item);
                    
                }
            

            //catch (Exception ex)
            //{

            //}
           
        }
        private async Task<int> PromptUserForTargetValueAsync()
        {
          string input = await Shell.Current.DisplayPromptAsync("Enter Batch Size", "Please enter the batch size:");

           if (!string.IsNullOrEmpty(input))
          {

               size = int.Parse(input);
            }

            return size;
        }

        async Task GoToIngPageAsync(Formula formula)
        {
            if (formula == null)
            {
                return;
            }
            else
            {
                
                int size = await PromptUserForTargetValueAsync();
                await db.UpdateFormulaQuantity(formula.Id, size);

                

                await Shell.Current.GoToAsync($"{nameof(Ingredients)}?id={formula.Id}&sizee={size}");

                //await Shell.Current.GoToAsync($"{nameof(DataPage)}?targetweighttt={formula.TargetWeight}");
            }
        }
    }
   

    //    public Formulas()
    //    {
    //        string dbPath = Constants.DatabasePath;
    //        bool databaseExists = File.Exists(dbPath);

    //        if (databaseExists)
    //        {
    //            _connection = new SQLiteConnection(dbPath);
    //            _connection.DropTable<Formula>();
    //        }

    //        // {
    //        _connection = new SQLiteConnection(dbPath);
    //        _connection.CreateTable<Formula>();

    //            if (!_connection.Table<Formula>().Any())
    //            {
    //                // Insert 3 rows of random data
    //                _connection.Insert(new Formula { Name = "Formula 1", TargetWeight = 100});
    //                _connection.Insert(new Formula { Name = "Formula 2", TargetWeight = 180 });
    //                _connection.Insert(new Formula { Name = "Formula 3", TargetWeight = 305});
    //            }

    //        GoToDatPageAsyncCommand = new AsyncRelayCommand<Formula>(async (formula) => await GoToDatPageAsync(formula));
    //    }

    //    public List<Formula> List()
    //    {
    //        return _connection.Table<Formula>().ToList();
    //    }

    //async task gotodatpageasync(formula formula)
    //    {
    //        if (formula == null)
    //        {
    //            return;
    //        }
    //        else
    //        {
    //            await shell.current.gotoasync($"{nameof(datapage)}?targetweight={formula.targetweight}");
    //        }
    //    }
    //}
    //    public static class Constants
    //{
    //    public const string DatabaseFilename = "Mydata.db";

    //    public const SQLite.SQLiteOpenFlags Flags =
    //        // open the database in read/write mode
    //        SQLite.SQLiteOpenFlags.ReadWrite |
    //        // create the database if it doesn't exist
    //        SQLite.SQLiteOpenFlags.Create |
    //        // enable multi-threaded database access
    //        SQLite.SQLiteOpenFlags.SharedCache;

    //    public static string DatabasePath =>
    //        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    //}
    //dbService = DependencyService.Get<IDbservice>();
}