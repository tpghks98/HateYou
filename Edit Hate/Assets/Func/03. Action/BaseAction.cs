    using UnityEngine;
using System.Collections;

public abstract class BaseAction {

    protected Character m_pTarget;
    protected float m_fTime =   0.0f;
    protected float m_fMaxTime;
    protected float m_fLimitTime = 1.0f;
    protected ACTIONID m_ID;
    public ACTIONID m_pActionETC = ACTIONID.NONE;
    public ST_POINT m_stArrivePoint;
    public VIEWSTATE m_vsArriveView;

    public float CurrTime
    {
      get { return m_fTime / m_fMaxTime;} 
    }
    public ACTIONID ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }
    public float LimitTime
    {
        set { m_fLimitTime = value; }
    }
    public void ActionSetup(Character ch, float fMax,
        ref ST_POINT pt, VIEWSTATE vs) 
    {
        m_vsArriveView = vs;
        m_stArrivePoint = pt;
        m_pTarget = ch;
        m_fMaxTime = fMax;
        if( StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO)
        {
            m_fMaxTime += 0.05f;
        }
        Setup();
    }
    protected virtual void Setup() { }
    protected abstract void ActionUpdate();
    protected virtual void ActionEnd() { }

    public bool Update()
    {
        m_fTime += Time.deltaTime;
        ActionUpdate();
        if( m_fTime >= m_fMaxTime)
        {

             ActionEnd();
            
            m_pTarget.OnActionEnd(m_ID);
            
            m_fTime = 0.0f;
            return true;
        }
        return false;
    }
}
