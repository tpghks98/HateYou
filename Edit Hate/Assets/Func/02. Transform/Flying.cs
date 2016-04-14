using UnityEngine;
using System.Collections;

public class Flying : MonoBehaviour {

    public Vector3 m_vOriPosition;
    public Vector3 m_vEndPosition = Vector3.zero;

    private short m_nMul = 1;
    private float m_fGuage = 0.0f;
	void Start () {
        m_vOriPosition = transform.position;
        m_vEndPosition = transform.position - Vector3.up * 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        m_fGuage += Time.deltaTime * m_nMul * Random.Range(0.25f, 1.00f);
        if( m_fGuage < 0.0f )
        {
            m_nMul *= -1;
            m_fGuage = 0.0f;
        }
        else if( m_fGuage > 1.0f )
        {
            m_nMul *= -1;
            m_fGuage = 1.0f;
        }
        transform.position = Vector3.Lerp(m_vOriPosition, m_vEndPosition, 
            m_fGuage);
	}
}
