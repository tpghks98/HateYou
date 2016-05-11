using UnityEngine;
using System.Collections;

public class BlockedAction : BaseAction {

    private bool m_IsKicked = false;
    protected override void ActionUpdate()
    {
        if (!m_IsKicked)
        {
            if (m_fTime / m_fMaxTime > 0.5f)
            {
                m_IsKicked = true;
                ST_POS_INFOR posinfor;
                posinfor.pt = m_pTarget.Point;
                posinfor.vs = m_pTarget.View;

                
                InGameController.Instance.GetBlockCtrl().GetItem(ref posinfor).Shaking();
                SoundMgr.Instance.CreateSound("Kick");

                var temp = m_pTarget.PrevInfo;
                var pItem = InGameController.Instance.GetBlockCtrl().GetItem(ref temp);

                bool IsOnBlock = true;
                if (pItem != null)
                {
                    if (pItem.ID == ITEMID.WARP)
                    {
                        m_pTarget.Ability = new WarpAbility();
                        m_pTarget.Ability.Setup(m_pTarget);
                        InGameController.Instance.IsOnWarp = true;
                        IsOnBlock = false;
                    }

                }

                if( IsOnBlock )
                {
                    InGameController.Instance.GetBlockCtrl().OnBlock(ref temp);
                }
            }
        }
    }
    protected override void ActionEnd()
    {
        ST_POS_INFOR posinfor;
        posinfor.pt = m_pTarget.Point;
        posinfor.vs = m_pTarget.View;

        var pItem =
            InGameController.Instance.GetBlockCtrl().GetItem(ref posinfor);
        if (pItem != null )
        {
            if( pItem.ID == ITEMID.COIN)
            {
                m_pTarget.SetUndoItemData(ref posinfor, ITEMID.COIN, 1, 0);

                InGameController.Instance.GetBlockCtrl()
                    .DestroyItem(ref posinfor);
            }
            else if( pItem.ID == ITEMID.WALL)
            {
                m_pTarget.SetUndoItemData(ref posinfor, ITEMID.WALL, 1, 0);
            }
        }
        posinfor.pt = m_stArrivePoint;
        posinfor.vs = m_vsArriveView;

    }
}
