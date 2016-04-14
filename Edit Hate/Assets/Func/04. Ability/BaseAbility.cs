using UnityEngine;
using System.Collections;

public abstract class BaseAbility {

    protected Character m_pTarget;

    public abstract void OnAbility();
    public abstract void OnIdleAnimation(); 

    public void Setup( Character pTarget)
    {
        m_pTarget = pTarget;
    }
}
