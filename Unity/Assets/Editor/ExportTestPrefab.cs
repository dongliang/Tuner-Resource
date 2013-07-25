using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

class ExportTestPrefab
{
		const string menuTitle = "Export/Prefab";

		[MenuItem(menuTitle)]
		static void Export ()
		{
				foreach (GameObject item in AssetUtil.SelectionPrefabs()) {

						AssetUtil.CreateAssetBundle (item, null, Tuner.Resource.LocationHelper.GetBundlePath () + "/Assets/test1.unity3d", BuildTarget.iPhone);
						Debug.Log (item.name);
				}
		}
}
