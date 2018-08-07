using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarehouseControlSystem.ViewModel;

namespace WarehouseControlSystem.View.Content
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IndicatorView : ContentView
	{
        public static readonly BindableProperty WidthHeightProperty = BindableProperty.Create("WidthHeight", typeof(decimal), typeof(IndicatorView), (decimal)200);
        public decimal WidthHeight
        {
            get { return (decimal)GetValue(WidthHeightProperty); }
            set { SetValue(WidthHeightProperty, value); }
        }

        public IndicatorView()
        {
            InitializeComponent();
        }
	}
}