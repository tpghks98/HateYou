using UnityEngine;
using System.Collections;

public class NullAbility : BaseAbility {
    public override void OnAbility()
    {

    }
    public override void OnIdleAnimation()
    {
        m_pTarget.OnAnimation("Jump_Idle");
    }
}
