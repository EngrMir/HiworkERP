using HiWork.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HiWork.BLL.ServiceHelper
{
    public class ModelBinder
    {
        /* The method provokes value to current cultural properties using generic and reflection */
        public static T SetCulturalValue<T>(T model, CommonModelHelper helper, List<string> items)
        {
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            items.ForEach(item =>
            {
                var propVal = helper.CulturalItem.GetType().GetProperty(item).GetValue(helper.CulturalItem, null);
                var prop = props.FirstOrDefault(x => x.Name.ToLower() == ($"{item}_{helper.Culture}").ToLower());
                if (prop != null)
                {
                    prop.SetValue(model, propVal, null);
                }
            });
            return model;
        }

        public static T ModifyGuidValue<T>(T model)
        {
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType.FullName.ToLower().Contains("system.guid")).ToList();
            props.ForEach(item =>
            {
                var val = item.GetValue(model, null);
                if (val != null && val.Equals(Guid.Empty))
                {
                    item.SetValue(model, null, null);
                }
            });
            return model;
        }
    }
}