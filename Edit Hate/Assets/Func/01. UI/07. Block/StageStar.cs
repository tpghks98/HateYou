using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageStar : MonoBehaviour {

    private static Sprite[] m_pSprite = null;

    private Image m_pImage = null;
    void Start()
    {
        if (m_pSprite == null)
        {
            m_pSprite = new Sprite[3];

            for (int n = 0; n < 3; ++n)
            {
                m_pSprite[n] = Resources.Load("04. Planet/ssstar_" + (n + 1)
                    , typeof(Sprite)) as Sprite;
            }
        }
        m_pImage = GetComponent<Image>();
    }

    public void OnClear( string str )
    {
        int nID = 0;
        if( str == "Perfect" )
        {
            nID = 2;
        }
        else if (str == "Great")
        {
            nID = 1;
        }

        m_pImage.sprite = m_pSprite[nID];
    }
}
