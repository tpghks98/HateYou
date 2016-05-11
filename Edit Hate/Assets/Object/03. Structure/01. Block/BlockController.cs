using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BlockController : ObjBase {

    public GameObject m_goBlockParent;
    private Block[, ,] m_ArrBlock;
    private Item[ , ,] m_ArrItem;
    private int m_nBlockSize    =   0;

    private Material m_mtlWhite;
    private Material m_mtlBlack;


    public Material MaterialWhite
    {
        get { return m_mtlWhite; }
        set { m_mtlWhite = value; }
    }
    public Material MaterialBlack
    {
        set { m_mtlBlack = value; }
        get { return m_mtlBlack; }
    }
    public int BlockSize
    {
        get { return m_nBlockSize;  }
    }

    public override void Initialize()
    {
     
        m_mtlWhite = Resources.Load("Materials/White", typeof(Material)) as Material;
        m_mtlBlack = Resources.Load("Materials/Black", typeof(Material)) as Material;

        /*
        switch( StageMgr.Instance.SellectChapter)
        {
            case 1:
                m_mtlBlack.mainTexture = 
                    Resources.Load( "02. InGame/03. Texture/1/Grass_ON_1"
                    ,typeof( Texture ) ) as Texture;
                m_mtlWhite.mainTexture =
                    Resources.Load("02. InGame/03. Texture/1/Grass_OFF"
                    , typeof(Texture)) as Texture;
                break;
            case 2:
                m_mtlBlack.mainTexture = 
                    Resources.Load( "02. InGame/03. Texture/2/Water_ON_1"
                    ,typeof( Texture ) ) as Texture;
                m_mtlWhite.mainTexture =
                    Resources.Load("02. InGame/03. Texture/2/Water_OFF"
                    , typeof(Texture)) as Texture;
                break;
            case 3:
                m_mtlBlack.mainTexture =
                    Resources.Load("02. InGame/03. Texture/3/Volcano_ON2_2"
                    , typeof(Texture)) as Texture;
                m_mtlWhite.mainTexture =
                    Resources.Load("02. InGame/03. Texture/3/Volcano_OFF"
                    , typeof(Texture)) as Texture;
                break;
            case 4:
                m_mtlBlack.mainTexture =
                    Resources.Load("02. InGame/03. Texture/4/Wood_ON_1"
                    , typeof(Texture)) as Texture;
                m_mtlWhite.mainTexture =
                    Resources.Load("02. InGame/03. Texture/4/Wood_OFF"
                    , typeof(Texture)) as Texture;
                break;
            case 5:
                m_mtlBlack.mainTexture =
                    Resources.Load("02. InGame/03. Texture/5/DeathWish_ON_1"
                    , typeof(Texture)) as Texture;
                m_mtlWhite.mainTexture =
                    Resources.Load("02. InGame/03. Texture/5/DeathWish_OFF"
                    , typeof(Texture)) as Texture;
                break;
        }
     */
    }

    public void DestroyItem( ref ST_POS_INFOR infor)
    {
        if (m_ArrItem[(int)infor.vs, infor.pt.y, infor.pt.x] != null)
        {
            GameObject.Destroy(
                m_ArrItem[(int)infor.vs, infor.pt.y, infor.pt.x].gameObject);
            m_ArrItem[(int)infor.vs, infor.pt.y, infor.pt.x] = null;
        }
    }
    public bool IsGameClear()
    {
        bool IsClear = true;
        foreach( var it in m_ArrBlock)
        {
            if (it.IsWhite)
            {
                IsClear = false;
                break;
            }
        }
        foreach( var it in m_ArrItem )
        {
            if( it != null )
            {
                if( it.ID == ITEMID.COIN )
                {
                    IsClear = false;
                    break;
                }
            }
        }
        return IsClear;
    }
    public Item GetItem(ref ST_POS_INFOR infor) { return m_ArrItem[(int)infor.vs, infor.pt.y, infor.pt.x]; }
    public void GetBlockPos(out Vector3 vOut, ref ST_POINT v, VIEWSTATE vs) { vOut = m_ArrBlock[(int)vs, v.y, v.x].transform.position; }
    public void GetBlockOnPos(out Vector3 vOut, ref ST_POINT v, VIEWSTATE vs, bool IsPlayer = false) 
    {
        Vector3 vPlusPositon = Vector3.zero;
        switch( vs )
        {
            case VIEWSTATE.TOP:
                vPlusPositon.y = 0.4f;
                break;
            case VIEWSTATE.RIGHT:
                vPlusPositon.z = 0.4f;
                break;
            case VIEWSTATE.FRONT:
                vPlusPositon.x = 0.4f;
                break;
        }
        float fVal = 1.0f;
        if( IsPlayer )
        {
            if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO )
            {
                fVal = 2.2f;
            }
        }

        GetBlockPos(out vOut, ref v, vs);
        vOut += vPlusPositon * fVal;

    }
    public void CreateBlock(VIEWSTATE vsView, int nSize,
        float fDist)
    {

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        m_nBlockSize = nSize;
        if( m_goBlockParent == null )
        {
            m_goBlockParent = GameObject.Find("BlockParent");
            m_goBlockParent.transform.localPosition =
                new Vector3(nSize * fDist - 1, 0, fDist * 0.5f);
        }
        int nView = (int)vsView;
        m_ArrBlock = new Block[nView, nSize, nSize];
        m_ArrItem = new Item[nView, nSize, nSize];
        GameObject go = null;
        for (int nZ = 0; nZ < (int)vsView; ++nZ)
        {
            go = new GameObject();
            go.name = ((VIEWSTATE)nZ).ToString();
            go.transform.SetParent(m_goBlockParent.transform);
          
            CreateViewBlock( nSize, (VIEWSTATE)nZ, fDist, go);
            switch ((VIEWSTATE)nZ)
            {
                case VIEWSTATE.FRONT:
                    go.transform.Translate(
                         new Vector3(fDist * nSize - fDist * 0.5f,
                            -fDist * 0.5f, 0.0f));
                    go.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case VIEWSTATE.RIGHT:
                    go.transform.Translate(new Vector3(0, -fDist * nSize + fDist * 0.5f
                        , fDist * 0.5f));
                        go.transform.rotation = Quaternion.Euler(90, 0, 0);
                    break;
            }
        }
        m_goBlockParent.transform.position = m_goBlockParent.transform.position + new Vector3(0, 2, 0);
    }


    public void OnBlock(ref ST_POS_INFOR posinfor) { m_ArrBlock[(int)posinfor.vs, posinfor.pt.y, posinfor.pt.x].OnCollisionPlayer(); }

    public void BlockSetting( ref string str, ref int nCount )
    {
        // Block Setting
        bool IsWhite = false;
        for (int n = 0; n < (int)VIEWSTATE.MAX; ++n)
        {
            for (int y = 0; y < m_nBlockSize; ++y)
            {
                for (int x = 0; x < m_nBlockSize; ++x)
                {
                    
                    if (str[nCount++].ToString() == "w")
                    {
                        IsWhite = true;
                    }
                    else
                    {
                        IsWhite = false;
                    }
                    m_ArrBlock[n, y,  x]
                        .ChangeColor(IsWhite);

                    ITEMID ID = (ITEMID)System.Convert.ToInt32(str[nCount++].ToString()); // Item
                    CreateItem(n, y, x, ID);

                }
            }
        }

        // MoveCount
        int nMoveCount = DataMgr.Instance.GetIntegerToken(ref str, ref nCount);
        InGameController.Instance.Player.MoveCount = nMoveCount + 2;
    }


    public void CreateItem( int nView, int y, int x, ITEMID ID)
    {
        string strLoadName = "02. InGame/01. Model/03. Item/";

        Vector3 vPosition = m_ArrBlock[nView, y, x].transform.position;
        Vector3 vPlus = Vector3.up;

        if (m_ArrItem[nView, y, x] != null)
        {
            GameObject.Destroy(
            m_ArrItem[nView, y, x].gameObject);
            m_ArrItem[nView, y, x] = null;
        }

        switch (ID)
        {
            case ITEMID.NONE:
                return;
            case ITEMID.WALL:
                strLoadName += "01. Wall/Wall";
                vPlus *= 2.3f;
                break;
            case ITEMID.NULL:
                strLoadName += "02. Null/Null";
                vPlus *= 2.7f;
                break;
            case ITEMID.DEAD:
                strLoadName += "03. Dead/Dead";
                vPlus *= 0.6f;
                break;
            case ITEMID.COIN:
                strLoadName += "04. Coin/Coin";
                vPlus *= 2.5f;
                break;
            case ITEMID.JUMP:
                strLoadName += "05. Jump/Jump";
                vPlus *= 1.6f;
                break;
            case ITEMID.WARP:
                strLoadName += "06. Warp/Warp";
                vPlus *= 2.5f;
                
                break;
        }

        Item pItem = ObjectMgr.Instance.CreateObject<Item>(vPosition, strLoadName);
        pItem.transform.rotation =
            m_ArrBlock[nView, y, x]
            .transform.rotation;
        pItem.gameObject.name += ID.ToString();
        pItem.View = (VIEWSTATE)nView;
        pItem.Point = new ST_POINT((short)x, (short)y);
        pItem.ID = ID;


        ObjectMgr.Instance.RotatePos(ref vPlus, new Vector3(-270, 0, 0),
            pItem.gameObject);

        pItem.transform.Translate(vPlus * 0.4f);

        m_ArrItem[nView, y, x] = pItem;
    }
    private void CreateViewBlock(int nSize,
        VIEWSTATE vs, float fDist, GameObject goTarget)
    {
        Block pBlock = null;

        for (int nY = 0; nY < nSize; ++nY)
        {
            for (int nX = 0; nX < nSize; ++nX)
            {
                pBlock = ObjectMgr.Instance.CreateObject<Block>(
                    "02. InGame/01. Model/01. Tile/Object");


                // Format 설정떄문에 x와 y를 뒤집고 값도 반전시킴

                m_ArrBlock[(int)vs, (nSize - 1) - nX, (nSize - 1) - nY] = pBlock;
                pBlock.transform.SetParent(goTarget.transform);

                pBlock.transform.Translate(
                    new Vector3(nX * fDist, nY * fDist, 0.0f));
            }
        }
    }
    void Start()
    {
        Camera.main.transform.LookAt(m_goBlockParent.transform.position);
    }
    void Update()
    {
	
	}
}
