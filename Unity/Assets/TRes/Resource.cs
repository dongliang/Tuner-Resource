using UnityEngine;

namespace Tuner.Resource
{
		public delegate void ResourceCallback (System.Object value,bool error,string message);

		public enum E_Resource_State
		{
				waiting,
				downloading,
				done
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

				public void Notify (bool error, string message)
				{
						mCallbackEvt.Invoke (error ? null : GetValue (), error, message);
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
