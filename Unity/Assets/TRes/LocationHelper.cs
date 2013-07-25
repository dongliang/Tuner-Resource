
using UnityEngine;
using System.IO;

namespace Tuner.Resource
{
		public class LocationHelper
		{
				public const string FilePrefix = "file://";
				public const string HttpPrefix = "http://";

				public static string GetBundlePath ()
				{
						string bundlePath = GetBundlePath_Editor ();
						switch (Application.platform) {
						case RuntimePlatform.OSXPlayer:
								bundlePath = GetBundlePath_Mac ();
								break;
						case RuntimePlatform.IPhonePlayer:
								bundlePath = GetBundlePath_IOS ();
								break;
						}
						return bundlePath;
				}
				
				/// <summary>
				/// Gets the file URL. first check cache path. if file not exist, use the bundle path.
				/// </summary>
				/// <returns>The file URL.</returns>
				/// <param name="relativePath">Relative path.the first character is '/'</param>
				public static string GetLoadFileURL (string relativePath)
				{		
						string loadFilePath = GetCachePath () + relativePath;

						//check if exsit.
						if (!File.Exists (loadFilePath)) {
								loadFilePath = GetBundlePath () + relativePath;
						}
						return FilePrefix + loadFilePath;
				}

				public static string GetCachePath ()
				{
						string cachePath = GetCachePath_Editor ();
						switch (Application.platform) {
						case RuntimePlatform.OSXPlayer:
								cachePath = GetCachePath_Mac ();
								break;
						case RuntimePlatform.IPhonePlayer:
								cachePath = GetCachePath_IOS ();
								break;
						}
						return cachePath;
				}

				static string GetBundlePath_Editor ()
				{
						//Application.dataPath = Unity/Assets;
						string unityPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
						string cachePath = unityPath + "/Bundles";
						return cachePath;
				}

				static string GetCachePath_Editor ()
				{
						//Application.dataPath = Unity/Assets;
						string unityPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
						string cachePath = unityPath + "/Caches";
						return cachePath;
				}

				static string GetBundlePath_Mac ()
				{
						//Application.dataPath = Content;
						string bundlePath = Application.dataPath + "/Bundles";
						return bundlePath;
				}

				static string GetCachePath_Mac ()
				{
						//Application.dataPath = Content;
						string cachePath = Application.dataPath + "/Caches";
						return cachePath;
				}

				static string GetBundlePath_IOS ()
				{
						//Application.dataPath = /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data
						return Application.dataPath.Substring (0, Application.dataPath.Length - 5);
				}

				static string GetCachePath_IOS ()
				{
						//Application.dataPath = /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data
						string bundlePath = GetBundlePath_IOS ();
						string cachePath = bundlePath.Substring (0, bundlePath.LastIndexOf ('/')) + "/Library/Caches";
						return cachePath;
				}


				
		}
}