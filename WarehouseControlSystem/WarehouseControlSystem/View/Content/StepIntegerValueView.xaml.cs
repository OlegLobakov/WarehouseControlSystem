// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
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