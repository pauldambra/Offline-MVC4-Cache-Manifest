using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OfflineAppCache.Infrastructure
{
    public interface IAssetListener
    {
        void OnDirectoryChange();
    }

    public class AssetMonitor
    {
        private readonly IAssetListener _assetListener;
        private readonly List<FileSystemWatcher> _fileSystemWatchers = new List<FileSystemWatcher>();

        public AssetMonitor(IEnumerable<string> pathsToMonitor, IAssetListener assetListener)
        {
            _assetListener = assetListener;
            foreach (var path in pathsToMonitor)
            {
                _fileSystemWatchers.Add(CreateFileWatcher(path));
            }
        }


        public FileSystemWatcher CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            var watcher = new FileSystemWatcher
                {
                    Path = path,
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true
                };
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            // Only watch text files.

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            return watcher;
        }
        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _assetListener.OnDirectoryChange();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            _assetListener.OnDirectoryChange();
        }
    }
}