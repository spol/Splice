Splice
===

A Plex-compatible media server for Windows. Splice is released under the GPL license and the code is available from https://github.com/spol/Splice. Contributions are welcome and encouraged.

Current state
---
Splice is currently in a pre-alpha state that I'll call Proof-of-Concept.

So far it allows basic adding, browsing and playback of TV shows. Splice consists of two parts:

1) The Splice Server application - currently this is a console application, but will be converted to a Windows service before the first beta release. New content scans are only performed on start up right now, so picking up new videos requires a restart.

2) The Splice Manager GUI - A Windows GUI app for managing the collections. Currently this only supports the adding and deleting of TV show collections. Currently this must be run on the same machine as the service, but the plan is to allow this to be run from another Windows machine to administer a remote Splice Server.

Bug Reports
---
Bugs can be reported via Github or by email to bugs@spliceserver.com.

Known Issues
---
* Collection artwork doesn't work.
* View modes don't persist

External Dependencies
---

Splice requires the .NET 3.5 Framework installed. In addition, the following third-party libraries are used:

* TvdbLib: http://code.google.com/p/tvdblib/
* ZeroConfService: http://code.google.com/p/zeroconfignetservices/
* MediaInfo: http://mediainfo.sourceforge.net/en
* KrystalWare UploadHelper: http://aspnetupload.com/Download-Source.aspx

Release History
---
2011-02-13 : PoC 1 - First release.