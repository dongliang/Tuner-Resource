using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tuner.Resource
{
		public class UpdateFileInfo
		{
				public string subPath;
				public string md5;
				public long size;
		}

		public class UpdateVersionInfo
		{
				public Dictionary<string,UpdateFileInfo> infos = new Dictionary<string, UpdateFileInfo> ();

				public void Deserialize (string value)
				{
						Dictionary<string,object> root_json = MiniJSON.Json.Deserialize (value) as Dictionary<string,object>;
			
						//assets json
						List<object> infos_json = root_json ["assets"] as List<object>;
						foreach (object item in infos_json) {
				
								//info obj
								UpdateFileInfo temp = new UpdateFileInfo ();
								//info json
								Dictionary<string,object> info_json = item as Dictionary<string,object>;
								temp.subPath = info_json ["subpath"].ToString ();
								temp.md5 = info_json ["md5"].ToString ();
								temp.size = (long)info_json ["size"];
								infos.Add (temp.subPath, temp);
						}
				}

				public string Serialize ()
				{
						string res = null;
						Dictionary<string,object> json_root = new Dictionary<string, object> ();
						List<object> json_assetlist = new List<object> ();
						json_root.Add ("assets", json_assetlist);
						foreach (KeyValuePair<string, UpdateFileInfo> item in infos) {

								Dictionary<string,object> tempDic = new Dictionary<string, object> ();
								tempDic.Add ("subpath", item.Value.subPath);
								tempDic.Add ("md5", item.Value.md5);
								tempDic.Add ("size", item.Value.size);
								json_assetlist.Add (tempDic);
						}
						res = MiniJSON.Json.Serialize (json_root);
						return res;
				}


				
		}
		
}
