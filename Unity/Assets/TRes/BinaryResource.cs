
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