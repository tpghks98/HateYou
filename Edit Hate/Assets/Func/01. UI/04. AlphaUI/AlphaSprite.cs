using UnityEngine;
using System.Collections;

public class AlphaSprite : MonoBehaviour
{
    SpriteRenderer m_pRenderer = null;
    public float m_fPlus = 2.0f;
    public float m_fStartAlpha = 0.0f;

    // Use this for initialization
    void Start()
    {
        m_pRenderer = GetComponent<SpriteRenderer>();
        var color = m_pRenderer.color;
        color.a = m_fStartAlpha;
        m_pRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        var color = m_pRenderer.color;
        color.a += Time.deltaTime * m_fPlus;
        if( color.a > 1.0f  || color.a < 0.0f)
        {
            Destroy(GetComponent<AlphaSprite>());
        }
        m_pRenderer.color = color;
    }
}