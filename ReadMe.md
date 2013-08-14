Dynamic MVC4 Offline Cache Demo
====
After visiting [Manchester JS usergroup](http://mancjs.com/#offline-apps-node-and-coffeescript) recently and watching a talk on offline web sites I wondered how easy it was to make an offline capable MVC4 website and have the cache manifest dynamically generated.

[There followed some giddy research ](hhttp://storify.com/pauldambra/giddy-cache-manifest-research) which I've gathered at Storify.

And based on the pointers in the links contained in that research I came up with this "ground-breaking" work.

An MVC4 site which builds a cache.manifest file containing all the files in the Content and Scripts folder (because they're where static files tend to go in MVC projects)

The application uses [the FileSystemWatcher class](http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx) to change the Cache.Manifest only when a monitored asset folder changes.

Feedback more than welcome!

Gotchas
----
###BundleConfig

NB, the default MVC4 BundleConfig adds "site.css" to the css bundle while the file is "Site.css" I could visit either cased URL in the browser successfully but the browser couldn't hit the lowercase version of the URL when offline so it is necessary to edit the BundleConfig to correct the case of Site.css

so

`bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));`

becomes

`bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));`


To-do
----
* Add notification of status on client and prompt user to reload when new assets are available

