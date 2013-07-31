namespace Tuner.Resource
{
		public interface IResourceBuilder
		{
				void CreateResource (string resourceType, out Resource result);
		}
}