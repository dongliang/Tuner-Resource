/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
using System.Text;

namespace Tuner.Resource
{
		class TextResource:Resource
		{
				public string mText = null;
		
				public override void OnDownloadComplete (UnityEngine.WWW www)
				{
						base.OnDownloadComplete (www);
						byte[] bytes = www.bytes;
						mText = Encoding.UTF8.GetString (bytes, 0, bytes.Length);
				}
		
				public override System.Object GetValue ()
				{
						return mText;
				}
		
				public override void Relase ()
				{
						base.Relase ();
						mText = null;
				}
		}
}