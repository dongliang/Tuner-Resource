namespace Tuner.Resource
{
		public class Tuner_Resource_Adapter
		{
				public virtual void CreateResource (string resourceType, out Resource result)
				{
						switch (resourceType) {
						case "ab":
								result = new ABResource ();
								break;
						case "bin":
								result = new BinaryResource ();
								break;
						default:
								result = null;
								break;
						}
				}
		}
}