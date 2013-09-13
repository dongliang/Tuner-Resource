/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
using System.Collections.Generic;
using UnityEngine;

namespace Tuner.Resource
{
		public delegate void ResourceGroupCallback (int count,string[] success,string[] error);

		enum E_ResourceGroup_State
		{
				prepare,
				start,
				end,
		}

		public class RequestInfo
		{
				public string url;
				public string resourceType;
				public ResourceCallback completeCallback;
		}

		public class GroupProgress
		{
				public int count;
				public int processed;
				public float currentItemProgress;
				public int errorNumber;
		}
	
		public class ResourceGroup
		{
				ResourceGroupCallback mCallback = null;
				E_ResourceGroup_State mState = E_ResourceGroup_State.prepare;
				Dictionary<string,RequestInfo> mRequestInfos = new Dictionary<string, RequestInfo> ();
				List<string> mWaitings = new List<string> ();
				string downloading = null;
				List<string> mDones = new List<string> ();
				List<string> mErrors = new List<string> ();
				int count = 0;
				int current = 0;

				public  ResourceGroup (ResourceGroupCallback callback)
				{
						mCallback = callback;
				}

				public void AddRequest (string url, string resourceType, ResourceCallback completeCallback)
				{
						if (string.IsNullOrEmpty (url)) {
								completeCallback.Invoke (url,null, Tuner.Resource.E_Resource_ErrorCode.Url_Null, "can not add to group,url is null");
								return;
						}
						if (string.IsNullOrEmpty (resourceType)) {
								completeCallback.Invoke (url,null, Tuner.Resource.E_Resource_ErrorCode.Resource_Null, "can not add to group,resource is null");
								return;
						}
						if (mState == E_ResourceGroup_State.prepare) {

								RequestInfo temp = new RequestInfo ();
								temp.url = url;
								temp.resourceType = resourceType;
								temp.completeCallback = completeCallback;
								mRequestInfos.Add (url, temp);
								mWaitings.Add (url);
								count++;

						} else {
								completeCallback (url,null, Tuner.Resource.E_Resource_ErrorCode.Fail, "can't add to group, group state is not propare.");
						}
				}

				public GroupProgress Progress ()
				{
						float currentItemProgress = 0;
						if (downloading != null) {
								currentItemProgress = ResourceMgr.Instance.Progress (downloading);
						}
						GroupProgress temp = new GroupProgress ();
						temp.count = count;
						temp.currentItemProgress = currentItemProgress;
						temp.processed = current;
						temp.errorNumber = mErrors.Count;
						return temp;
				}

				public void Relase ()
				{
						foreach (KeyValuePair<string, RequestInfo> item in mRequestInfos) {
								ResourceMgr.Instance.Remove (item.Value.url);
						}
						mRequestInfos.Clear ();
						mDones.Clear ();
						mErrors.Clear ();
						downloading = null;
						count = 0;
						current = 0;
						mCallback = null;
						mState = E_ResourceGroup_State.prepare;
				}
		
				public void Start ()
				{
						if (SendRequest ()) {
								mState = E_ResourceGroup_State.start;
						} else {
								Debug.Log ("can not start,group is null.");
						}
				}

				bool SendRequest ()
				{
						if (mWaitings.Count > 0) {
								RequestInfo temp = mRequestInfos [mWaitings [0]];
								ResourceMgr.Instance.AddRequest (temp.url, temp.resourceType, ResourceCallback_group);
								downloading = temp.url;
								mWaitings.Remove (temp.url);
								return true;

						} else {
								return false;
						}
				}

				void ResourceCallback_group (string url,System.Object value, E_Resource_ErrorCode result, string message)
				{	
						//call back function
						if (mRequestInfos [downloading].completeCallback != null) {
				mRequestInfos [downloading].completeCallback.Invoke (url,value, result, message);
						}

						//add downloading to error or done.
						if (result == E_Resource_ErrorCode.Success) {

								mDones.Add (downloading);

						} else {

								mErrors.Add (downloading);
						}

						downloading = null;
			
						current++;
						
						if (current == count) {
						  
								if (mCallback != null) {
										mCallback.Invoke (count, mDones.ToArray (), mErrors.ToArray ());
										mState = E_ResourceGroup_State.end;
								}

						} else {
								if (!SendRequest ()) {
										Debug.Log ("Error,mWating not enough. current:" + current.ToString () + "count: " + count.ToString ());
								}
						}
				}
		}
}
