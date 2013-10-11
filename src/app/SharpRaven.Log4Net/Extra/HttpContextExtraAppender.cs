using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace SharpRaven.Log4Net.Web
{
    public class HttpContextExtraAppender : ExtraAppender
    {
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


        protected override object Append(object extra)
        {
            var httpContext = GetHttpContext();

            if (httpContext == null)
                return extra;

            try
            {
                var serverVariables = (NameValueCollection)httpContext.Request.ServerVariables;
                var keys = serverVariables.AllKeys.ToArray();

                foreach (string key in keys)
                {
                    var value = serverVariables[key];
                    extra = Append(extra, key, value);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return extra;
        }
    }
}