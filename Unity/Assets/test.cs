using UnityEngine;
using System.Collections;
using Tuner.Resource;
using System.IO;

public class test : MonoBehaviour
{
		public	TextMesh tm;

		// Use this for initialization
		void Awake ()
		{
				//Debug.Log (Application.dataPath);
				//ResourceMgr.Instance.AddRequest (Tuner.Resource.LocationHelper.GetLoadFileURL ("/Assets/test1.unity3d"), "ab", ResourceCallback);

				TResRoot.Instance.Init (new TResAdapter());
				TResRoot.Instance.CheckUpdate (CheckUpdateCallback);
				
		}

		public void CheckUpdateCallback (E_CheckUpdate_ErrorCode error, int count, long size, string msg)
		{
				if (error == E_CheckUpdate_ErrorCode.haveUpdate) {
						Debug.Log ("update file: " + count.ToString ());
						Debug.Log ("total size:" + size.ToString ());
						TResRoot.Instance.StartUpdate (UpdateDoneCallback);
				} else {
						Debug.Log (error);
				}
		}

		public void UpdateDoneCallback (int count, string[] success, string[] error)
		{
				Debug.Log ("update count : " + count.ToString ());
				Debug.Log ("success:");
				foreach (string item in success) {
						Debug.Log (item);
				}
				foreach (string item in error) {
						Debug.Log (item);
				}
		}

		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			
				TResRoot.Instance.Update ();
		}

		public void ResourceCallback (System.Object value, E_Resource_ErrorCode errorCode, string message)
		{
				if (gameObject != null && errorCode == E_Resource_ErrorCode.Success) {
						GameObject.Instantiate (value as GameObject);
				}
		}
}
