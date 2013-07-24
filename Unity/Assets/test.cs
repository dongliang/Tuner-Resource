using UnityEngine;
using System.Collections;
using Tuner.Resource;

public class test : MonoBehaviour
{

		// Use this for initialization
		void Awake ()
		{
				ResourceMgr.Instance.AddRequest ("localhost/test.unity3d","ab", ResourceCallback);
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
