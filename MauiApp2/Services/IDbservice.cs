using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Services
{
    public interface IDbservice
    {
        Task<IEnumerable<Formula>> GetFormula();
        Task<IEnumerable<Formula>> GetAllFormulasAsync();
        Task<IEnumerable<Ingredients_model>> GetIngredientsAsync(int id);
        
    }
}
