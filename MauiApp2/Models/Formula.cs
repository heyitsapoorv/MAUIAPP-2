
using MauiApp2.Pages;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MauiApp2.Models
{
    public class Formula
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
       
        public string Name { get; set; }

        public int BatchSize { get; set; }

        [OneToMany]
        public List<Ingredients_model> Ingredients_model { get; set; }
    }
}
