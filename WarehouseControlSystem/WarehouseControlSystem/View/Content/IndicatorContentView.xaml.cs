using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarehouseControlSystem.View.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndicatorContentView : ContentView
    {
        public IndicatorContentView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //
        }
    }
}