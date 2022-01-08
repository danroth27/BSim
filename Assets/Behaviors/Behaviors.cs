using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public static class Behaviors
    {
        public static IEnumerable<Type> GetBehaviorTypes() =>
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && typeof(IBehavior).IsAssignableFrom(t));


        public static IEnumerable<PropertyInfo> GetBehaviorProperties(IBehavior behavior)
        {
            var behaviorType = behavior.GetType();
            if (!typeof(IBehavior).IsAssignableFrom(behaviorType))
            {
                throw new InvalidOperationException("Type is not a behavior");
            }

            return behaviorType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
        }

        public static string ToFriendlyName(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var res = new StringBuilder();

            res.Append(str[0]);
            for (var i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    res.Append(' ');
                }
                res.Append(str[i]);

            }
            return res.ToString();
        }

    }
}
