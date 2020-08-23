using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf.NetCore.Options;

namespace Rotativa
{
    public class CustomOptions : IConvertOptions
    {
        [OptionFlag("--header-html")]
        public string HeaderHtml { get; set; }

        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        public string GetConvertOptions()
        {
            var result = new StringBuilder();

            var fields = this.GetType().GetProperties();
            foreach (var fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (of == null)
                    continue;

                object value = fi.GetValue(this, null);
                if (value == null)
                    continue;

                if (fi.PropertyType == typeof(Dictionary<string, string>))
                {
                    var dictionary = (Dictionary<string, string>)value;
                    foreach (var d in dictionary)
                    {
                        result.AppendFormat(" {0} \"{1}\" \"{2}\"", of.Name, d.Key, d.Value);
                    }
                }
                else if (fi.PropertyType == typeof(bool))
                {
                    if ((bool)value)
                        result.AppendFormat(CultureInfo.InvariantCulture, " {0}", of.Name);
                }
                else
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
                }
            }

            return result.ToString().Trim();
        }
    }
}
