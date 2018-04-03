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
using WarehouseControlSystem.Helpers.Containers.StateContainer;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading;
using WarehouseControlSystem.Model;

namespace WarehouseControlSystem.ViewModel.Base
{
    public class BaseViewModel : BindableObject, IDisposable
    {
        public INavigation Navigation { get; set; }

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        } string title;
        public State State
        {
            get { return state1; }
            set
            {
                if (state1 != value)
                {
                    state1 = value;
                    LoadAnimation = state1 == State.Loading;
                    OnPropertyChanged(nameof(State));
                }
            }
        } State state1;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        } bool selected;
        public string LoadingText
        {
            get { return loadingtext; }
            set
            {
                if (loadingtext != value)
                {
                    loadingtext = value;
                    OnPropertyChanged(nameof(LoadingText));
                }
            }
        } string loadingtext;
        public bool LoadAnimation
        {
            get { return loadanimation; }
            set
            {
                if (loadanimation != value)
                {
                    loadanimation = value;
                    OnPropertyChanged(nameof(LoadAnimation));
                }
            }
        } bool loadanimation;
        public string InfoText
        {
            get { return infotext; }
            set
            {
                if (infotext != value)
                {
                    infotext = value;
                    OnPropertyChanged(nameof(InfoText));
                }
            }
        } string infotext;
        public string ErrorText
        {
            get { return errortext; }
            set
            {
                if (errortext != value)
                {
                    errortext = value;
                    OnPropertyChanged(nameof(ErrorText));
                }
            }
        } string errortext;
        public string RequestLabelText
        {
            get { return requestlabeltext; }
            set
            {
                if (requestlabeltext != value)
                {
                    requestlabeltext = value;
                    OnPropertyChanged(nameof(RequestLabelText));
                }
            }
        } string requestlabeltext;
        public string RequestMessageText
        {
            get { return requesmessagettext; }
            set
            {
                if (requesmessagettext != value)
                {
                    requesmessagettext = value;
                    OnPropertyChanged(nameof(RequestMessageText));
                }
            }
        } string requesmessagettext;

        public ICommand ErrorOKCommand { protected set; get; }
        public ICommand WarningOKCommand { protected set; get; }
        public ICommand NoInternetOKCommand { protected set; get; }
        public ICommand NoDataOKCommand { protected set; get; }

        public ICommand OKCommand { protected set; get; }
        public ICommand CancelCommand { protected set; get; }
        public ICommand CancelChangesCommand { protected set; get; }

        public bool IsDisposed = false;

        public AsyncCancelationDispatcher ACD;

        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ErrorOKCommand = new Command(ErrorOk);
            WarningOKCommand = new Command(WarningOk);
            NoInternetOKCommand = new Command(NoInternetOK);
            NoDataOKCommand = new Command(NoDataOK);
            ACD = new AsyncCancelationDispatcher();
        }

        public virtual void ErrorOk()
        {
            State = State.Normal;
        }

        public virtual void WarningOk()
        {
            State = State.Normal;
        }

        public virtual void NoInternetOK()
        {
            State = State.Normal;
        }

        public virtual void NoDataOK()
        {
            State = State.Normal;
        }

        public virtual void Dispose()
        {
            ACD.CancelAll();
            Navigation = null;
            ErrorOKCommand = null;
            WarningOKCommand = null;
            NoInternetOKCommand = null;
            NoDataOKCommand = null;

            IsDisposed = true;
        }

        public string ColorToHex(Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            return String.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
        }

        public string ColorToHexAlfa(Color color)
        {
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            int alpha = (int)(color.A * 255);
            return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", red, green, blue, alpha);
        }
    }
}
