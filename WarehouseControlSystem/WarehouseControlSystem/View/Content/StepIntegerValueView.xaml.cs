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
	public partial class StepIntegerValueView : ContentView
	{
        public event Action ValueChanges;

        public static readonly BindableProperty StepBackgroundColorProperty = BindableProperty.Create(nameof(StepBackgroundColor), typeof(Color), typeof(StepIntegerValueView), Color.White, BindingMode.Default, null, Changed);
        public Color StepBackgroundColor
        {
            get { return (Color)GetValue(StepBackgroundColorProperty); }
            set { SetValue(StepBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(StepIntegerValueView), 0, BindingMode.TwoWay, null, Changed);
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void Changed(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as StepIntegerValueView;
            instance?.RaiseEvents();
        }

        private void RaiseEvents()
        {
            if (ValueChanges is Action)
            {
                ValueChanges();
            }
        }

        public StepIntegerValueView()
		{
			InitializeComponent();
		}

        private void Button_Clicked_Minus(object sender, EventArgs e)
        {
            Value = Value - 1;
        }

        private void Button_Clicked_Plus(object sender, EventArgs e)
        {
            Value = Value + 1;
        }
    }
}