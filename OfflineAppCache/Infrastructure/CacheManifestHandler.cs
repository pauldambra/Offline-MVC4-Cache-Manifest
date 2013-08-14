using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfflineAppCache.Infrastructure
{
    public class CacheManifestHandler : IHttpHandler, IAssetListener
    {
        private UrlHelper _urlHelper;
        private static long _lastChangeTimeStamp;

        private static readonly List<string> VirtualFolderPaths = new List<string>
            {
                "~/Content",
                "~/Scripts"
            };

        private static AssetMonitor _assetMonitor;

        public void ProcessRequest(HttpContext context)
        {
            _urlHelper = new UrlHelper(context.Request.RequestContext);

            InitialiseAssetMonitor(context);

            //don't let the browser/proxies cache the manifest using traditional caching methods.
            SetCacheability(context);

            //set the correct MIME type for the manifest
            context.Response.ContentType = "text/cache-manifest";

            //manifest requires this on first line
            AddManifestHeaderText(context);

            AddCacheSection(context);
            
            AddNetworkSection(context);
        }

        private void InitialiseAssetMonitor(HttpContext context)
        {
            if (_assetMonitor != null)
            {
                return;
            }
            var physicalServerPaths = new List<string>();
            physicalServerPaths.AddRange(VirtualFolderPaths
                .Select(virtualPath => context.Server.MapPath(virtualPath)));
            _assetMonitor = new AssetMonitor(physicalServerPaths, this);
            _lastChangeTimeStamp = DateTime.Now.Ticks;
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
            context.Response.Write("#Version: " + _lastChangeTimeStamp + Environment.NewLine);
        }

        private static void SetCacheability(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetExpires(DateTime.MinValue);
        }

        private void AddFolderContents(HttpContext context)
        {
            foreach (var url in VirtualFolderPaths
                .SelectMany(path => GetRelativePathsToRoot(context, path)))
                context.Response.Write(url + Environment.NewLine);
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
        
        public void OnDirectoryChange()
        {
            _lastChangeTimeStamp = DateTime.Now.Ticks;
        }
    }
}