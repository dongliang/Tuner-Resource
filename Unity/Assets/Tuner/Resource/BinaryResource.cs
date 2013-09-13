/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
namespace Tuner.Resource
{
		public class BinaryResource:Resource
		{
				public byte[] mBytes = null;

				public override void OnDownloadComplete (UnityEngine.WWW www)
				{
						base.OnDownloadComplete (www);
						mBytes = www.bytes;
				}

				public override System.Object GetValue ()
				{
						return mBytes;
				}

				public override void Relase ()
				{
						base.Relase ();
						mBytes = null;
				}
		}


}