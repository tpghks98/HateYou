using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    
    public Vector3 m_vStartPosition = Vector3.zero;
    public Vector3 m_vEndPosition = Vector3.zero;
    private float m_fTime = 0.0f;
    public float m_fMaxTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
        m_fTime += Time.deltaTime;
        transform.position =
            Vector3.Lerp(m_vStartPosition, m_vEndPosition, m_fTime / m_fMaxTime);
        if( m_fTime >= m_fMaxTime )
        {
            Destroy(GetComponent<Move>());
        }
	}
}
