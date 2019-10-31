using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
namespace YiZhan.Common.YZExtensions
{
    public static class EumHelper
    {
        public static string GetDescription(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<AssemblyDescriptionAttribute>()?
                .Description;
        }
    }
}