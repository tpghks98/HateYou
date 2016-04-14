using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Alpha : MonoBehaviour {

    public float m_fStartAlpha = 0.0f;
    public float m_fEndAlpha = 0.0f;
    public float m_fMaxTime = 0.0f;
    public float m_fDelayTime = 0.0f;
    public Image m_pTargetImage = null;
    public bool m_IsFixedTime = false;

    private float m_fCurrTime = 0.0f;
    public bool m_IsReturnValue = false;
    public bool m_IsRepeat = false;
    private int m_nMultiplyNum = 1;

    void Start()
    {
        if (m_pTargetImage == null)
        {
            m_pTargetImage = GetComponent<Image>();
        }    
        if( m_pTargetImage != null )
        {
            var color = m_pTargetImage.color;
            color.a = m_fStartAlpha;
            m_pTargetImage.color = color;
        }
    }
	
	// Update is called once per frame
	void Update () {
        float fDeltaTime = Time.deltaTime * m_nMultiplyNum;
        if (m_IsFixedTime)
        {
            fDeltaTime = Time.fixedDeltaTime;
        }
        if (m_fDelayTime > 0.0f)
        {
            m_fDelayTime -= fDeltaTime;
        }
        else
        {
            if (m_fCurrTime != m_fMaxTime)
            {
                m_fCurrTime += fDeltaTime;
                if (m_fCurrTime > m_fMaxTime)
                {
                    m_fCurrTime = m_fMaxTime;
                    if( m_IsReturnValue )
                    {
                        m_fCurrTime = m_fMaxTime - 0.001f;
                        m_nMultiplyNum = -1;
                    }
                }
                else if( m_IsRepeat && m_fCurrTime < 0.0f)
                {
                    m_nMultiplyNum = 1;
                    m_fCurrTime = 0.0f;
                }

                if( m_pTargetImage == null )
                {
                    m_pTargetImage = GetComponent<Image>();
                }
                Color color = m_pTargetImage.color;
                color.a = Mathf.Lerp(m_fStartAlpha, m_fEndAlpha, m_fCurrTime / m_fMaxTime);

                m_pTargetImage.color = color;


            }
        }
	}
}
