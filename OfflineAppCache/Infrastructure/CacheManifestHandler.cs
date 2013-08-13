using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace OfflineAppCache.Infrastructure
{
    public class CacheManifestHandler : IHttpHandler
    {
        private UrlHelper _urlHelper;

        public void ProcessRequest(HttpContext context)
        {
            _urlHelper = new UrlHelper(context.Request.RequestContext);

            //don't let the browser/proxies cache the manifest using traditional caching methods.
            SetCacheability(context);

            //set the correct MIME type for the manifest
            context.Response.ContentType = "text/cache-manifest";

            //manifest requires this on first line
            AddManifestHeaderText(context);

            AddCacheSection(context);
            
            AddNetworkSection(context);
        }

        private static void AddNetworkSection(HttpContext context)
        {
            //write out the assets that MUST be used online 
            //asterisk(*) means everything that is not already referenced to be cached
            context.Response.Write("NETWORK:" + Environment.NewLine);
            context.Response.Write("*" + Environment.NewLine);
        }

        private void AddCacheSection(HttpContext context)
        {
            //write out the assets that MUST be cached
            context.Response.Write("CACHE:" + Environment.NewLine);
            //write out the links in specified directories
            AddFolderContents(context);
            AddKnownPaths(context);
        }

        private void AddKnownPaths(HttpContext context)
        {
            var actionLink = _urlHelper.Action("ThatOther", "Home");
            if (!string.IsNullOrWhiteSpace(actionLink)) {
                context.Response.Write(actionLink + Environment.NewLine);
            }
        }

        private static void AddManifestHeaderText(HttpContext context)
        {
            context.Response.Write("CACHE MANIFEST" + Environment.NewLine);
            context.Response.Write("#Version: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + Environment.NewLine);
        }

        private static void SetCacheability(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetExpires(DateTime.MinValue);
        }

        private void AddFolderContents(HttpContext context)
        {
            var virtualFolderPaths = new List<string>
                {
                    "~/Content",
                    "~/Scripts"
                };
            foreach (var url in virtualFolderPaths
                .SelectMany(path => GetRelativePathsToRoot(context, path)))
            {
                context.Response.Write(url + Environment.NewLine);
            }
        }

        private IEnumerable<string> GetRelativePathsToRoot(HttpContext context, string virtualPath)
        {
            var physicalPath = context.Server.MapPath(virtualPath);
            var absolutePaths = Directory.EnumerateFiles(
                physicalPath,
                "*.*",
                SearchOption.AllDirectories
            );
            return absolutePaths.Select(
                x => _urlHelper.Content(virtualPath + x.Replace(physicalPath, ""))
            );
        }

        public bool IsReusable { get { return false; } }
    }
}