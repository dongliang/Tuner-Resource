
namespace Tuner.Resource
{
		class TunerResourceRoot:Singleton<TunerResourceRoot>
		{
				public void Update ()
				{
						ResourceMgr.Instance.Update ();
				}
		}
}