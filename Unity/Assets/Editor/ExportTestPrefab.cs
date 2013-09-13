/*
   Tunner Resource - Easy to manage resource in Unity3d. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Resource
*/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

class ExportTestPrefab
{
		const string menuTitle = "Tuner/Export/Prefab";

		[MenuItem(menuTitle)]
		static void Export ()
		{
				foreach (GameObject item in AssetUtil.SelectionPrefabs()) {

						AssetUtil.CreateAssetBundle (item, null, Tuner.LocationHelper.GetBundlePath () + "/assets/test1.unity3d", BuildTarget.iPhone);
						Debug.Log (item.name);
				}
		}
}
