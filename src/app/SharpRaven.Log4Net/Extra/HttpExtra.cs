using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace SharpRaven.Log4Net.Extra
{
    public class HttpExtra
    {
        private readonly dynamic httpContext;

        private HttpExtra(dynamic httpContext)
        {
            this.httpContext = httpContext;
            Request = GetRequest();
            Response = GetResponse();
        }


        public static HttpExtra GetHttpExtra()
        {
            var context = GetHttpContext();
            if (context == null)
                return null;

            return new HttpExtra(context);
        }

        public object Request { get; private set; }
        public object Response { get; private set; }


        private object GetResponse()
        {
            try
            {
                return new
                {
                    Cookies = Convert(x => x.Response.Cookies),
                    Headers = Convert(x => x.Response.Headers),
                    ContentEncoding = this.httpContext.Response.ContentEncoding.HeaderName,
                    HeaderEncoding = this.httpContext.Response.HeaderEncoding.HeaderName,
                    this.httpContext.Response.ContentType,
                    this.httpContext.Response.Charset,
                    this.httpContext.Response.Expires,
                    this.httpContext.Response.ExpiresAbsolute,
                    this.httpContext.Response.IsClientConnected,
                    this.httpContext.Response.IsRequestBeingRedirected,
                    this.httpContext.Response.RedirectLocation,
                    this.httpContext.Response.SuppressContent,
                    this.httpContext.Response.TrySkipIisCustomErrors,
                    Status = new
                    {
                        this.httpContext.Response.Status,
                        Code = this.httpContext.Response.StatusCode,
                        Description = this.httpContext.Response.StatusDescription,
                        SubCode = this.httpContext.Response.SubStatusCode,
                    }
                };
            }
            catch (Exception exception)
            {
                return new
                {
                    Exception = exception
                };
            }
        }


        private object GetRequest()
        {
            try
            {
                return new
                {
                    ServerVariables = Convert(x => x.Request.ServerVariables),
                    Form = Convert(x => x.Request.Form),
                    Cookies = Convert(x => x.Request.Cookies),
                    Headers = Convert(x => x.Request.Headers),
                    //Params = Convert(x => x.Request.Params),
                    ContentEncoding = this.httpContext.Request.ContentEncoding.HeaderName,
                    this.httpContext.Request.AcceptTypes,
                    this.httpContext.Request.ApplicationPath,
                    this.httpContext.Request.ContentType,
                    this.httpContext.Request.CurrentExecutionFilePath,
                    this.httpContext.Request.CurrentExecutionFilePathExtension,
                    this.httpContext.Request.FilePath,
                    this.httpContext.Request.HttpMethod,
                    this.httpContext.Request.IsAuthenticated,
                    this.httpContext.Request.IsLocal,
                    this.httpContext.Request.IsSecureConnection,
                    this.httpContext.Request.Path,
                    this.httpContext.Request.PathInfo,
                    this.httpContext.Request.PhysicalApplicationPath,
                    this.httpContext.Request.PhysicalPath,
                    this.httpContext.Request.QueryString,
                    this.httpContext.Request.RawUrl,
                    this.httpContext.Request.TotalBytes,
                    this.httpContext.Request.Url,
                    this.httpContext.Request.UserAgent,
                    User = new
                    {
                        Languages = this.httpContext.Request.UserLanguages,
                        Host = new
                        {
                            Address = this.httpContext.Request.UserHostAddress,
                            Name = this.httpContext.Request.UserHostName,
                        }
                    }
                };
            }
            catch (Exception exception)
            {
                return new
                {
                    Exception = exception
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


        private IDictionary<string, string> Convert(Func<dynamic, NameValueCollection> collectionGetter)
        {
            if (this.httpContext == null)
                return null;

            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                NameValueCollection collection = collectionGetter.Invoke(this.httpContext);
                var keys = collection.AllKeys.ToArray();

                foreach (var key in keys)
                {
                    // NOTE: Ignore these keys as they just add duplicate information. [asbjornu]
                    if (key == "ALL_HTTP" || key == "ALL_RAW")
                        continue;

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
