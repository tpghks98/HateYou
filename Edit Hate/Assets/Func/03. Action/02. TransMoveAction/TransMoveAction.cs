using UnityEngine;
using System.Collections;

public class TransMoveAction : BaseAction {

    private Vector3 m_vStartPosition;
    private bool m_IsRot = false;
    private bool m_IsJump = false;

    protected override void ActionUpdate()
    {
        Vector3 vArrivePosition;
        Vector3 vStartPosition;
        ST_POINT ptStart = m_pTarget.Point;
        VIEWSTATE vs = m_pTarget.View;

        InGameController.Instance.GetBlockCtrl().
            GetBlockOnPos(out vArrivePosition, ref m_stArrivePoint, m_vsArriveView, true);

        vStartPosition = m_vStartPosition;
        
        
        // fix
        switch( vs )
        {
            case VIEWSTATE.TOP:
                vArrivePosition.y = vStartPosition.y;
                break;
            case VIEWSTATE.FRONT:
                vArrivePosition.x = vStartPosition.x;
                break;
            case VIEWSTATE.RIGHT:
                vArrivePosition.z = vStartPosition.z;
                break;
        }

        float fCurrTime = (m_fTime / m_fMaxTime) * m_fLimitTime ;

        float fDivisionTime = 0.45f;
        if( m_IsJump )
        {
            fDivisionTime = 0.3f;
        }
        if (fCurrTime < fDivisionTime)
        {
            // Move
            m_pTarget.transform.position
                = Vector3.Lerp(vStartPosition, vArrivePosition, fCurrTime / fDivisionTime);
        }
        else if (fCurrTime > fDivisionTime + 0.1f)
        {
            // Move
            if( m_IsJump )
            {
                fDivisionTime *= 2.0f;
            }
            m_pTarget.transform.position
                = Vector3.Lerp(vStartPosition, vArrivePosition, ( (fCurrTime - fDivisionTime + 0.1f) / fDivisionTime ) );
        }
        else
        {
            
            if (!m_IsRot)
            {
                m_IsRot = true;
                m_vStartPosition = m_pTarget.transform.position;
                m_pTarget.View = m_vsArriveView;
                // Rot
                m_pTarget.Up = InGameController.Instance.CalcUpVec(m_pTarget.View);

                if( m_vsArriveView == VIEWSTATE.FRONT && vs == VIEWSTATE.RIGHT ||
                    m_vsArriveView == VIEWSTATE.RIGHT && vs == VIEWSTATE.FRONT)
                {
                    var v = m_pTarget.LookDir;
                    v.y = v.x;
                    v.x = -v.z;
                    v.z = -v.y;
                    v.y = 0.0f;
                    m_pTarget.LookDir = v;
                }
                m_pTarget.StartRotation(5.0f);



            }
            
        }

        switch (m_pActionETC)
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
            InGameController.Instance.GetBlockCtrl().GetItem(ref ps);
        if (Item != null)
        {
            switch (Item.ID)
            {
                case ITEMID.COIN:
                    IsNull = true;
                    break;
                case ITEMID.WARP:
                    IsNull = true;
                    break;
            }
        }
        if (IsNull)
        {
            if (m_pTarget.Ability == null)
            {
                m_pTarget.Ability = new NullAbility();
                m_pTarget.Ability.Setup(m_pTarget);
            }
        }
    }
    protected override void Setup()
    {
        m_vStartPosition = m_pTarget.transform.position;
        m_IsRot = false;
    }


    private void ItemScaling()
    {
        if ((m_fTime / m_fMaxTime) > 0.3f)
        {
            ST_POS_INFOR ps;
            ps.pt = m_stArrivePoint;
            ps.vs = m_vsArriveView;
            var pItem = InGameController.Instance.GetBlockCtrl().GetItem(ref ps);
            pItem.Scaling();
            pItem.gameObject.transform.Translate(Vector3.back * 5.0f * Time.deltaTime);
        }
    }
}
