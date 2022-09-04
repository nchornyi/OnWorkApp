using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

namespace OnWork.Droid
{
    public sealed class AccessNativeBitmapConfig : IBitmapDescriptorFactory
    {
        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            int resId = 0;
            switch (descriptor.Id)
            {
                //case "type2":
                //    resId = Resource.Drawable.w;
                //    break;
                case "base":
                    resId = Resource.Drawable.worklocation;
                    break;

                case "pin":
                    resId = Resource.Drawable.pin;
                    break;

            }

            return AndroidBitmapDescriptorFactory.FromResource(resId);
        }
    }
}