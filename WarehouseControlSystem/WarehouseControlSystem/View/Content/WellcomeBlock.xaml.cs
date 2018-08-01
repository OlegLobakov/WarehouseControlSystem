// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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
using System.Windows.Input;

namespace WarehouseControlSystem.View.Content
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WellcomeBlock : ContentView
	{
        public event Action Taped; 

        public ICommand TapCommand { get; }

        public static readonly BindableProperty FileImageSourceProperty = BindableProperty.Create(nameof(FileImageSource), typeof(FileImageSource), typeof(WellcomeBlock), null, BindingMode.Default, null, ImageChanged);
        public FileImageSource FileImageSource
        {
            get { return (FileImageSource)GetValue(FileImageSourceProperty); }
            set { SetValue(FileImageSourceProperty, value); }
        }

        private static void ImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //var instance = bindable as WellcomeBlock;
            //instance?.Update();
        }

        public static readonly BindableProperty FileImagePathProperty = BindableProperty.Create("FileImagePath", typeof(string), typeof(WellcomeBlock), "");
        public string FileImagePath
        {
            get { return (string)GetValue(FileImagePathProperty); }
            set { SetValue(FileImagePathProperty, value); }
        }

        public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(WellcomeBlock), "");
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public WellcomeBlock()
		{
            TapCommand = new Command(OnTapped);
            InitializeComponent ();
            BindingContext = this;

            TapGestureRecognizer tc = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    OnTapped();
                })
            };

            GestureRecognizers.Add(tc);
        }

        private void OnTapped()
        {            
            if (Taped is Action)
            {         
                Taped();
            }
        }
    }
}