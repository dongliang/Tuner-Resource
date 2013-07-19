using UnityEngine;
using System.Collections;
using Tuner.Resource;

public class test : MonoBehaviour
{

		// Use this for initialization
	void Awake()
	{

			ResourceMgr.Instance.AddRequest("localhost/test.unity3d",new ABResource(),ResourceCallback);
	}
		void Start ()
		{
	
	}
	
		// Update is called once per frame
		void Update ()
		{
				ResourceMgr.Instance.Update ();
		}

		public void ResourceCallback (System.Object value, bool error, string message)
		{


				if (gameObject != null && !error) {
						GameObject.Instantiate (value as GameObject);
				}
		}
}
