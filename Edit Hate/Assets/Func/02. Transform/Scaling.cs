using UnityEngine;
using System.Collections;

public class Scaling : MonoBehaviour {
    public Vector3 m_vStartScale;
    public Vector3 m_vEndScale;
    public float m_fMaxTime;
    public float m_fDelayTime;
    public bool m_IsUI = false;
    public bool m_IsEasing = true;
    public bool m_IsFixedTime = false;

    private float m_fCurrTime = 0.0f;

    void Start()
    {
        if( m_IsUI )
        {
            m_vStartScale =
                new Vector3( SceneMgr.Instance.StartScale,
                    SceneMgr.Instance.StartScale,
                    SceneMgr.Instance.StartScale );
        }
    }
    void Update()
    {
        float fDeltaTime = Time.deltaTime;
        if( m_IsFixedTime )
        {
            fDeltaTime = Time.fixedDeltaTime;
        }
        if (m_fDelayTime > 0.0f)
        {
            transform.localScale = Vector3.zero;
            m_fDelayTime -= fDeltaTime;

        }
        else
        {
            m_fCurrTime += fDeltaTime;
            if (m_fCurrTime > m_fMaxTime)
            {
                m_fCurrTime = m_fMaxTime;
                Destroy(GetComponent<Scaling>());
            }

            float fTime = (m_fCurrTime / m_fMaxTime) ;
            if( m_IsEasing )
            {
                fTime *=
                    EasingUtil.easeInOutBack(0.0f, 1.0f, fTime);
            }
            transform.localScale =
                Vector3.Lerp(m_vStartScale, m_vEndScale, fTime );
        }
    }
}
