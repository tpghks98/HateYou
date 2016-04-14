using UnityEngine;
using System.Collections;

public class PushAction : BaseAction {

    private Vector3 m_vStartPosition;

    protected override void Setup()
    {
        m_vStartPosition = m_pTarget.transform.position;
    }

    protected override void ActionUpdate()
    {
        Vector3 vArrivePosition;
        Vector3 vStartPosition = m_vStartPosition;
        ST_POINT ptStart = m_pTarget.Point;

        InGameController.Instance.GetBlockCtrl().
            GetBlockOnPos(out vArrivePosition, ref m_stArrivePoint, m_vsArriveView);

        m_pTarget.transform.position
            = Vector3.Lerp(vStartPosition, vArrivePosition, (m_fTime / m_fMaxTime) * 2.0f);
        /*
        if ( (m_fTime / m_fMaxTime) > 0.5f)
        {
            ST_POS_INFOR ps;
            ps.pt = m_stArrivePoint;
            ps.vs = m_vsArriveView;

            
            var pItem =
                InGameController.Instance.GetBlockCtrl().GetItem(ref ps);
      
        }
         */
    }
    protected override void ActionEnd()
    {
        m_pTarget.Point = m_stArrivePoint;
        m_pTarget.View = m_vsArriveView;

        ST_POS_INFOR ps;
        ps.pt = m_stArrivePoint;
        ps.vs = m_vsArriveView;

        var pItem = InGameController.Instance.GetBlockCtrl().GetItem( ref ps ) ;
        if( pItem != null )
        {
            Debug.Log("Y2");
            m_pTarget.SetUndoItemData(ref ps, pItem.ID, 1, 0);
            InGameController.Instance.GetBlockCtrl().DestroyItem(ref ps);

        }
    }
}
