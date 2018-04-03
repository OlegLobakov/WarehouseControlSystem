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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using WarehouseControlSystem.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer_Droid))]
namespace WarehouseControlSystem.Droid
{
    public class CustomEntryRenderer_Droid : EntryRenderer
    {
        public CustomEntryRenderer_Droid() : base(Android.App.Application.Context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control?.SetBackgroundColor(Android.Graphics.Color.ParseColor("#e3e3e3"));
            Control?.SetTextColor(Android.Graphics.Color.Black);

            if (Control != null)
            {
                //Control.SetPadding(0, Control.PaddingTop, 0, Control.PaddingBottom);
                Control.SetPadding(0, 0, 0, 0);
                Control.Gravity = GravityFlags.CenterVertical;
            }
        }
    }
}