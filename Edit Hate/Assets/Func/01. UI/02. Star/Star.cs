using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
    private SpriteRenderer m_pSpriteRenderer = null;
    private float m_fAlpha = 1.0f;
    private Vector2 m_vDirection = Vector2.zero;
    private float m_fSpeed = 1.0f;
	
	void Start () {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();
        Init();
	}
    private void Init()
    {
        m_fSpeed = Random.Range(1, 10000) * 0.0001f * 5.0f;
        transform.position = new Vector3(Random.Range(-13.0f, 13.0f), Random.Range(-8.0f, 8.0f), 2.0f);
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        m_vDirection = new Vector2(Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f) * Random.Range(-1.0f, 1.0f)) * 0.5f;
        m_fAlpha = 1.0f;
    }
	
	
	void Update () {
        m_fAlpha -= Time.deltaTime * 0.01f * m_fSpeed;
        transform.Translate(m_vDirection * Time.deltaTime);
        if( m_fAlpha < 0.2f )
        {
            Init();
        }
        Color c = m_pSpriteRenderer.color;
        c.a = m_fAlpha;
        m_pSpriteRenderer.color = c;
	}
}
