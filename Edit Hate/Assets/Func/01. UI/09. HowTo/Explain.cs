using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Explain : MonoBehaviour {
    private Sprite[] m_pExplain;
    private Sprite[] m_pSpriteNum;
    private int m_nMaxSize = 0;
    private int m_nCurrSize = 0;

    public Image m_pImage;
    public Image m_pImgNum;
    public Image m_pMouse;

    private ST_MOVE_DATA[] m_pMoveData;
    private float m_fTime = 0.0f;
	// Use this for initialization
	void Start () {
	}

    public void Initialize()
    {
        gameObject.SetActive(true);
        m_nCurrSize = 0;
        m_nMaxSize = 0;
        m_pExplain = null;
        m_pSpriteNum = null;


        InGameController.Instance.TimePause();

        if( StageMgr.Instance.GetLanguageData() != LANGUAGE_DATA.KOREAN)
        {
            return;
        }
        switch (StageMgr.Instance.SellectStage)
        {
            case 1:
                Setup(2, "02. InGame/02. UI/HowTO/move_");
                MoveDataSet(0, new Vector2(62, -26), new Vector2(288, -219));
                MoveDataSet(1, new Vector2(155, 109), new Vector2(155, -156));
                break;
            case 6:
                Setup(1, "02. InGame/02. UI/HowTO/wall_");
                break;
            case 9:
                Setup(1, "02. InGame/02. UI/HowTO/noout_");
                break;
            case 13:
                Setup(1, "02. InGame/02. UI/HowTO/null_");
       //         MoveDataSet(0, new Vector2(-485, -156), new Vector2(-167, 79));

                break;
            case 21:
                Setup(1, "02. InGame/02. UI/HowTO/dead_");
      //          MoveDataSet(0, new Vector2(-485, -156), new Vector2(-167, 79));

                break;
            case 26:
                Setup(2, "02. InGame/02. UI/HowTO/jump_");
       //         MoveDataSet(0, new Vector2(-29, -7), new Vector2(-335, 136));
      //          MoveDataSet(1, new Vector2(150, 121), new Vector2(-127, -128));

                break;
            case 51:
                Setup(1, "02. InGame/02. UI/HowTO/coin_");
     //           MoveDataSet(0, new Vector2(-485, -156), new Vector2(-167, 79));

                break;
            case 76:
                Setup(3, "02. InGame/02. UI/HowTO/warp_");
        //        MoveDataSet(0, new Vector2(-485, -156), new Vector2(-167, 79));
       //         MoveDataSet(2, new Vector2(-459, -34), new Vector2(-233, 99));
                break;
        }

        
    }

    private void MoveDataSet( int n, Vector2 vStart, Vector2 vEnd )
    {
        m_pMoveData[n].m_vStart = vStart;
        m_pMoveData[n].m_vEnd = vEnd;
    }
    private void Setup( int nSize, string strResource)
    {
        if( nSize > 3 )
        {
            nSize = 3;
        }
        m_nMaxSize = nSize;
        m_pExplain = new Sprite[nSize];
        if( nSize <= 1)
        {
            m_pImgNum.enabled = false;
        }
        else
        {
            m_pSpriteNum = new Sprite[nSize];
        }

        for( int n =0; n < nSize; ++n )
        {
            string str = "" + (nSize) + "-" + (n + 1);
            Debug.Log(str);

            if (nSize > 1)
            {
                m_pSpriteNum[n] =
                   Resources.Load("02. InGame/02. UI/HowTO/" + str, typeof(Sprite)) as Sprite;
            }

            m_pExplain[n] =
                Resources.Load(strResource + ( n + 1), typeof(Sprite)) as Sprite;
        }
        m_pMoveData = new ST_MOVE_DATA[nSize];
        for( int n =0; n < m_pMoveData.Length; ++n )
        {
            m_pMoveData[n].m_vStart = new Vector2(0, 1000);
            m_pMoveData[n].m_vEnd = new Vector2(0, 1000);
        }
    }
	
	// Update is called once per frame
	void Update () {
        m_fTime += Time.fixedDeltaTime * 0.75f;
        if( m_fTime >= 1.0f )
        {
            m_fTime = 0.0f;
        }
        if( Input.GetMouseButtonDown(0))
        {
            m_nCurrSize += 1;
            m_fTime = 0.0f;
        }
        if( m_nCurrSize >= m_nMaxSize )
        {
            InGameController.Instance.TimeStart();

            if (m_pImage != null)
            {
                m_pImage.enabled = false;
            }
            gameObject.SetActive(false);
            return;
        }
        m_pImage.sprite = m_pExplain[m_nCurrSize];
        m_pImage.SetNativeSize();
        m_pImage.enabled = true;


        m_pMouse.gameObject.transform.localPosition = Vector2.Lerp(m_pMoveData[m_nCurrSize].m_vStart, m_pMoveData[m_nCurrSize].m_vEnd, EasingUtil.easeInOutBack(0.0f, 1.0f, m_fTime) );
//        Debug.Log(m_pMouse.gameObject.transform.localPosition);
        if (m_nMaxSize > 1)
        {
            m_pImgNum.sprite = m_pSpriteNum[m_nCurrSize];
            m_pImgNum.SetNativeSize();
        }
	}
}
