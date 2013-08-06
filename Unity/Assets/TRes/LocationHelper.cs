/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
using UnityEngine;
using System.IO;

namespace Tuner.Resource
{
		public class LocationHelper
		{
				public const string FilePrefix = "file://";
				public const string HttpPrefix = "http://";
				static INetPathHolder mNetPathHolder = new TResAdapter ();

				static public void Init (INetPathHolder holder)
				{
						mNetPathHolder = holder;
				}

				static public string GetNetPath ()
				{
						return mNetPathHolder.GetNetPath ();
				}

				static public string GetBundlePath ()
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

				static string GetBundlePath_Editor ()
				{
						//Application.dataPath = Unity/Assets;
						string unityPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
						string cachePath = unityPath + "/bundles";
						return cachePath;
				}

				static string GetCachePath_Editor ()
				{
						//Application.dataPath = Unity/Assets;
						string unityPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
						string cachePath = unityPath + "/caches";
						return cachePath;
				}

				static string GetBundlePath_Mac ()
				{
						//Application.dataPath = Content;
						string bundlePath = Application.dataPath + "/bundles";
						return bundlePath;
				}

				static string GetCachePath_Mac ()
				{
						//Application.dataPath = Content;
						string cachePath = Application.dataPath + "/caches";
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

				public static string CreateDirectory (string path)
				{
			
						if (System.IO.Directory.Exists (path) == false) {
								UnityEngine.Debug.Log ("Create:" + path);
								System.IO.Directory.CreateDirectory (path);
								//AssetDatabase.Refresh ();
						}
						return path;
				}


				
		}
}