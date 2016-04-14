using UnityEngine;
using System.Collections;

public class DeadAction : BaseAction {

    private Vector3 m_vStartPosition;

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
            GetBlockOnPos(out vArrivePosition, ref m_stArrivePoint, m_vsArriveView);

        m_pTarget.transform.position 
            =  Vector3.Lerp(vStartPosition, vArrivePosition, ( m_fTime / m_fMaxTime ) * m_fLimitTime);
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

}
