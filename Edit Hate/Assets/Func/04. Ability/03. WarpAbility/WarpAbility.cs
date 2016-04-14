using UnityEngine;
using System.Collections;

public class WarpAbility : BaseAbility {


    public override void OnAbility()
    {
        GameObject go = InGameController.Instance.GetPickingObject();
        if( go != null )
        {
            if( m_pTarget.Action == null )
            {
                var Item =  go.GetComponent<Item>();

                if( Item != null )
                {
                    ST_POS_INFOR posinfor;
                    posinfor.pt = Item.Point;
                    posinfor.vs = Item.View;
                    if (posinfor.pt != m_pTarget.Point ||
                        posinfor.vs != m_pTarget.View)
                    {

                        m_pTarget.Action =
                            InGameController.Instance.Acting(m_pTarget, ref posinfor,
                            ACTIONID.WARP);
                        m_pTarget.ActionStart();
                        InGameController.Instance.Player.MoveCountUp();
                    }
                }
            }
        }
    }
    public override void OnIdleAnimation()
    {
        m_pTarget.OnAnimation("Warp_Idle");
    }

}
