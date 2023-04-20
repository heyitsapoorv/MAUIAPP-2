using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteNetExtensions.Attributes;
using System.Threading.Tasks;

namespace MauiApp2.Models
{
    public class IngredRatio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(typeof(Formula))]
        public int FormulaId { get; set; }

        [ForeignKey(typeof(Ingredients_model))]
        public int IngredientId { get; set; }

        public double Quantity { get; set; }
    }
}

