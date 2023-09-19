using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.Services
{
    public class CurrentViewService : ICurrentViewService
    {
        private string? _currentView;

        public string? CurrentView { get => _currentView; set => _currentView = value; }
    }
}
