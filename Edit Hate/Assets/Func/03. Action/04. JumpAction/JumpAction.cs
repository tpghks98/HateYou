using UnityEngine;
using System.Collections;

public class JumpAction : BaseAction {

    protected override void ActionUpdate()
    {
    }
    protected override void ActionEnd() 
    {
        m_pTarget.Point = m_stArrivePoint;
        m_pTarget.View = m_vsArriveView;
    }

}
