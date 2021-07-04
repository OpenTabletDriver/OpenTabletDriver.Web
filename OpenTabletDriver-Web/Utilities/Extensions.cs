using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OpenTabletDriver.Web.Utilities
{
    public static class Extensions
    {
        public static ViewDataDictionary<T> With<T>(this ViewDataDictionary<T> viewData, Action<ViewDataDictionary> action)
        {
            var newData = new ViewDataDictionary<T>(viewData);
            action(newData);
            return newData;
        }
    }
}