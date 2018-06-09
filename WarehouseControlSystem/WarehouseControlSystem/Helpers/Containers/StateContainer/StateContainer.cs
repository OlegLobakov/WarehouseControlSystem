using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(State), typeof(StateContainer), State.Normal, BindingMode.Default, null, StateChanged);

        private static void StateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var parent = bindable as StateContainer;
            if (parent != null)
            {
                if (oldValue != newValue)
                {
                    parent.ChooseStateProperty(newValue);

                    if (parent.OnAfterStateSet is Action<StateContainer>)
                    {
                        parent.OnAfterStateSet(parent);
                    }
                }
            }
        }

        public State State
        {
            get { return (State)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        private void ChooseStateProperty(object newValue)
        {
            try
            {
                if (Conditions == null && Conditions?.Count == 0)
                {
                    return;
                }

                foreach (StateCondition sc in Conditions)
                {
                    if (sc.State.ToString() == newValue.ToString())
                    {
                        if (Content != null)
                        {
                            Content.IsVisible = false;
                        }
                        Content = sc.Content;
                        Content.IsVisible = true;
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
