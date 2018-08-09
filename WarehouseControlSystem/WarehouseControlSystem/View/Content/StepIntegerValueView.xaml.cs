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
        public event Action<int,int> ValueChanges;

        public static readonly BindableProperty StepBackgroundColorProperty = BindableProperty.Create(nameof(StepBackgroundColor), typeof(Color), typeof(StepIntegerValueView), Color.White, BindingMode.Default, null, null);
        public Color StepBackgroundColor
        {
            get { return (Color)GetValue(StepBackgroundColorProperty); }
            set { SetValue(StepBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(StepIntegerValueView), 0, BindingMode.TwoWay, null, ValueChanged);
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var instance = bindable as StepIntegerValueView;
            int newvalue1 = (int)newValue;
            int oldvalue1 = (int)oldValue;
            instance?.RaiseEvents(newvalue1, oldvalue1);
        }

        private void RaiseEvents(int newvalue, int oldvalue)
        {
            if (ValueChanges is Action<int, int>)
            {
                ValueChanges(newvalue, oldvalue);
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