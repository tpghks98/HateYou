using UnityEngine;
using UnityEditor;
using System.Collections;

public class Cache : MonoBehaviour {
	[MenuItem ("Assets/ClearABCache")]
	static void ClearABCache ()
	{
		//		BuildPipeline.BuildAssetBundles (Application.dataPath+"/StreamingAssets");
		PlayerPrefs.DeleteAll();
		System.IO.Directory.Delete(Application.persistentDataPath,true);

	}
}
