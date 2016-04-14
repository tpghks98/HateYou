using UnityEngine;
using System.Collections;

public class Star_Parent : MonoBehaviour {

    public GameObject[] m_goStar;
    private Scaling m_pScaling = null;
	void Start () {
        for( int n =0 ; n < 50; ++n )
        {
            CreateStar();
        }
	
	}
    private void CreateStar()
    {
        
        (Instantiate(m_goStar[Random.Range(0, 4)]) as GameObject )
            .transform.SetParent( transform );
    }
    public void OnScaling( float fValue )
    {
        if( m_pScaling == null )
        {
            m_pScaling = gameObject.AddComponent<Scaling>();
            m_pScaling.m_fMaxTime = 1.0f;
            m_pScaling.m_vEndScale = transform.localScale + Vector3.one * fValue;
            if( m_pScaling.m_vEndScale.x > 2.0f )
            {
                m_pScaling.m_vEndScale = Vector3.one * 2.0f;
            }
            else if( m_pScaling.m_vEndScale.x < 0.5f )
            {
                m_pScaling.m_vEndScale = Vector3.one * 0.5f;
            }
            m_pScaling.m_vStartScale = transform.localScale;
            m_pScaling.m_fDelayTime = 0.0f;
            m_pScaling.m_IsEasing = false;
        }
    }
}

