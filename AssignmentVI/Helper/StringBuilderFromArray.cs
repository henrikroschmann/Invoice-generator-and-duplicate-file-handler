using System.Reflection;
using System.Text;

namespace AssignmentVI.Helper
{
    internal static class StringBuilderFromArray
    {
        /// <summary>
        /// Helper class to make the customer/ company data into a multiline string from object.
        /// To make it fit for both Company and Customer anon object was used instead.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>string</returns>
        internal static string ArrayToMultiLineString(object item)
        {
            var arrayToString = new StringBuilder();
            for (var index = 0;
                index < item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Length;
                index++)
            {
                var prop = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)[index];
                arrayToString.Append($"{prop.GetValue(item, null)}\n");
            }

            return arrayToString.ToString();
        }
    }
}