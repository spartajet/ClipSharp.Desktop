using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace ClipSharp.Core.ViewModels
{
    public class DisplayWindowViewModel
    {
        private ILogger<DisplayWindowViewModel> logger;
        private ISqlSugarClient db;
        public DisplayWindowViewModel(ILogger<DisplayWindowViewModel> logger, ISqlSugarClient db)
        {
            this.logger = logger;
            this.db = db;

        }
    }
}
