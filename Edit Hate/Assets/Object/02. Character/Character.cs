using UnityEngine;
using System.Collections;

public abstract class Character : ObjBase {

    protected Vector3 m_vLookDirection = Vector3.zero;
    protected float m_fLookTime = 0.0f;

    protected BaseAction m_pAction = null;
    protected BaseAbility m_pAbility = null;

    protected Animation m_pAnimation = null;
    protected Vector3 m_vUp = new Vector3( 0, 1, 0 );
    protected ST_POS_INFOR m_stPrevInfo;
    protected int m_nMoveCount = 0;
    protected float m_fRestTime = 0.0f;

    protected ST_UNDO_DATA m_stUndoData;

    // property
    public ST_POS_INFOR PrevInfo
    {
        get { return m_stPrevInfo; }
    }
    public Vector3 LookDir
    {
        get { return m_vLookDirection; }
        set { m_vLookDirection = value; }
    }
    public BaseAbility Ability
    {
        get { return m_pAbility; }
        set { m_pAbility = value;  }
    }
    public BaseAction Action
    {
        get { return m_pAction; }
        set { m_pAction = value; }
    }
    public Vector3 Up
    {
        get { return m_vUp; }
        set { m_vUp = value; }
    }
    public int MoveCount
    {
        get { return m_nMoveCount; }
        set { m_nMoveCount = value; }
    }

    public ST_ITEM_DATA[] UndoItemData
    {
        get { return m_stUndoData.stItemData; }
        set { m_stUndoData.stItemData = value; }
    }

    public void ResetUndo()
    {
        m_stUndoData.IsCanUse = false;
        m_stUndoData.IsColorChange = false;
        m_stUndoData.pPrevAbility = null;
        m_stUndoData.stItemData = null;
    }
    public void SetUndoItemData( ref ST_POS_INFOR ps, ITEMID ID, int nResetCount, int nCount )
    {
        if( nResetCount > 0 )
        {
            m_stUndoData.stItemData = null;
            m_stUndoData.stItemData = new ST_ITEM_DATA[nResetCount];
        }

        m_stUndoData.stItemData[nCount].nID = ID;
        m_stUndoData.stItemData[nCount].stPosInfo = ps;
    }
    public void MoveCountUp()
    {
        --m_nMoveCount;
        if( m_nMoveCount <= 0 )
        {
            m_nMoveCount = 0;
        }
        m_stUndoData.stPrev.pt = m_vPosition;
        m_stUndoData.stPrev.vs = m_vsView;
        m_stUndoData.IsCanUse = true;
        m_stUndoData.IsColorChange = false;
        m_stUndoData.stItemData = null;
        m_stUndoData.pPrevAbility = null;



    }

    public bool GetCanUndo()
    {
        return m_stUndoData.IsCanUse;
    }
    private void UndoMoveCount()
    {
        ++m_nMoveCount;
    }

    public void Undo()
    {
        if (m_pAction == null)
        {
            if (m_stUndoData.IsCanUse)
            {
                m_stUndoData.IsCanUse = false;

                if( m_stUndoData.stItemData != null )
                {
                    for( int n =0; n < m_stUndoData.stItemData.Length; ++n )
                    {
                        switch( m_stUndoData.stItemData[n].nID )
                        {
                            case ITEMID.WARP:
                                InGameController.Instance.IsOnWarp = true;
                                break;
                            case ITEMID.COIN:
                                m_stUndoData.IsColorChange = true;
                                break;

                        }
                        InGameController.Instance.GetBlockCtrl().CreateItem(
                            (int)m_stUndoData.stItemData[n].stPosInfo.vs,
                            m_stUndoData.stItemData[n].stPosInfo.pt.y,
                            m_stUndoData.stItemData[n].stPosInfo.pt.x,
                            m_stUndoData.stItemData[n].nID);
                    }
                    m_stUndoData.stItemData = null;
                }

                if (m_stUndoData.IsColorChange)
                {
                    ST_POS_INFOR ps;
                    ps.pt = m_vPosition;
                    ps.vs = m_vsView;
                    InGameController.Instance.GetBlockCtrl().OnBlock(ref ps);
                }

                var pItem =
                    InGameController.Instance.GetBlockCtrl()
                    .GetItem(ref m_stUndoData.stPrev);


                m_pAbility = null;

                if (pItem != null )
                {
                    if( pItem.ID == ITEMID.WARP )
                    {
                        m_pAbility = new WarpAbility();
                        m_pAbility.Setup(this);
                        InGameController.Instance.IsOnWarp = true;
                    }
                }


                if( m_stUndoData.pPrevAbility != null )
                {
                    m_pAbility = m_stUndoData.pPrevAbility;
                }
                m_vPosition = m_stUndoData.stPrev.pt;
                m_vsView = m_stUndoData.stPrev.vs;
                PositionSetup(ref m_vPosition, m_vsView);
                UndoMoveCount();

                m_stUndoData.pPrevAbility = null;
                OnIdleState();
            }
        }
    }

    public virtual void OnAnimation( string strName, float fTime = 0.0f )
    {
        if( m_pAnimation == null )
        {
            m_pAnimation = GetComponent<Animation>();
            if( m_pAnimation == null )
            {
                m_pAnimation = GetComponentInChildren<Animation>();
            }
        }
        if (m_pAnimation != null)
        {
            if (StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO)
            {
                if( strName == "Idle" ||
                    strName == "Jump_Idle" ||
                    strName == "Warp_Idle" )
                {
                    return;
                }
                transform.localScale = Vector3.one;

                if( strName == "Push" )
                {
                    strName = "Box_Null";
                    transform.localScale = Vector3.one * 2.54f;

                }
                else if( strName == "Kick")
                {
                    strName = "Box_Wall";
                    transform.localScale = Vector3.one * 2.54f;
                }

                else{
                      strName = "Box_Jump";
                }
            }
 
            if (fTime != 0.0f)
            {
                var v = m_pAnimation[strName];
                v.speed = v.length / (fTime - 0.05f);
            }
            m_pAnimation.Play(strName);
        }
    }
    public virtual void OnActionEnd(ACTIONID actID)
    {
       // m_fRestTime = 0.1f;
        ST_POS_INFOR ps;
        ps.pt = m_vPosition;
        ps.vs = m_vsView;
        switch( actID )
        {
            case ACTIONID.END:
                OnBlock(ref ps);
                return;
            case ACTIONID.BLOCK:
                ACTIONID act;
                if (m_vsView == m_stPrevInfo.vs)
                {
                    act = ACTIONID.MOVE;
                }
                else
                {
                    act = ACTIONID.TRANSMOVE;
                }
                m_pAction = 
                    InGameController.Instance.
                    Acting(this, ref ps, ACTIONID.BLOCK, act);
                break;
            case ACTIONID.MOVE:
                m_pAction =
                    InGameController.Instance.
                    Acting(this, ref m_stPrevInfo, ACTIONID.MOVE, ACTIONID.NONE);
                break;
            case ACTIONID.TRANSMOVE:
                m_pAction =
                    InGameController.Instance.
                    Acting(this, ref m_stPrevInfo, ACTIONID.TRANSMOVE, ACTIONID.NONE);
                break;
            case ACTIONID.NULL:
                OnBlock(ref ps);
                break;
            case ACTIONID.GETJUMP:
                m_pAbility = new JumpAbility();
                m_pAbility.Setup(this);
                OnBlock( ref ps);
                break;
            case ACTIONID.NONE:
                OnIdleState();
                break;
            case ACTIONID.DEAD:
                m_pAction =
                    InGameController.Instance.
                    Acting(this, ref ps, ACTIONID.DEAD, ACTIONID.GAMEOVER);
                break;
            case ACTIONID.GAMEOVER:
                InGameController.Instance.GameOver();
                break;
        }
        ActionStart();
    }

    private void OnBlock( ref ST_POS_INFOR ps )
    {
        bool IsColorChange = true;
        if (m_pAbility != null)
        {
            switch( m_pAbility.GetType().ToString() )
            {
                case "NullAbility":
                IsColorChange = false;
                m_pAbility = null;
                if (IsItem( ref ps ))
                {
                    if ( InGameController.Instance.GetBlockCtrl().GetItem( ref ps ).ID
                        == ITEMID.NULL )
                    {
                        SetUndoItemData(ref ps, ITEMID.NULL, 1, 0);
                        InGameController.Instance.GetBlockCtrl().DestroyItem(ref ps);
                    }
                }
                break;
                case "JumpAbility":
                if (IsItem(ref ps))
                {
                    if (InGameController.Instance.GetBlockCtrl().GetItem(ref ps).ID
                        == ITEMID.JUMP)
                    {
                        SetUndoItemData(ref ps, ITEMID.JUMP, 1, 0);
                        InGameController.Instance.GetBlockCtrl().DestroyItem(ref ps);
                    }
                }
                break;
                case "WarpAbility":
                IsColorChange = false;
                break;
            }

        }
        if( IsColorChange )
        {
            m_stUndoData.IsColorChange = true;
            InGameController.Instance.GetBlockCtrl().OnBlock(ref ps);
        }

        OnIdleState();
    }
    public virtual void Move( ref Vector2 vDirection )
    {
        int nSize = InGameController.Instance.GetBlockCtrl().BlockSize;


        Vector2 vLookDir = vDirection;
 
        if (Mathf.Abs(vLookDir.x) > Mathf.Abs(vLookDir.y))
        {
            vLookDir.y = 0.0f;
        }
        else
        {
            vLookDir.x = 0.0f;
        }
        vLookDir.Normalize();


        ST_POS_INFOR posinfor;

        int nJump = 1;
        if( m_pAbility != null )
        {
            if( m_pAbility.GetType()  == typeof( JumpAbility ) )
            {
                nJump = 2;
                m_stUndoData.pPrevAbility = m_pAbility;
                m_pAbility = null;
            }
        }
        posinfor.pt = m_vPosition;
        posinfor.pt.x += (short)(vLookDir.x * nJump);
        posinfor.pt.y += (short)(vLookDir.y * nJump);
        posinfor.vs = m_vsView;


        bool IsMove = false;
        bool IsTrans = false;
        if (posinfor.pt.x > (nSize - 1) || posinfor.pt.x < 0
            || posinfor.pt.y > (nSize - 1) || posinfor.pt.y < 0)
        {
            IsTrans = true;
            CalcArrivePoint(ref posinfor, ref vLookDir);
        }

        if (m_pAction == null)
        {
            if (m_fRestTime <= 0.0f)
            {
                m_pAction = InGameController.Instance.GetAction(this, ref posinfor);
                ActionStart();
                IsMove = true;
                MoveCountUp();
            }
        }

        if (IsMove)
        {
            m_stPrevInfo.vs = m_vsView;
            m_stPrevInfo.pt = m_vPosition;
            m_vLookDirection = new Vector3(-vLookDir.y, 0.0f, vLookDir.x);

            StartRotation(8.0f);
            if (nJump >= 2)
            {
                float fAniTime = 0.75f;
                if (IsTrans)
                {
                    fAniTime = 1.0f;
                }
                bool IsJumpAni = true;
                if (m_pAbility != null)
                {
                    if (m_pAbility.GetType() == typeof(NullAbility))
                    {
                        IsJumpAni = false;
                    }
                }
                else if (m_pAction.ID == ACTIONID.GETJUMP )
                {
                    IsJumpAni = false;
                }
                if (IsJumpAni)
                {
                    OnAnimation("Jump", fAniTime);
                    m_pAction.ActionSetup(this, fAniTime,
                        ref posinfor.pt, posinfor.vs);
                }
            }
        }
    }
    private bool IsItem(ref ST_POS_INFOR ps)
    {
        if (InGameController.Instance.GetBlockCtrl().GetItem(ref ps) != null)
        {
            return true;
        }
        return false;
    }
    public void ActionStart()
    {
        StartCoroutine(ActionUpdate());
    }
    public void StartRotation( float fValue = 4.0f)
    {
        m_fLookTime = 0.0f;

        if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO  && fValue == 8.0f)
        {
            fValue = float.MaxValue;
        }
        StartCoroutine(LookAtRotSlerp(fValue)); 
    }
    public virtual void PositionSetup( ref ST_POINT pt , VIEWSTATE view )
    {
        Vector3 vPosition;

        InGameController.Instance.GetBlockCtrl().
            GetBlockOnPos( out vPosition,  ref pt, view ,true );

        transform.position = vPosition;
        m_vPosition = pt;
        m_vsView = view;

        m_vUp = InGameController.Instance.CalcUpVec(view);

        m_vLookDirection = new Vector3(1, 0, 0);
        StartRotation(float.MaxValue);
    }
    public  void PositionSetup()
    {
        Vector3 vPosition;

        InGameController.Instance.GetBlockCtrl().
            GetBlockOnPos(out vPosition, ref m_vPosition, m_vsView, true);

        transform.position = vPosition;

        m_vUp = InGameController.Instance.CalcUpVec(m_vsView);

        m_vLookDirection = new Vector3(1, 0, 0);
        StartRotation(float.MaxValue);
    }

    private void CalcArrivePoint(ref ST_POS_INFOR posinfor, ref Vector2 vMove)
    {
        int nSize = InGameController.Instance.GetBlockCtrl().BlockSize - 1;
        int nOriSize = InGameController.Instance.GetBlockCtrl().BlockSize;
        switch( m_vsView )
        {
            case VIEWSTATE.TOP:
                if (posinfor.pt.x > 4)
                {
                    posinfor.vs = VIEWSTATE.RIGHT;
                    posinfor.pt.x = (short)(posinfor.pt.x - nOriSize);
                //    m_vUp = new Vector3(0, 0, 1);
                }
                else if (posinfor.pt.y < 0)
                {
                    posinfor.vs = VIEWSTATE.FRONT;
                    posinfor.pt.y = (short)(nOriSize + posinfor.pt.y);
                 //   m_vUp = new Vector3(1, 0, 0);
                }
                else
                {
                    posinfor.pt = m_vPosition;
                    posinfor.vs = m_vsView;
                }
                break;
            case VIEWSTATE.FRONT:
                if( posinfor.pt.y > 4)
                {
                    posinfor.vs = VIEWSTATE.TOP;
                    posinfor.pt.y = (short)( posinfor.pt.y - nOriSize );
             //       m_vUp = new Vector3(0, 1, 0);
                }
                else if( posinfor.pt.x > 4 )
                {
                    posinfor.vs = VIEWSTATE.RIGHT;
                    short nTemp = posinfor.pt.x;
                    posinfor.pt.x = (short)( nSize - posinfor.pt.y );
                    posinfor.pt.y = (short)(nTemp - nOriSize);
               /*     m_vUp = new Vector3(0, 0, 1);
                    float fTemp = vMove.x;
                    vMove.x = vMove.y;
                    vMove.y = fTemp;
                * */
                }
                else
                {
                    posinfor.pt = m_vPosition;
                    posinfor.vs = m_vsView;
                }
                break;
            case VIEWSTATE.RIGHT:
                if (posinfor.pt.x < 0)
                {
                    posinfor.vs = VIEWSTATE.TOP;
                    posinfor.pt.x = (short)( nOriSize + posinfor.pt.x );
              //      m_vUp = new Vector3(0, 1, 0);
                }
                else if (posinfor.pt.y < 0)
                {
                    posinfor.vs = VIEWSTATE.FRONT;
                    short nTemp = posinfor.pt.y;
                    posinfor.pt.y = (short)(nSize - posinfor.pt.x);
                    posinfor.pt.x = (short)( nOriSize + nTemp );
            /*        m_vUp = new Vector3(1, 0, 0);
                    float fTemp = vMove.x;
                    vMove.x = vMove.y;
                    vMove.y = fTemp;
             * */
                }
                else
                {
                    posinfor.pt = m_vPosition;
                    posinfor.vs = m_vsView;
                }
                break;
        }

    }
    public void OnIdleState()
    {
        m_pAction = null;
        if( m_pAbility != null )
        {
            m_pAbility.OnIdleAnimation();
        }
        else
        {
            OnAnimation("Idle");
        }
        if (!InGameController.Instance.CheckGameClear())
        {
            if (m_nMoveCount <= 0)
            {
                InGameController.Instance.GameOver();
            }
        }

    }
    IEnumerator ActionUpdate()
    {
        while( true )
        {
            if( m_pAction == null )
            {
                yield break;
            }
            if( m_pAction.Update() )
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator LookAtRotSlerp( float fValue = 1.0f)
    {
        Quaternion quatPrev = transform.rotation;
        while( true )
        {
            if( m_vLookDirection == Vector3.zero )
            {
                yield break;
            }
            m_fLookTime += Time.deltaTime * fValue;
            
            Quaternion q = Quaternion.LookRotation(
                (m_vLookDirection));
            transform.up = m_vUp;
            transform.Rotate(q.eulerAngles);

            transform.rotation = Quaternion.Slerp(quatPrev, this.transform.rotation, m_fLookTime );


            if (m_fLookTime >= 1.0f)
            {
                yield break;
            }
            else
            {
                yield return  null;
            }
        }
    }
}
