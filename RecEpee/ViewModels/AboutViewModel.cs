using RecEpee.Properties;
using System;
using System.Drawing;
using System.Reflection;

namespace RecEpee.ViewModels
{
    class AboutViewModel
    {
        static AboutViewModel()
        {
            var assembly = typeof(AboutViewModel).Assembly;

            Product = GetAttributeValue<AssemblyProductAttribute>(assembly, a => a.Product);
            Copyright = GetAttributeValue<AssemblyCopyrightAttribute>(assembly, a => a.Copyright);
            Description = GetAttributeValue<AssemblyDescriptionAttribute>(assembly, a => a.Description);
            //Version = GetAttributeValue<AssemblyFileVersionAttribute>(assembly, a => a.Version);
            Version = Info.Version;
        }

        public static string Product { get; private set; }
        public static string Copyright { get; private set; }
        public static string Description { get; private set; }
        public static string Version { get; private set; }

        public static string Logo
        {
            get
            {
                return @"../Images/icon.png";
            }
        }

        public static string Title
        {
            get
            {
                return "About " + Product;
            }
        }

        private static string GetAttributeValue<AttrType>(Assembly assembly, Func<AttrType, string> property) where AttrType : class
        {
            var attributes = assembly.GetCustomAttributes(typeof(AttrType), false);

            if (attributes.Length > 0)
            {
                var attribute = (AttrType)attributes[0];

                return property(attribute);
            }

            return "";
        }
    }
}
