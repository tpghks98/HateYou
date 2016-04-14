using UnityEngine;
using System.Collections;

public class SetBase : MonoBehaviour {

    private static bool m_IsBaseSet =   false;

	// Use this for initialization
	void Start () {
        if (!m_IsBaseSet)
        {
            m_IsBaseSet = true;
            ObjectMgr.Instance.Initialize();
            SceneMgr.Instance.Initialize();
            Screen.SetResolution(Screen.width, (Screen.width / 16) * 9, true);
        }
	}
}
