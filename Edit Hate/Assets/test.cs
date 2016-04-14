using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(GetComponent<Animation>()["warp_down"]);
        GetComponent<Animation>().Play( "warp_down"  );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
