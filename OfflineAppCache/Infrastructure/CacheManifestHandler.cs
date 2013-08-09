using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Optimization;

namespace OfflineAppCache.Infrastructure
{
    public class CacheManifestHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //don't let the browser/proxies cache the manifest using traditional caching methods.
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetExpires(DateTime.MinValue);

            //set the correct MIME type for the manifest
            context.Response.ContentType = "text/cache-manifest";

            //manifest requires this on first line
            context.Response.Write("CACHE MANIFEST" + Environment.NewLine);
            context.Response.Write("#Version: " + DateTime.Now.ToLongDateString());

            //write out the assets that MUST be cached
            context.Response.Write("CACHE:" + Environment.NewLine);

            //write out the links in the bundles
            WriteBundle(context, "~/bundles/jquery");
            WriteBundle(context, "~/bundles/handlebars");
            WriteBundle(context, "~/bundles/app");
            WriteBundle(context, "~/Content/css");

            //add other assets not mentioned in the bundles
//            context.Response.Write(Scripts.Url("~/Images/image1.png") + Environment.NewLine);
//            context.Response.Write(Scripts.Url("~/Images/image2.png") + Environment.NewLine);

            //write out the assets that MUST be used online 
            //asterisk(*) means everything that is not already referenced to be cached
            context.Response.Write("NETWORK:" + Environment.NewLine);
            context.Response.Write("*" + Environment.NewLine);
        }


        private static void WriteBundle(HttpContext context, string virtualPath)
        {
                foreach (var contentVirtualPath in 
                    BundleResolver.Current.GetBundleContents(virtualPath))
                {
                    context.Response.Write(Scripts.Url(contentVirtualPath) + 
                        Environment.NewLine);
                }
        }

        public bool IsReusable { get { return false; } }
    }
}