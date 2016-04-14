using UnityEngine;
using System.Collections;

public class Static : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if( GameObject.Find( gameObject.name) != gameObject )
        {
            GameObject.Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }
}
