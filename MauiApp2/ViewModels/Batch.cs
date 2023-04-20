using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MauiApp2.ViewModels
{
    public partial class Batch : Base
    {
       
        public ObservableCollection<Batchline> _batchList { get; set; } = new ObservableCollection<Batchline>();

        public AsyncCommand RefreshCommand3 { get; }
        public Batch()
        {
            RefreshCommand3 = new AsyncCommand(Refresh);
        }
            
            
        
        async Task Refresh()
        {
            IsBusy = true;

#if DEBUG
            await Task.Delay(500);

#endif
            _batchList.Clear();
            var db = new Dbservice();

            var ingredients = await db.GetFinal2Async();
            
                //var ingredients = await db.GetIngredientsAsync();
                foreach (var e in ingredients)
            {
                
                _batchList.Add(e);

            }

            
        }
    }
}
