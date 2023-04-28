﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Models
{
    public class Ingredients_model
    {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string Name { get; set; }
            public double TargetWeight { get; set; }

        [ForeignKey(nameof(Formula))]
        public int FormulaId { get; set; }



    }
}
