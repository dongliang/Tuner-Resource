using UnityEngine;

namespace Tuner.Resource
{
		public class ABResource:Resource
		{
				AssetBundle ab = null;

				public override void OnDownloadComplete (UnityEngine.WWW www)
				{
						base.OnDownloadComplete (www);
						ab = www.assetBundle;
				}

				public override System.Object GetValue ()
				{
						return ab.mainAsset;
				}

				public override void Relase ()
				{
						base.Relase ();
						if (ab != null) {
								ab.Unload (false);
						}
						
						ab = null;
				}
			
		}
}
