using UnityEngine;
using System.Collections;

public class Item : Structure{

    private ITEMID m_nID    =   ITEMID.NONE;
    private Shake m_pShake = null;
    private Shake m_pFly = null;
    private Scaling m_pScaling = null;
    private Move m_pMoving = null;
    private bool m_IsItemAbility = false;
    private Vector3 m_vStartPosition = Vector3.zero;
    // Property

    public ITEMID ID
    {
        get { return m_nID;  }
        set { m_nID = value; }
    }

    public override void Initialize()
    {
    }
    public void Start()
    {
        m_vStartPosition = transform.position;

    }

    public void Shaking()
    {
        if (m_pShake == null)
        {
            m_pShake = gameObject.AddComponent<Shake>();
            m_pShake.m_vDirection = Vector3.left;
        }
    }
    public void Scaling()
    {
        if( m_pScaling == null )
        {
            m_pScaling = gameObject.AddComponent<Scaling>();
            m_pScaling.m_vStartScale = transform.localScale;
            m_pScaling.m_vEndScale = Vector3.zero;
            m_pScaling.m_fMaxTime = 0.5f;
        }
    }
    public void Moving( Vector3 vEndPosition, float fTime)
    {
        if( m_pMoving != null )
        {
            m_pMoving.m_fMaxTime = fTime;
            m_pMoving.m_vStartPosition = transform.position;
            m_pMoving.m_vEndPosition = vEndPosition;
        }
        else
        {
            m_pMoving = gameObject.AddComponent<Move>();
            m_pMoving.m_fMaxTime = fTime;
            m_pMoving.m_vStartPosition = transform.position;
            m_pMoving.m_vEndPosition = vEndPosition;
        }
    }
    public void Flying()
    {
        if( m_pFly == null)
        {
            m_pFly = gameObject.AddComponent<Shake>();
            m_pFly.m_vDirection = Vector3.forward * 0.25f;
            m_pFly.m_fTickTime = 0.5f;
            m_pFly.m_IsLoop = true;
        }

    }
	// Update is called once per frame
	void Update () {
        if (m_nID == ITEMID.WARP)
        {
            if (InGameController.Instance.IsOnWarp  )
            {
                ST_POS_INFOR ps;
                ps.vs = InGameController.Instance.Player.View;
                ps.pt = InGameController.Instance.Player.Point;

                if (InGameController.Instance.Player.Action != null)
                {
                    ps.vs = InGameController.Instance.Player.Action.m_vsArriveView;
                    ps.pt = InGameController.Instance.Player.Action.m_stArrivePoint;
                }
                if (ps.vs == m_vsView &&
                   ps.pt == m_vPosition)
                {
                    if (!m_IsItemAbility)
                    {
                        m_IsItemAbility = true;
                        Moving(m_vStartPosition +
                            InGameController.Instance.CalcUpVec(m_vsView)
                            * 3.0f, 0.35f);
                        Flying();
                    }
                }
                else
                {
                    Flying();
                    transform.rotation = 
                        Quaternion.Euler( transform.rotation.eulerAngles
                         + Vector3.forward * Time.deltaTime
                         * 360.0f);
                   // transform.position = m_vStartPosition;
                }
            }
            else
            {
                if( m_pFly != null )
                {
                    Destroy(m_pFly);
                }
                if (m_IsItemAbility)
                {
                    m_IsItemAbility = false;
                    Moving(m_vStartPosition, 0.35f);
                }
            }
        }
     
	}
}
