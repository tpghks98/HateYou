using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaUI : MonoBehaviour {

    Image m_pImage = null;

	// Use this for initialization
	void Start () {
        m_pImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        float fAlpha = SceneMgr.Instance.AlphaColor;
        if( fAlpha < 1.0f )
        {
            var color = m_pImage.color;
            color.a = fAlpha;
            m_pImage.color = color;
        }
	}

    public void OnExit()
    {
        SceneMgr.Instance.GameExit();
    }
}
