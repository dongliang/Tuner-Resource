using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AssetUtil
{
		public static void CreateAssetBundle (UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string path, BuildTarget target)
		{
				string parent = path.Substring (0, path.LastIndexOf ("/") + 1);
				CreateDirectory (parent);
				BuildPipeline.BuildAssetBundle (mainAsset, assets, path,
		                                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);
				AssetDatabase.Refresh ();
		}

		public static string CreateDirectory (string path)
		{
		
				if (System.IO.Directory.Exists (path) == false) {
						UnityEngine.Debug.Log ("Create:" + path);
						System.IO.Directory.CreateDirectory (path);
						AssetDatabase.Refresh ();
				}
				return path;
		}

		public static UnityEngine.GameObject[] SelectionPrefabs ()
		{
				List<GameObject> temp = new List<GameObject> ();
		
				foreach (GameObject item in Selection.gameObjects) {
						if (PrefabUtility.GetPrefabType (item) == PrefabType.Prefab) {
								temp.Add (item);
						}
				}
		
				return temp.ToArray ();
		}
	
		public static UnityEngine.Object CreateAsset (UnityEngine.Object obj, string path)
		{
				string parent = path.Substring (0, path.LastIndexOf ("/") + 1);
				CreateDirectory (parent);
				AssetDatabase.CreateAsset (obj, path);
				UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath (path);
				return asset;
		}
	

}