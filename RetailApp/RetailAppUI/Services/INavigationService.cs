using RetailAppUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppUI.Services
{
    public interface INavigationService
    {
        BaseViewModel CurrentView { get; }
        void NavigateTo<T>() where T : BaseViewModel;
    }
}
