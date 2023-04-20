using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace MauiApp2.Models
{
    public class Batchline
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        //[ForeignKey(typeof(Formula))]
        //public int FormulaId { get; set; }

        [ForeignKey(typeof(Ingredients_model))]
        public double TargetWeight { get; set; }

        public double ActualWeight { get; set; }


    }
}
