using UnityEngine;
using System.Collections;

public class WarpAction : BaseAction {
    private Vector3 m_vOriScale;
    private Vector3 m_vStartPosition;
    private Vector3 m_vEndPosition;

    protected override void Setup()
    {
        m_vOriScale = m_pTarget.transform.localScale;
        m_vStartPosition = m_pTarget.transform.position;
        m_vEndPosition = m_vStartPosition
            + InGameController.Instance.CalcUpVec(m_pTarget.View) * 3.0f;        
 
    }

    protected override void ActionUpdate()
    {
        Vector3 vStartPosition = m_vOriScale;
        ST_POINT ptStart = m_pTarget.Point;


        float fCurrTime = m_fTime / m_fMaxTime;


        if (fCurrTime > 0.8f)
        {
            m_pTarget.transform.position =
                Vector3.Lerp(m_vStartPosition, m_vEndPosition, fCurrTime / 0.2f);
        }
        else if (fCurrTime > 0.6f)
        {
            m_pTarget.transform.localScale =
                Vector3.Lerp(Vector3.zero, m_vOriScale, (fCurrTime - 0.6f) / 0.2f);
        }
        else if (fCurrTime < 0.4f)
        {
            m_pTarget.transform.localScale =
                Vector3.Lerp(m_vOriScale, Vector3.zero, ( fCurrTime - 0.2f ) / 0.2f);
        }
        else if (fCurrTime < 0.2f)
        {
            m_pTarget.transform.position =
                Vector3.Lerp(m_vStartPosition, m_vEndPosition, fCurrTime / 0.2f);
        }
        else
        {
            Vector3 vArrivePosition;


            InGameController.Instance.GetBlockCtrl().
                GetBlockOnPos(out vArrivePosition, ref m_stArrivePoint, m_vsArriveView, true);

            m_pTarget.transform.position = vArrivePosition;
            m_vStartPosition = m_pTarget.transform.position
                + InGameController.Instance.CalcUpVec(m_vsArriveView) * 3.0f;
            m_vEndPosition = m_pTarget.transform.position;

            ST_POS_INFOR ps;
            ps.pt = m_pTarget.Point;
            ps.vs = m_pTarget.View;
            InGameController.Instance.GetBlockCtrl().GetItem(ref ps).Scaling();

            ps.pt = m_stArrivePoint;
            ps.vs = m_vsArriveView;
            InGameController.Instance.GetBlockCtrl().GetItem(ref ps).Scaling();
            m_pTarget.StartRotation( float.MaxValue);
            m_pTarget.Up = InGameController.Instance.CalcUpVec(ps.vs);

        }
    }
    protected override void ActionEnd()
    {

        ST_POS_INFOR ps;

        ps.pt = m_pTarget.Point;
        ps.vs = m_pTarget.View;


        m_pTarget.SetUndoItemData(ref ps, ITEMID.WARP, 2, 0);

        InGameController.Instance.GetBlockCtrl().DestroyItem(ref ps);

        m_pTarget.Point = m_stArrivePoint;
        m_pTarget.View = m_vsArriveView;

        ps.pt = m_stArrivePoint;
        ps.vs = m_vsArriveView;

        m_pTarget.SetUndoItemData(ref ps, ITEMID.WARP, 0, 1);



        InGameController.Instance.GetBlockCtrl().DestroyItem(ref ps);
        InGameController.Instance.IsOnWarp = false;
        m_pTarget.Ability = null;

    }
}
