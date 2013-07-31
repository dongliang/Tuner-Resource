namespace Tuner.Resource
{
		public class TResAdapter:IResourceBuilder,INetPathHolder
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
						case "txt":
								result = new TextResource ();
								break;
						default:
								result = null;
								break;
						}
				}

				public virtual string GetNetPath ()
				{
						return "localhost";
				}
		}
}