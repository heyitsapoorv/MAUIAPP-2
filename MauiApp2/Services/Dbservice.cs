using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using MauiApp2.Models;

namespace MauiApp2.Services
{

    public class Dbservice
    {
        SQLiteConnection _connection;
        public Dbservice()
        {

        }

        async Task Init()
        {

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "neeww.db");
       

            // Check if the database file exists before creating it
            if (!File.Exists(databasePath))
            {
                
                _connection = new SQLiteConnection(databasePath);

                _connection.CreateTable<Formula>();
                _connection.CreateTable<Ingredients_model>();
                _connection.CreateTable<IngredRatio>();

                // Insert 3 rows of random data into Formula table
                _connection.Insert(new Formula { Id = 1, Name = "Cake Recipe ", BatchSize = 100 });
                _connection.Insert(new Formula { Id = 2, Name = "Other Recipe", BatchSize = 180 });
                _connection.Insert(new Formula { Id = 3, Name = "Some Recipe ", BatchSize = 305 });

                // Insert 2 rows of random data into Ingredient table for Formula 1
                _connection.Insert(new IngredRatio { FormulaId=1, Name = "Sugar", Quantity = 50 });
                _connection.Insert(new IngredRatio { FormulaId = 1, Name = "Flour", Quantity = 20 });
                _connection.Insert(new IngredRatio { FormulaId = 1, Name = "Water", Quantity = 20 });
                _connection.Insert(new IngredRatio { FormulaId = 1, Name = "Salt", Quantity = 10 });

                // Insert 1 row of random data into Ingredient table for Formula 2
                _connection.Insert(new IngredRatio { FormulaId = 2, Name = "Ingredient C", Quantity = 15 });

                _connection.Insert(new IngredRatio { FormulaId = 3, Name = "Ingredient D", Quantity = 55 });


                _connection.CreateTable<Ingredients_model>();

                // Insert rows of data into Ingredients_model table
                _connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Sugar", TargetWeight = 0 });
                _connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Flour", TargetWeight = 0 });
                _connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Water", TargetWeight = 0 });
                _connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Salt", TargetWeight = 0 });

                // Insert 1 row of random data into Ingredient table for Formula 2
                _connection.Insert(new Ingredients_model { FormulaId = 2, Name = "Ingredient C", TargetWeight = 0 });

                _connection.Insert(new Ingredients_model { FormulaId = 3, Name = "Ingredient D", TargetWeight = 0 });


                _connection.CreateTable<Batchline>();

                // Insert rows of data into Ingredients_model table
                _connection.Insert(new Batchline { Name = "Sugar", TargetWeight = 0, ActualWeight = 0 });
                _connection.Insert(new Batchline { Name = "Flour", TargetWeight = 0, ActualWeight = 0 });
                _connection.Insert(new Batchline { Name = "Water", TargetWeight = 0, ActualWeight = 0 });
                _connection.Insert(new Batchline { Name = "Salt", TargetWeight = 0, ActualWeight = 0 });

                // Insert 1 row of random data into Ingredient table for Formula 2
                _connection.Insert(new Batchline { Name = "Ingredient C", TargetWeight = 0, ActualWeight = 0 });

                _connection.Insert(new Batchline { Name = "Ingredient D", TargetWeight = 0, ActualWeight = 0 });
            }
            else
            {
                _connection = new SQLiteConnection(databasePath);
                //_connection.DropTable<Ingredients_model>();
                //_connection.CreateTable<Ingredients_model>();

                //// Insert rows of data into Ingredients_model table
                //_connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Ingredient A", TargetWeight = 0});
                //_connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Ingredient B", TargetWeight = 0 });
                //_connection.Insert(new Ingredients_model { FormulaId = 1, Name = "Ingredient E", TargetWeight = 0 });

                //// Insert 1 row of random data into Ingredient table for Formula 2
                //_connection.Insert(new Ingredients_model { FormulaId = 2, Name = "Ingredient C", TargetWeight = 0});

                //_connection.Insert(new Ingredients_model { FormulaId = 3, Name = "Ingredient D", TargetWeight = 0});
            }
            

            
            
        }

        public async Task UpdateFormulaQuantity(int formulaId, int newQuantity)
        {
            await Init();

            // Use an UPDATE statement to update the Quantity value for the specified formula ID
            var sql = $"UPDATE Formula SET BatchSize = {newQuantity} WHERE Id = {formulaId}";
            _connection.Execute(sql);
        }

        public async Task UpdateIngreQuantity(int formId, double newQuantity,int id)
        {
            await Init();

            // Use an UPDATE statement to update the Quantity value for the specified formula ID
            var sql = $"UPDATE Ingredients_model SET TargetWeight = {newQuantity} WHERE FormulaId = {formId} AND Id = { id }";
            _connection.Execute(sql);
        }

        //public async Task UpdateActual(string str, double newQuantity, double target)
        //{
        //    await Init();

        //    // Use an UPDATE statement to update the Quantity value for the specified formula ID
        //    var sql = $"UPDATE Batchline SET ActualWeight = {newQuantity} WHERE Name= {str} And TargetWeight ={target}";
        //    _connection.Execute(sql);
        //}




        public async Task<List<Formula>> GetFormula()
        {
            await Init();
            var formm = _connection.Table<Formula>().ToList();
            return formm;
        }
        public async Task<List<Formula>> GetAllFormulasAsync()
        {
            var formulaList = await GetFormula();
            return formulaList;
        }


        public async Task<List<IngredRatio>> GetIngredientsAsync(int id)
        {
            await Init();

            var ingre = _connection.Table<IngredRatio>().Where(c => c.FormulaId == id).ToList();

            return ingre;
        }

        public async Task<List<Ingredients_model>> GetIngredients2Async(int id)
        {
            await Init();

            var ingre = _connection.Table<Ingredients_model>().Where(c => c.FormulaId == id).ToList();

            return ingre;
        }
        //public async Task<List<Batchline>> GetFinalAsync(string str)
        //{
        //    await Init();

        //    var ingre = _connection.Table<Batchline>().Where(c => c.Name == str).ToList();

        //    return ingre;
        //}
        public async Task<List<Batchline>> GetFinal2Async()
        {
            await Init();

            var ingre = _connection.Table<Batchline>().ToList();

            return ingre;
        }
    }
}

