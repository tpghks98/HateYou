using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenButton : MonoBehaviour {

    private Image m_pImage;
    private Alpha m_pAlpha;
    private bool m_IsAlphaDestory = false;


    public Image[] m_AlphaImg;
    void Start()
    {
        m_pImage = GetComponent<Image>();
        m_pAlpha = GetComponent<Alpha>();
    }

	void Update () {
        // Mobile
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (m_pImage.color.a > 0.1f)
                {
                    m_IsAlphaDestory = true;
                    Invoke("GoToHomeScene", 0.35f);

                }
            }
        }
        if( Input.GetMouseButtonDown( 0 ) )
        {

            if (m_pImage.color.a > 0.1f)
            {
                m_IsAlphaDestory = true;
                Invoke("GoToHomeScene", 0.35f);
            }
        }

        if( m_IsAlphaDestory )
        {
            if (m_pAlpha != null)
            {
                Destroy(m_pAlpha);
                m_pAlpha = null;
            }
            for (int n = 0; n < m_AlphaImg.Length; ++n)
            {
                var color = m_AlphaImg[n].color;
                color.a -= Time.deltaTime * 5;
                m_AlphaImg[n].color = color;
            }
        }

	}

    public void GoToHomeScene()
    {
        SceneMgr.Instance.ImmediateChangeScene(SCENEID.HOME); 
    }
}
