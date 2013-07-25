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
				Debug.Log (Application.dataPath);
				ResourceMgr.Instance.AddRequest (Tuner.Resource.LocationHelper.GetLoadFileURL ("/Assets/test1.unity3d"), "ab", ResourceCallback);
		}

		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			
				ResourceMgr.Instance.Update ();
		}

		public void ResourceCallback (System.Object value, E_Resource_ErrorCode errorCode, string message)
		{
				if (gameObject != null && errorCode == E_Resource_ErrorCode.Success) {
						GameObject.Instantiate (value as GameObject);
				}
		}
}
