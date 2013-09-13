/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
using System.Collections.Generic;

namespace Tuner.Resource
{
		public class ResourceGroupMgr:Singleton<ResourceGroupMgr>
		{
				Dictionary<string,ResourceGroup> mGroups = new Dictionary<string, ResourceGroup> ();

				public void addGroup (string groupName, ResourceGroupCallback callback)
				{
						ResourceGroup temp = null;
						mGroups.TryGetValue (groupName, out temp);
						if (temp == null) {
								temp = new ResourceGroup (callback);
								mGroups.Add (groupName,temp);
						}
				}
				
				public void addRequest (string groupName, string url, string resourceType, ResourceCallback completeCallback)
				{	
						ResourceGroup temp = null;
						mGroups.TryGetValue (groupName, out temp);
						if (temp != null) {
								temp.AddRequest (url, resourceType, completeCallback);
						}
				}
				
				public void removeGroup (string groupName)
				{
						if (mGroups.ContainsKey (groupName)) {
								mGroups [groupName].Relase ();
						}
						mGroups.Remove (groupName);
				}

				public void Start (string groupName)
				{
						ResourceGroup temp = null;
						mGroups.TryGetValue (groupName, out temp);
						if (temp != null) {
								temp.Start ();
						}
				}

				public GroupProgress Progress (string groupName)
				{
						GroupProgress progress = null;
						ResourceGroup temp = null;
						
						mGroups.TryGetValue (groupName, out temp);
						if (temp != null) {
								progress = temp.Progress ();
						}
						return progress;
				}
		}
}
