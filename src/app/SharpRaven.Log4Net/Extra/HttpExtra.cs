using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace SharpRaven.Log4Net.Extra
{
    public class HttpExtra
    {
        private static readonly dynamic httpContext;


        static HttpExtra()
        {
            httpContext = GetHttpContext();
        }


        public object Request
        {
            get
            {
                return new
                {
                    ServerVariables = Convert(x => x.Request.ServerVariables)
                };
            }
        }


        private static dynamic GetHttpContext()
        {
            var systemWeb = AppDomain.CurrentDomain
                                     .GetAssemblies()
                                     .FirstOrDefault(assembly => assembly.FullName.StartsWith("System.Web"));

            if (systemWeb == null)
                return null;

            var httpContextType = systemWeb.GetExportedTypes()
                                           .FirstOrDefault(type => type.Name == "HttpContext");

            if (httpContextType == null)
                return null;

            var currentHttpContextProperty = httpContextType.GetProperty("Current",
                                                                         BindingFlags.Static | BindingFlags.Public);

            if (currentHttpContextProperty == null)
                return null;

            return currentHttpContextProperty.GetValue(null, null);
        }


        private static IDictionary<string, string> Convert(Func<dynamic, NameValueCollection> collectionGetter)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                NameValueCollection collection = collectionGetter.Invoke(httpContext);
                var keys = collection.AllKeys.ToArray();

                foreach (var key in keys)
                {
                    var value = collection[key];
                    dictionary.Add(key, value);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return dictionary;
        }
    }
}