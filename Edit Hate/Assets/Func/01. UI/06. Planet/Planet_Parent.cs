using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Planet_Parent : MonoBehaviour {

    private Sprite[] m_pSprite;
    public Image[] m_pImage;
    public StageBlockController m_pStageBlockCtrl = null;
    private float m_fTime = 0.0f;
    private ScrollBar m_pScroll;

	// Use this for initialization
	void Start () {
        m_pScroll = GetComponent<ScrollBar>();
	}

    public void LoadData( int nChapter, int nPlanetNum)
    {
        m_pSprite = new Sprite[nPlanetNum];

        for( int n =0; n < nPlanetNum; ++n )
        {
            m_pSprite[n] =
                Resources.Load( "04. Planet/Planet/" + nChapter + "/"  + (n + 1)
                , typeof( Sprite ) ) as Sprite;

            m_pImage[n].sprite= m_pSprite[n];
            m_pImage[n].SetNativeSize();
        }        
        m_pScroll.MoveDistance(StageMgr.Instance.SellectPlanet);
        OpenBlock( StageMgr.Instance.SellectPlanet);
    }

    public void OpenBlock(int nPlanet)
    {

        if (nPlanet <= 0)
        {
            nPlanet = 1;
        }
        StageMgr.Instance.SellectPlanet = nPlanet;

        int nStageNum =
        (StageMgr.Instance.SellectChapter - 1) * 25
            + ((nPlanet - 1) * 5);

        m_pStageBlockCtrl.OpenBlock(nStageNum);

        for (int n = 0; n < 5; ++n)
        {
            var color = m_pImage[n].color;
            if (n != (nPlanet - 1))
            {
                color = new Color(0.6f, 0.6f, 0.6f);
            }
            else
            {
                color = new Color(1.0f, 1.0f, 1.0f);
            }
            m_pImage[n].color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

	}
}
