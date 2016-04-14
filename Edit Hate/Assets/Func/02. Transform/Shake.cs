using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    private Vector3 m_vOriPos = Vector3.zero;
    public Vector3 m_vDirection = Vector3.zero;
    public float m_fPower = 1.0f;
    public float m_fMaxTime = 1.0f;
    public float m_fTickTime = 0.1f;
    public bool m_IsLoop = false;
    private float m_fCurrTime = 0.0f;

    void Start()
    {
        m_vOriPos = transform.position;
        StartCoroutine(Shaking( m_vDirection));
    }
	// Update is called once per frame
	void Update () {
        if (!m_IsLoop)
        {
            m_fCurrTime += Time.deltaTime;
            if (m_fCurrTime >= m_fMaxTime)
            {
                m_fCurrTime = m_fMaxTime;
                transform.position = m_vOriPos;
                Destroy(GetComponent<Shake>());
            }
        }
	}
    IEnumerator Shaking( Vector3 vDirection, float fTime = 0.0f)
    {
        while( true )
        {
            fTime += Time.deltaTime;
            transform.Translate(vDirection * Time.deltaTime * m_fPower);
            if (fTime >= m_fTickTime)
            {
                StartCoroutine(Shaking(-vDirection, m_fTickTime - fTime));
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }
}
