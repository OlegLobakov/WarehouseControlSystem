using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.ViewModel.Base;

namespace WarehouseControlSystem.View.Pages.Base
{
    public class SchemeBaseView : ContentView
    {
        public NAVBaseViewModel Model { get; set; }

        public SchemeBaseView(NAVBaseViewModel model)
        {
            Model = model;
            BindingContext = Model;
        }
    }
}
