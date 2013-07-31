using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tuner.Resource
{
		public delegate void CheckUpdateCallback (E_CheckUpdate_ErrorCode error,int count,long size,string msg);
		
		public enum E_CheckUpdate_ErrorCode
		{
				haveUpdate,
				doNotHaveUpdate,
				error,
		}

		public class UpdateMgr:Singleton<UpdateMgr>
		{	
				public CheckUpdateCallback mCheckCallBack = null;
				public ResourceGroupCallback mCompleteCallback = null;
				VersionInfo mNetVersionInfo = null;
				VersionInfo mLocalVersionInfo = null;
				List<UpdateFileInfo> mDownLoadList = new List<UpdateFileInfo> ();
				List<UpdateFileInfo> mDeleteList = new List<UpdateFileInfo> ();
				long mSize = 0;
				const string cUpdateGroupName = "update";

				public void Check (CheckUpdateCallback callback)
				{	
						mCheckCallBack = callback;
						//download resource info on the net.
						ResourceMgr.Instance.AddRequest (LocationHelper.GetNetPath () + "/info/resourcesinfo.json", "txt", NetInfoCallBack);
						//load resource info at local.
						ResourceMgr.Instance.AddRequest (LocationHelper.GetLoadFileURL ("/info/resourcesinfo.json"), "txt", LocalInfoCallBack);
				}

				void NetInfoCallBack (string url, System.Object value, E_Resource_ErrorCode result, string message)
				{
						if (result == E_Resource_ErrorCode.Success) {
								//build
				
								mNetVersionInfo = new VersionInfo (value.ToString ());
								compare ();
						} else {
								mCheckCallBack.Invoke (E_CheckUpdate_ErrorCode.error, 0, 0, "can not download resource info.");
						}
				}
		
				void LocalInfoCallBack (string url, System.Object value, E_Resource_ErrorCode result, string message)
				{
						if (result == E_Resource_ErrorCode.Success) {
								//build
								mLocalVersionInfo = new VersionInfo (value.ToString ());
								compare ();
						} else {
								mCheckCallBack.Invoke (E_CheckUpdate_ErrorCode.error, 0, 0, "can not load local resource info.");
						}
				}

				void compare ()
				{
						if (mNetVersionInfo != null && mLocalVersionInfo != null) {
								//compare
				
								//download list
								// 1. net have && local havent.
								// 2. both have but md5 different.
								foreach (KeyValuePair<string, UpdateFileInfo> item in mNetVersionInfo.infos) {
										UpdateFileInfo temp = null;
										mLocalVersionInfo.infos.TryGetValue (item.Key, out temp);
										if (temp == null || temp.md5 != item.Value.md5) {
												mDownLoadList.Add (item.Value);
												mSize += item.Value.size;
										}
								}
				
								//delete list
								//1. local have && net havent.
								foreach (KeyValuePair<string,UpdateFileInfo> item in mLocalVersionInfo.infos) {
										UpdateFileInfo temp = null;
										mNetVersionInfo.infos.TryGetValue (item.Key, out temp);
										if (temp == null) {
												mDeleteList.Add (item.Value);
										}
								}
				
								if (mDownLoadList.Count > 0) {
										if (mCheckCallBack != null) {
												mCheckCallBack.Invoke (E_CheckUpdate_ErrorCode.haveUpdate, mDownLoadList.Count, mSize, "hanve update.");
										}
					
								} else {
										if (mCheckCallBack != null) {
												mCheckCallBack.Invoke (E_CheckUpdate_ErrorCode.doNotHaveUpdate, 0, 0, "the local version is the lastest version.");
										}
					
								}
						}
				}

				public GroupProgress Progress ()
				{
						return	ResourceGroupMgr.Instance.Progress (cUpdateGroupName);
				}

				public void Start (ResourceGroupCallback completeCallback)
				{
						mCompleteCallback = completeCallback;

						//add group
						ResourceGroupMgr.Instance.addGroup (cUpdateGroupName, GroupCallback);
						foreach (UpdateFileInfo item in mDownLoadList) {
								//add net download to group
								string url = LocationHelper.GetNetPath () + item.subPath;
								ResourceGroupMgr.Instance.addRequest (cUpdateGroupName, url, "bin", downloadCallback);
						}
						//start group
						ResourceGroupMgr.Instance.Start (cUpdateGroupName);
				}

				void downloadCallback (string url, System.Object value, E_Resource_ErrorCode result, string message)
				{
						//save file.
						string subPath = url.Substring (LocationHelper.GetNetPath ().Length);
						string savePath = LocationHelper.GetCachePath () + subPath;
						byte[] bytes = (byte[])value;
						saveBytes (savePath, bytes);
						Debug.Log (subPath);
				}

				void saveBytes (string path, byte[] bytes)
				{
						FileStream stream = null;
						if (File.Exists (path)) {
								File.Delete (path);
						}
						string parent = path.Substring (0, path.LastIndexOf ("/") + 1);
						LocationHelper.CreateDirectory (parent);
						stream = new FileStream (path, FileMode.Create);
			
						BinaryWriter writer = new BinaryWriter (stream);
						writer.Write (bytes);
						writer.Flush ();
				}
		
				void GroupCallback (int count, string[] success, string[] error)
				{
						//save version
						foreach (UpdateFileInfo item in mDownLoadList) {
								mLocalVersionInfo.infos.Add (item.subPath, item);
						}
						string json = mLocalVersionInfo.Serialize ();

						byte[] bytes = Encoding.UTF8.GetBytes (json.ToCharArray ());
						saveBytes (LocationHelper.GetCachePath () + "/info/resourcesinfo.json", bytes);
						mCompleteCallback.Invoke (count, success, error);
				}

				public void Relase ()
				{
						ResourceGroupMgr.Instance.removeGroup (cUpdateGroupName);
						mCheckCallBack = null;
						mCompleteCallback = null;
						mNetVersionInfo = null;
						mLocalVersionInfo = null;
						
						if (mDownLoadList != null) {
								mDownLoadList.Clear ();
								mDownLoadList = new List<UpdateFileInfo> ();
						}
						if (mDeleteList != null) {
								mDeleteList.Clear ();
								mDeleteList = new List<UpdateFileInfo> ();
						}
						mSize = 0;
				}
		}
}

