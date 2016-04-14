using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class InGameController : SingleTon<InGameController> {

    private BlockController m_pBlockCtrl = null;
    private int m_nMaxIndex = 5;
    private float m_fBlockSize = 1.8f;
    private Player m_pPlayer = null;
    private int m_nPerfectCount = 0;
    private int m_nClearCount = 0;
    private bool m_IsOnWarp = false;
    private bool m_IsDie = false;
    private ShowNumber m_pMoveCount = null;

    private SpriteRenderer m_pBackStRenderer;
    private bool m_IsPause = false;

    private Explain m_pExplainObject = null;

    
    // property

    public void TimePause()
    {
        m_IsPause = true;
        Time.timeScale = 0.0f;
        if (m_pPlayer != null)
        {
            m_pPlayer.OnMouseReset();
        }
    }
    public void TimeStart()
    {
        m_IsPause = false;
        Time.timeScale = 1.0f;
        if( m_pPlayer != null )
        {
            m_pPlayer.OnMouseReset();
        }
    }
    public bool Pause
    {
        get { return m_IsPause;  }
    }
     public Player Player
    {
        get { return m_pPlayer; }
        set { m_pPlayer = value;  }
    }

    public bool IsOnWarp
     {
         get { return m_IsOnWarp; }
         set { m_IsOnWarp = value; }
     }
    public bool IsDie
    {
        get { return m_IsDie; }
        set { m_IsDie = value; }
    }

    public override void Initialize()
    {
        if (m_pBlockCtrl != null)
        {
            m_pBlockCtrl.DestroyObject();
        }
        m_pBlockCtrl = ObjectMgr.Instance.CreateObject<BlockController>();
        LoadStage(StageMgr.Instance.SellectStage);
    }
    void Start()
    {
        m_pBackStRenderer = GameObject.Find("Back_ST").GetComponent<SpriteRenderer>();
        OnBackStEnable();
    }

    public bool CheckGameClear()
    {
        if( m_pBlockCtrl.IsGameClear())
        {
            string strPlayerPrefs = "ST" + StageMgr.Instance.SellectStage;
            var State = GameObject.Find("State").GetComponent<State>();
            if (m_pPlayer.MoveCount >= 2)
            {
                State.OnClearMsg(STATEID.PERFECT);
                State.SetState(STATEID.PERFECT);
                PlayerPrefs.SetString( strPlayerPrefs, "Perfect");

            }
            else
            {
                State.OnClearMsg(STATEID.GREAT);
                State.SetState(STATEID.GREAT);
                string str = PlayerPrefs.GetString(strPlayerPrefs);
                if( str != "Perfect" )
                {
                    PlayerPrefs.SetString(strPlayerPrefs, "Great");
                }
            }

            if( StageMgr.Instance.ClearStgae < StageMgr.Instance.SellectStage )
            {
                StageMgr.Instance.ClearStgae = StageMgr.Instance.SellectStage;
                PlayerPrefs.SetInt("ClearStage", StageMgr.Instance.ClearStgae);

                if( StageMgr.Instance.OpenChapter < 1 + ( StageMgr.Instance.ClearStgae / 25 ) )
                {
                    StageMgr.Instance.OpenChapter =
                        1 + (StageMgr.Instance.ClearStgae / 25);
                    PlayerPrefs.SetInt("OpenChapter", StageMgr.Instance.OpenChapter);
                }
            }

            StageMgr.Instance.SellectChapter = ( ( 
                StageMgr.Instance.SellectStage )  / 25 ) + 1;
            StageMgr.Instance.SellectPlanet = ( 
                (StageMgr.Instance.SellectStage - 
                ( StageMgr.Instance.SellectChapter - 1) * 25) - 1 ) / 5 + 1;
            return true;
        }
        return false;
    }
    public void GameOver()
    {
        if (!m_IsDie)
        {
  //          m_pPlayer.OnAnimation("Dead");
            m_pPlayer.Action = null;
            m_IsDie = true;
            var State = GameObject.Find("State").GetComponent<State>();
            State.SetState(STATEID.FAIL);
            State.OnState(true);
        }
    }
    

    public Material GetMaterial(bool IsWhite)
    {
        if( m_pBlockCtrl == null  )
        {
            return null;
        }

        if (IsWhite)
        {
            return m_pBlockCtrl.MaterialWhite;
        }
        else
        {
            return m_pBlockCtrl.MaterialBlack;
        }
    }

    public BlockController GetBlockCtrl() { return m_pBlockCtrl; }

    public BaseAction GetAction(Character Target, ref ST_POS_INFOR posinfor)
    {
        BaseAction pAction = null;
        bool IsWarpOff = true;
        if (Target.Point == posinfor.pt && Target.View == posinfor.vs)
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.MOVE);
            pAction.ActionSetup(Target, 0.5f, ref posinfor.pt, posinfor.vs);
            Target.OnAnimation("NoOut", 0.5f);
            return pAction;
            IsWarpOff = false;
        }

        Item pItem = m_pBlockCtrl.GetItem(ref posinfor);


        if (pItem == null)
        {
            if (Target.View != posinfor.vs)
            {
                pAction = Acting(Target, ref posinfor, ACTIONID.TRANSMOVE);
            }
            else
            {
                pAction = Acting(Target, ref posinfor, ACTIONID.MOVE);
            }
            IsWarpOff = true;
        }
        else
        {
            Debug.Log(pItem.ID);
            switch (pItem.ID)
            {
                case ITEMID.WALL:
                    pAction = KickAction(Target, ref posinfor);
                    break;
                case ITEMID.COIN:
                    pAction = KickAction(Target, ref posinfor);
                    break;
                case ITEMID.NULL:
                    pAction =
                        PushAction(Target, ref posinfor, ACTIONID.NULL);

                    Target.Ability = new NullAbility();
                    Target.Ability.Setup(Target);
                    break;
                case ITEMID.JUMP:
                    pAction = 
                        PushAction(Target, ref posinfor, ACTIONID.GETJUMP);
                  //  pAction = Acting(Target, ref posinfor, ACTIONID.PUSH
                   //     ,ACTIONID.GETJUMP);
                    break;
                case ITEMID.WARP:
                    if (Target.View != posinfor.vs)
                    {
                        pAction = Acting(Target, ref posinfor, ACTIONID.TRANSMOVE);
                    }
                    else
                    {
                        pAction = Acting(Target, ref posinfor, ACTIONID.MOVE);
                    }
                    Target.Ability = new WarpAbility();
                    Target.Ability.Setup(Target);
                    m_IsOnWarp = true;
                    IsWarpOff = false;
                    break;
                case ITEMID.DEAD:
                    pAction = DeadAction(Target, ref posinfor);
                    break;
                default:
                    return null;
            }
        }
        if(IsWarpOff)
        {
            InGameController.Instance.IsOnWarp = false;
            if (Target.Ability != null)
            {
                if (Target.Ability.GetType() == typeof(WarpAbility))
                {
                    Target.Ability = null;
                }
            }
        }
        return pAction;
    }
    private BaseAction DeadAction( Character Target, ref ST_POS_INFOR posinfor)
    {
        BaseAction pAction;
        if (Target.View != posinfor.vs)
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.TRANSMOVE, ACTIONID.DEAD);
        }
        else
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.MOVE, ACTIONID.DEAD);
        }
        pAction.LimitTime = 0.65f;
        return pAction;
    }
    private BaseAction KickAction( Character Target, ref ST_POS_INFOR posinfor)
    {
        BaseAction pAction;
        if (Target.View != posinfor.vs)
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.TRANSMOVE, ACTIONID.BLOCK);
        }
        else
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.MOVE, ACTIONID.BLOCK);
        }
        pAction.LimitTime = 0.65f;
        if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO )
        {
            pAction.LimitTime = 0.4f;
        }
        return pAction;
    }

    private BaseAction PushAction( Character Target, ref ST_POS_INFOR posinfor, ACTIONID endAction)
    {
        BaseAction pAction;
        if (Target.View != posinfor.vs)
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.TRANSMOVE, endAction);
   
        }
        else
        {
            pAction = Acting(Target, ref posinfor, ACTIONID.MOVE, endAction);
        }
        pAction.m_pActionETC = endAction;
        pAction.LimitTime = 3.0f;
        pAction.ActionSetup(Target, 1.0f, ref posinfor.pt, posinfor.vs);
        Target.OnAnimation("Push", 1.0f);


        return pAction;
    }
    public BaseAction Acting( Character Target, ref ST_POS_INFOR posinfor, ACTIONID actID
        , ACTIONID actMsg = ACTIONID.END)
    {
        BaseAction pAction = null;
        switch( actID )
        {
            case ACTIONID.MOVE:
                pAction = new MoveAction();
                pAction.ActionSetup(Target, 0.4f, ref posinfor.pt, posinfor.vs);
                Target.OnAnimation("Walk", 0.4f); 
                break;
            case ACTIONID.TRANSMOVE:
                pAction = new TransMoveAction();
                pAction.ActionSetup(Target, 0.5f, ref posinfor.pt, posinfor.vs);
                Target.OnAnimation("Walk", 0.5f);
                break;
            case ACTIONID.BLOCK:
                pAction = new BlockedAction();
                Target.OnAnimation("Kick", 0.6f);
                pAction.ActionSetup(Target, 0.6f, ref posinfor.pt, posinfor.vs);
                break;
            case ACTIONID.PUSH:
                pAction = new PushAction();
                pAction.ActionSetup(Target, 1.0f, ref posinfor.pt, posinfor.vs);
                Target.OnAnimation("Push", 1.0f);
                break;
            case ACTIONID.WARP:
                pAction = new WarpAction();
                pAction.ActionSetup(Target, 2.0f, ref posinfor.pt, posinfor.vs);
                if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO )
                {
                    Target.OnAnimation("Box_Jump", 2.0f);
                }
                else
                {
                    Target.OnAnimation("Push", 2.0f);
                }
                break;
            case ACTIONID.DEAD:
              pAction = new DeadAction();
                pAction.ActionSetup(Target, 2.0f, ref posinfor.pt, posinfor.vs);
                Target.OnAnimation("Dead", 2.0f);
                break;
        }
        pAction.ID = actMsg;

        return pAction;
    }

    public Vector2 CalcTouchDir(Vector2 vStart, Vector2 vEnd, VIEWSTATE view)
    {
        Vector2 vDirection = vEnd - vStart;
        

        switch (view)
        {
            case VIEWSTATE.TOP:
            // 대각선을 역변환 해주는 작업
            Matrix4x4 mat =new Matrix4x4();
            mat.SetTRS( Vector3.zero, Quaternion.Euler( 0, 0, 45), Vector3.one );

            vDirection = mat.MultiplyVector(vDirection);
            break;
            case VIEWSTATE.RIGHT:
            var vTemp = vDirection.x;
            vDirection.x = -vDirection.y;
            vDirection.y = vTemp;
            break;
            case VIEWSTATE.FRONT:
            break;
        }

        return vDirection;
    }

    public GameObject GetPickingObject()
    {
        RaycastHit hit;
        bool IsTouch = false;
        Vector3 vPosition = Input.mousePosition;
        // Mobile
        if( Input.touchCount > 0 )
        {
            var touch = Input.GetTouch( 0 );
            if( touch.phase  == TouchPhase.Ended )
            {
                IsTouch = true;
                vPosition = touch.position;
            }
        }

        if (Input.GetMouseButtonUp(0) || IsTouch )
        {
            Ray ray = Camera.main.ScreenPointToRay(vPosition);


            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
    private void LoadStage( int nStage )
    {
        // Load Binary Data
        string strFileName = "Data/ST_" + nStage;

        TextAsset data
            = Resources.Load(strFileName) as TextAsset;

        Stream s = new MemoryStream(data.bytes);

        int nCount = 0;
        var bf = new BinaryFormatter();
        string str = bf.Deserialize(s).ToString();


        // Data Setting - Player & Block
        m_pBlockCtrl.CreateBlock(VIEWSTATE.MAX, m_nMaxIndex, m_fBlockSize);
        string strPlayerLoadName = "02. InGame/01. Model/02. Player/Object";
        if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO )
        {
            strPlayerLoadName = "02. InGame/01. Model/02. Player/Object_Block";
        }
        m_pPlayer =
            ObjectMgr.Instance.CreateObject<Player>( strPlayerLoadName);

        ExplainLoad();
        PlayerSetting(ref str, ref nCount);
        m_pBlockCtrl.BlockSetting(ref str, ref nCount);

        s.Close();
    }

    public void NotSceneChLoadStage( )
    {

        string strFileName = "Data/ST_" + StageMgr.Instance.SellectStage;

        TextAsset data
            = Resources.Load(strFileName) as TextAsset;

        Stream s = new MemoryStream(data.bytes);

        int nCount = 0;
        var bf = new BinaryFormatter();
        string str = bf.Deserialize(s).ToString();

        ExplainLoad();

        PlayerSetting(ref str, ref nCount);
        m_pBlockCtrl.BlockSetting(ref str, ref nCount);

        m_IsDie = false;
        m_IsPause = false;
        m_pPlayer.OnIdleState();
        m_pPlayer.Action = null;
        m_pPlayer.Ability = null;
        m_pPlayer.gameObject.transform.localScale = Vector3.one;
        m_pPlayer.ResetUndo();
    }

    private void ExplainLoad()
    {
        if (m_pExplainObject == null)
        {
            GameObject go = GameObject.Find("ExPlain");
            if (go != null)
            {
                m_pExplainObject = go.GetComponent<Explain>();
            }
        }
        if (m_pExplainObject != null)
        {
            m_pExplainObject.Initialize();
        }
    }
    private void PlayerSetting( ref string str, ref int nCount)
    {


        m_pPlayer.Ability = null;
        m_pPlayer.Action = null;
        ST_POINT pt;
        pt.x = DataMgr.Instance.GetShortToken(ref str, ref nCount); // Start X
        pt.y = DataMgr.Instance.GetShortToken(ref str, ref nCount); // Start Y
        VIEWSTATE vs = (VIEWSTATE)
        DataMgr.Instance.GetIntegerToken(ref str, ref nCount); // Start View

        InGameController.Instance.Player.PositionSetup(ref pt, vs);


        DataMgr.Instance.GetStringToken(ref str, ref nCount); // Start Color
        OnBackStEnable();
    }

    private void OnBackStEnable()
    {
        if (m_pBackStRenderer != null)
        {
            if (m_pBackStRenderer.gameObject.GetComponent<AlphaSprite>() == null)
            {
                m_pBackStRenderer.gameObject.AddComponent<AlphaSprite>();
            }
            var mtl = m_pBlockCtrl.MaterialBlack;
            switch (StageMgr.Instance.SellectChapter)
            {
                case 1:
                    m_pBackStRenderer.sprite = Resources.Load("Sky Blue", typeof( Sprite ) ) as Sprite;
                    mtl.color = new Color( 0.54f, 0.95f, 0.99f );
                    break;
                case 2:
                    m_pBackStRenderer.sprite = Resources.Load("Blue", typeof(Sprite)) as Sprite;
                    mtl.color = new Color(0.47f, 0.67f, 1.0f);
                    break;
                case 3:
                    m_pBackStRenderer.sprite = Resources.Load("Purple", typeof(Sprite)) as Sprite;
                    mtl.color = new Color(0.64f, 0.58f, 0.98f);
                    break;
                case 4:
                    m_pBackStRenderer.sprite = Resources.Load("Orange", typeof(Sprite)) as Sprite;
                    mtl.color = new Color(0.99f, 0.58f, 0.47f);
                    break;
                case 5:
                    m_pBackStRenderer.sprite = Resources.Load("Green", typeof(Sprite)) as Sprite;
                    mtl.color = new Color( 0.55f, 1.0f, 0.83f );
                    break;
            }
            if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO )
            {
                mtl.color = new Color(0.9f, 0.9f, 0.9f);
            }
            m_pBlockCtrl.MaterialBlack = mtl;
        }
    }

    public void SetUnActiveBackSt()
    {
        if (m_pBackStRenderer != null)
        {
            var Alpha = m_pBackStRenderer.gameObject.GetComponent<AlphaSprite>();
            if (Alpha == null)
            {
                Alpha = m_pBackStRenderer.gameObject.AddComponent<AlphaSprite>();
            }

            Alpha.m_fPlus = -4.0f;
            Alpha.m_fStartAlpha = 1.0f;
        }
    }
    public Vector3 CalcUpVec(VIEWSTATE vs)
    {
        Vector3 vReturn = Vector3.zero;
        switch (vs)
        {
            case VIEWSTATE.TOP:
                vReturn = new Vector3(0, 1, 0);
                break;
            case VIEWSTATE.FRONT:
                vReturn = new Vector3(1, 0, 0);
                break;
            case VIEWSTATE.RIGHT:
                vReturn = new Vector3(0, 0, 1);
                break;
        }
        return vReturn;
    }
    
    
	// Update is called once per frame
	void Update () {
        if( m_pMoveCount == null )
        {
            m_pMoveCount = GameObject.Find("Move_Count_Number").GetComponent<ShowNumber>();
        }
        if( m_pMoveCount != null )
        {
            m_pMoveCount.Draw(m_pPlayer.MoveCount);
        }
	}
}
