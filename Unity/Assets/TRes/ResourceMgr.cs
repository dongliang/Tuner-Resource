/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
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
				IResourceBuilder mResourceBuilder = new TResAdapter ();

				public void Init (IResourceBuilder adapter)
				{
						mResourceBuilder = adapter;
				}
				
				///remove resource at any time.
				public void Remove (string url)
				{
						mRemoveList.Add (url);
				}
				/// Add a resource load request, it will call the callback at the resource load complete.or error.
				public void AddRequest (string url, string resourceType, ResourceCallback completeCallback)
				{

						Resource resource = null;
						mResourceBuilder.CreateResource (resourceType, out resource);

						//check param
						if (string.IsNullOrEmpty (url)) {
								if (completeCallback != null) {
										completeCallback.Invoke (url,null, E_Resource_ErrorCode.Url_Null, "url is null");
								}
								return;
						}
						if (resource == null) {
								if (completeCallback != null) {
										completeCallback.Invoke (url,null, E_Resource_ErrorCode.Resource_Null, "Resource object is null");
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
						temp.AddCallback (completeCallback);

						//notify cached.
						if (temp.mState == E_Resource_State.done) {
								temp.Notify (E_Resource_ErrorCode.Success, "Resource download Success.");
						}
						
				}

				

				public float Progress (string url)
				{
						float result = 0;
						//find
						WWW www = null;
						if (mDownloadingWWW.TryGetValue (url, out www)) {
								result = www.progress;
						}
						return result;
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
												
												Debug.Log (res.mUrl + downloading_item.Value.error);
												res.Notify (E_Resource_ErrorCode.Fail, downloading_item.Value.error);
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
												res.Notify (E_Resource_ErrorCode.Success, "Resource download Success.");
										}
					
										if (deleteList == null) {
												deleteList = new List<string> ();
										}
										deleteList.Add (downloading_item.Key);
								}

				
						}

						//delete WWW
						if (deleteList != null) {
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
										temp.Notify (E_Resource_ErrorCode.Cancel, "Cancel at Waiting");
										mWaiting.Remove (url);
										
										break;
								case E_Resource_State.downloading:
										WWW tempWWW = null;
										if (mDownloadingWWW.TryGetValue (url, out tempWWW)) {
												tempWWW.Dispose ();
												mDownloadingWWW.Remove (url);
												temp.Notify (E_Resource_ErrorCode.Cancel, "Cancel at Downlading");	
												mDownloading.Remove (url);
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