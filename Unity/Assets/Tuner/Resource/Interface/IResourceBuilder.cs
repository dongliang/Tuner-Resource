/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
namespace Tuner.Resource
{
		public interface IResourceBuilder
		{
				void CreateResource (string resourceType, out Resource result);
		}
}