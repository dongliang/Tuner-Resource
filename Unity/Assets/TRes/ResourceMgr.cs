using System.Collections.Generic;
using UnityEngine;

namespace Tuner.Resource
{		
		public class ResourceMgr:Singleton<ResourceMgr>
		{
				//list
				private Dictionary<string,Resource> mWaiting = new Dictionary<string, Resource> ();
				private Dictionary<string,Resource> mDownloading = new Dictionary<string, Resource> ();
				private Dictionary<string,Resource> mCached = new Dictionary<string, Resource> ();
				private Dictionary<string,WWW> mDownloadingWWW = new Dictionary<string, WWW> ();
				private const int maxProcessors = 1;
				private List<string> mRemoveList = new List<string> ();
				
				///remove resource at any time.
				public void Remove (string url)
				{
						mRemoveList.Add (url);
				}
				/// Add a resource load request, it will call the callback at the resource load complete.or error.
				public void AddRequest (string url, Resource resource, ResourceCallback completeCallback)
				{
						//check param
						if (string.IsNullOrEmpty (url)) {
								if (completeCallback != null) {
										completeCallback.Invoke (null, true, "url is null");
								}
								return;
						}
						if (resource == null) {
								if (completeCallback != null) {
										completeCallback.Invoke (null, true, "Resource object is null");
								}
								return;
						}
						
						//find resource.
						Resource temp = find (url);
						
						if (temp == null) {
								resource.mUrl = url;
								temp = resource;
								mWaiting.Add (url, temp);
						}
						
						//add callback
						temp.addCallback (completeCallback);

						//notify cached.
						if (temp.mState == E_Resource_State.done) {
								temp.Notify (false, "Resource download Success.");
						}
						
				}
				
				public void Update ()
				{
						process ();
				}

				private void process ()
				{
						process_AutoStart ();
						process_CheckDownloading ();
						process_Remove ();
				}

				private void process_AutoStart ()
				{
						if (mDownloadingWWW.Count < maxProcessors) {
								Resource temp = GetFirstWait ();
								if (temp != null) {
										Start (temp);
								}
						}
				}

				private void process_CheckDownloading ()
				{
						List<string> deleteList = null;
						//check downloading
						foreach (KeyValuePair<string, WWW> downloading_item in mDownloadingWWW) {
				
								//check error
								if (downloading_item.Value.error != null) {
					
										Resource res = find (downloading_item.Value.url);
										if (res != null) {
												mDownloading.Remove (res.mUrl);
												res.Notify (true, downloading_item.Value.error);
										}
										if (deleteList == null) {
												deleteList = new List<string> ();
										}
										deleteList.Add (downloading_item.Key);
					
								}
								//check done
								else if (downloading_item.Value.isDone) {
					
										Resource res = find (downloading_item.Value.url);
										if (res != null) {
												mDownloading.Remove (res.mUrl);
												res.OnDownloadComplete (downloading_item.Value);
												mCached.Add (res.mUrl, res);
												res.Notify (false, "Resource download Success.");
										}
					
										if (deleteList == null) {
												deleteList = new List<string> ();
										}
										deleteList.Add (downloading_item.Key);
								}
								//delete WWW
								foreach (string delete_Item in deleteList) {
										mDownloadingWWW [delete_Item].Dispose ();
										mDownloadingWWW.Remove (delete_Item);
								}
						}
				}

				private void process_Remove ()
				{
						foreach (string item in mRemoveList) {
								_Remove (item);
						}
						mRemoveList.Clear ();
				}
		
				private void Start (Resource resource)
				{
						string url = resource.mUrl;
						mWaiting.Remove (url);
						mDownloading.Add (url, resource);
						WWW temp = new WWW (url);
						mDownloadingWWW.Add (url, temp);
						resource.mState = E_Resource_State.downloading;
				}

				private Resource GetFirstWait ()
				{
						Resource temp = null;
						foreach (KeyValuePair<string,Resource> item in mWaiting) {
								temp = item.Value;	
								break;
						}
						return temp;
				}

				private void _Remove (string url)
				{
						Resource temp = find (url);
			
						if (temp != null) {
								switch (temp.mState) {
								case E_Resource_State.waiting:
										mWaiting.Remove (url);
										break;
								case E_Resource_State.downloading:
										WWW tempWWW = null;
										if (mDownloadingWWW.TryGetValue (url, out tempWWW)) {
												tempWWW.Dispose ();
												mDownloadingWWW.Remove (url);
										}	
										break;
								case E_Resource_State.done:
										temp.Relase ();
										mCached.Remove (url);
										break;
								}
						}
				}
		
				private Resource find (string url)
				{
						Resource temp = null;
						mWaiting.TryGetValue (url, out temp);
						if (temp != null) {
								return temp;
						}
						mDownloading.TryGetValue (url, out temp);
						if (temp != null) {
								return temp;
						}
						mCached.TryGetValue (url, out temp);
						if (temp != null) {
								return temp;
						}
						return null;
				}
				
		}
}