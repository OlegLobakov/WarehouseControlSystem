using System;
using System.Collections.Generic;
using System.Text;
using WarehouseControlSystem.Model.NAV;
using WarehouseControlSystem.ViewModel.Base;
using Xamarin.Forms;

namespace WarehouseControlSystem.ViewModel
{
    public class BinInfoViewModel : BaseViewModel
    {
        public string Caption
        {
            get { return caption; }
            set
            {
                if (caption != value)
                {
                    caption = value;
                    OnPropertyChanged(nameof(Caption));
                }
            }
        }
        string caption;

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
        }
        string description;

        public string Detail
        {
            get { return detail; }
            set
            {
                if (detail != value)
                {
                    detail = value;
                    OnPropertyChanged(nameof(Detail));
                }
            }
        }
        string detail;

        public string Value1
        {
            get { return value1; }
            set
            {
                if (value1 != value)
                {
                    value1 = value;
                    OnPropertyChanged(nameof(Value1));
                }
            }
        }
        string value1;

        public string Value2
        {
            get { return value2; }
            set
            {
                if (value2 != value)
                {
                    value2 = value;
                    OnPropertyChanged(nameof(Value2));
                }
            }
        }
        string value2;

        public string Value3
        {
            get { return value3; }
            set
            {
                if (value3 != value)
                {
                    value3 = value;
                    OnPropertyChanged(nameof(Value3));
                }
            }
        }
        string value3;

        public bool ImageIsVisible
        {
            get { return imageisvisible; }
            set
            {
                if (imageisvisible != value)
                {
                    imageisvisible = value;
                    OnPropertyChanged(nameof(ImageIsVisible));
                }
            }
        }
        bool imageisvisible;

        public ImageSource ImageSource
        {
            get { return imagesource; }
            set
            {
                if (imagesource != value)
                {
                    imagesource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }
        ImageSource imagesource;


        public string ImageURL
        {
            get { return imageurl; }
            set
            {
                if (imageurl != value)
                {
                    imageurl = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }
        string imageurl;

        public BinInfoViewModel(BinInfo bi, INavigation navigation) : base(navigation)
        {
            Caption = bi.Caption;
            Description = bi.Description;
            Detail = bi.Detail;
            Value1 = bi.Value1;
            Value2 = bi.Value2;
            value3 = bi.Value3;
            ImageURL = bi.ImageURL;
        }
    }
}
