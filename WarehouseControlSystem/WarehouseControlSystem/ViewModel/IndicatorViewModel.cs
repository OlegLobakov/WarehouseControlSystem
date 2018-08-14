using System;
using System.Collections.Generic;
using System.Text;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;
using WarehouseControlSystem.Model.NAV;
using System.Windows.Input;

namespace WarehouseControlSystem.ViewModel
{
    public class IndicatorViewModel :  BaseViewModel
    {
        public string Header
        {
            get { return header; }
            set
            {
                if (header != value)
                {
                    header = value;
                    OnPropertyChanged(nameof(Header));
                }
            }
        } string header;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        } string description;
        public Color ValueColor
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        } Color color;
        public string Value
        {
            get { return value1; }
            set
            {
                if (value1 != value)
                {
                    value1 = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        } string value1;
        public int Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        } int position;
        public bool IsURLExist
        {
            get { return isurlexist; }
            set
            {
                if (isurlexist != value)
                {
                    isurlexist = value;
                    OnPropertyChanged(nameof(IsURLExist));
                }
            }
        } bool isurlexist;
        public string URL
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged(nameof(URL));
                }
            }
        } string url;

        public ICommand TapCommand { protected set; get; }
        public event Action<IndicatorViewModel> OnTap;

        public IndicatorViewModel(INavigation navigation, Indicator indicator) : base(navigation)
        {
            FillFields(indicator);
            TapCommand = new Command<object>(Tap);
        }

        public void FillFields(Indicator indicator)
        {
            Header = indicator.Header;
            Description = indicator.Description;
            Value = indicator.Value;
            ValueColor = Color.FromHex(indicator.ValueColor);
            URL = indicator.URL;
            IsURLExist = !string.IsNullOrEmpty(indicator.URL);
        }

        public void Tap(object sender)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                try
                {
                    Uri uri = new Uri(URL);
                    Device.OpenUri(uri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                if (OnTap is Action<LocationViewModel>)
                {
                    OnTap(this);
                }
            }
        }
    }
}
