/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
namespace Tuner.Resource
{

		//ResourceMgr.Init  LocationHelper.Init  Updater.Init
		class TResRoot:Singleton<TResRoot>
		{
				
				//common
				public void Init (TResAdapter adapter)
				{
						ResourceMgr.Instance.Init (adapter);
						LocationHelper.Init (adapter);
				}

				public void Update ()
				{
						ResourceMgr.Instance.Update ();
				}

				//resource
				public void AddRequest (string url, string resourceType, ResourceCallback completeCallback)
				{
						ResourceMgr.Instance.AddRequest (url, resourceType, completeCallback);
				}
				
				public void Remove (string url)
				{
						ResourceMgr.Instance.Remove (url);
				}

				public float Progress (string url)
				{
						return ResourceMgr.Instance.Progress (url);
				}

				//path
				public string GetNetPath ()
				{
						return LocationHelper.GetNetPath ();
				}

				public string GetBundlePath ()
				{
						return LocationHelper.GetBundlePath ();
				}

				public static string GetCachePath ()
				{
						return LocationHelper.GetCachePath ();
				}

				public static string GetLoadFileURL (string relativePath)
				{
						return LocationHelper.GetLoadFileURL (relativePath);
				}

				//update
				public void CheckUpdate (CheckUpdateCallback callback)
				{
						UpdateMgr.Instance.Check (callback);
				}

				public void StartUpdate (ResourceGroupCallback callback)
				{
						UpdateMgr.Instance.Start (callback);
				}
				
				public void RelaseUpdate ()
				{
						UpdateMgr.Instance.Relase ();
				}
				
				//group
				public void addGroup (string groupName, ResourceGroupCallback callback)
				{
						ResourceGroupMgr.Instance.addGroup (groupName, callback);
				}

				public void addRequest (string groupName, string url, string resourceType, ResourceCallback completeCallback)
				{
						ResourceGroupMgr.Instance.addRequest (groupName, url, resourceType, completeCallback);
				}

				public void removeGroup (string groupName)
				{
						ResourceGroupMgr.Instance.removeGroup (groupName);
				}

				public void Start (string groupName)
				{
						ResourceGroupMgr.Instance.Start (groupName);
				}

				public GroupProgress GroupProgress (string groupName)
				{
						return ResourceGroupMgr.Instance.Progress (groupName);
				}
		}
}