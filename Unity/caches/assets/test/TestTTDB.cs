using UnityEngine;
using System.Collections;
using TTDB;
public class TestTTDB : MonoBehaviour {

	// Use this for initialization 
    void Start()
    {
        TDRoot.Instance.Init(Application.dataPath + "/Data/", new string[] { "MissionList","gun" });
        TDRoot.Instance.AddDataTunner("MissionList", 210251);
        TDRoot.Instance.AddDataTunner("gun", 16);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Destroy(GameObject.Find("Cube"));
            Debug.Log("dis");
        }
        if (GameObject.Find("Cube") == null)
        {
            Debug.Log("yes");
        }
        else
        { 
            Debug.Log("no");
        }
	}
}
