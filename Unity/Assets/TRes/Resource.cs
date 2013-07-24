using UnityEngine;

namespace Tuner.Resource
{
	public delegate void ResourceCallback (System.Object value,E_Resource_ErrorCode result ,string message);

		public enum E_Resource_State
		{
				waiting,
				downloading,
				done
		}

		public enum E_Resource_ErrorCode
		{
				Success,
				Cancel,
				Fail,
				Url_Null,
				Resource_Null
				
				
		}

		public class Resource
		{
				public string mUrl;
				public	E_Resource_State mState = E_Resource_State.waiting;

				public event ResourceCallback mCallbackEvt;

				public bool IsDone {
						get {
								return mState == E_Resource_State.done;
						}
				}

				public virtual void	OnDownloadComplete (WWW www)
				{
						mState = E_Resource_State.done;
				}

				public virtual System.Object GetValue ()
				{
						return null;
				}

				public virtual void Relase ()
				{
					
				}
				
				//callback
				public void addCallback (ResourceCallback callback)
				{
						if (callback == null) {
								return;
						} else {
								mCallbackEvt += callback;
						}
				}
		
				public void removeCallback (ResourceCallback callback)
				{
						if (callback == null) {
								return;
						} else {
								mCallbackEvt -= callback;
						}
				}

		public void Notify (E_Resource_ErrorCode result, string message)
				{
			mCallbackEvt.Invoke (result == E_Resource_ErrorCode.Success ?GetValue (): null , result, message);
						clearCallback ();
				}
		
				public void clearCallback ()
				{
						while (mCallbackEvt != null) {
								mCallbackEvt -= mCallbackEvt;
						}
				}
		}
}
