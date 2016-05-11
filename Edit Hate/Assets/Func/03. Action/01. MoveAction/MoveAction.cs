using UnityEngine;
using System.Collections;

public class MoveAction : BaseAction{

    private Vector3 m_vStartPosition;
    private bool m_IsSound = false;

    protected override void Setup()
    {
        m_vStartPosition = m_pTarget.transform.position;
    }

    override protected void ActionUpdate()
    {
        Vector3 vArrivePosition;
        Vector3 vStartPosition = m_vStartPosition;
        ST_POINT ptStart = m_pTarget.Point;

        InGameController.Instance.GetBlockCtrl().
            GetBlockOnPos(out vArrivePosition, ref m_stArrivePoint, m_vsArriveView, true);

        m_pTarget.transform.position 
            =  Vector3.Lerp(vStartPosition, vArrivePosition, ( m_fTime / m_fMaxTime ) * m_fLimitTime);

        Debug.Log(m_pTarget);
        Debug.Log(m_pTarget.transform.position);
        switch( m_pActionETC )
        {
            case ACTIONID.GETJUMP:
                ItemScaling();
                break;
            case ACTIONID.NULL:
                ItemScaling();
                break;
        }

    }

    protected override void ActionEnd()
    {
        m_pTarget.Point = m_stArrivePoint;
        m_pTarget.View = m_vsArriveView;

        ST_POS_INFOR ps;
        ps.pt = m_stArrivePoint;
        ps.vs = m_vsArriveView;

        bool IsNull = false;
        var Item = 
            InGameController.Instance.GetBlockCtrl().GetItem( ref ps );
        if( Item != null )
        {
            switch( Item.ID)
            {
                case ITEMID.COIN:
               //     IsNull = true;
                    break;
                case ITEMID.WARP:
                    IsNull = true;
                    break;
            }
        }
        if( IsNull )
        {
            if (m_pTarget.Ability == null)
            {
                m_pTarget.Ability = new NullAbility();
                m_pTarget.Ability.Setup(m_pTarget);
            }
        }
    }

    private void ItemScaling()
    {
        float fTime = 0.24f;
        if( StageMgr.Instance.GetPlayType() == PLAYTYPE.COLOR )
        {
            fTime = 0.6f;
        }
        if ((m_fTime / m_fMaxTime) > fTime)
        {
            if (!m_IsSound)
            {
                m_IsSound = true;
                SoundMgr.Instance.CreateSound("Push");
            }

            ST_POS_INFOR ps;
            ps.pt = m_stArrivePoint;
            ps.vs = m_vsArriveView;
            var pItem = InGameController.Instance.GetBlockCtrl().GetItem(ref ps);
            pItem.Scaling();
            pItem.gameObject.transform.Translate(Vector3.back * 5.0f * Time.deltaTime);
        }
    }

}
