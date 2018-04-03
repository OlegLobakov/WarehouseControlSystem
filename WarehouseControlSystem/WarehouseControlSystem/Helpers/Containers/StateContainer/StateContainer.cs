using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarehouseControlSystem.Helpers.Containers.StateContainer
{
    [ContentProperty("Conditions")]
    [Preserve(AllMembers = true)]
    public class StateContainer : ContentView
    {
        public event Action<StateContainer> OnAfterStateSet;

        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(object), typeof(StateContainer), null, BindingMode.Default, null, StateChanged);

        public static void Init()
        {
            //for linker
        }

        private static void StateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var parent = bindable as StateContainer;
            //if ((parent != null) && (newValue != null))
            if (parent != null)
            {
                parent.ChooseStateProperty(newValue);

                if (parent.OnAfterStateSet is Action<StateContainer>)
                {
                    parent.OnAfterStateSet(parent);
                }
            }
        }

        public object State
        {
            get { return GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        private void ChooseStateProperty(object newValue)
        {
            try
            {
                if (Conditions == null && Conditions?.Count == 0) return;
                foreach (var stateCondition in Conditions.Where(stateCondition => stateCondition.State != null && stateCondition.State.ToString().Equals(newValue.ToString())))
                {
                    if (Content != null)
                    {
                        Content.IsVisible = false;
                    }
                    Content = stateCondition.Content;
                    Content.IsVisible = true;
                    break;
                }
            }
            catch
            {
            }
        }

        private async Task ChooseStateProperty2(object newValue)
        {
            if (Conditions == null && Conditions?.Count == 0) return;

            try
            {
                foreach (var stateCondition in Conditions.Where(stateCondition => stateCondition.State != null && stateCondition.State.ToString().Equals(newValue.ToString())))
                {
                    if (Content != null)
                    {
                        await Content.FadeTo(0, 100U); 
                        Content.IsVisible = false; 
                        await Task.Delay(50); 
                    }  
                    stateCondition.Content.Opacity = 0;
                    Content = stateCondition.Content;
                    Content.IsVisible = true;
                    await Content.FadeTo(1);

                    break;
                }
            }
            catch
            {
            }
        }
    }
}
