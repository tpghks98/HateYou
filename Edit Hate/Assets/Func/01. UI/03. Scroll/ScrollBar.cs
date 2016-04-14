using UnityEngine;
using System.Collections;

public class ScrollBar : MonoBehaviour {
    public float m_fSize = 0.0f;
    public int m_nNum = 0;
    public Vector2 m_vPossible;
    public Canvas m_pCanvas;

    private Vector2 m_vScroll;
    private Vector2 m_vPrevMousePosition;
    private Vector2 m_vBeginMousePosition;
    private Vector3 m_vEnd;

    private float m_fPlusMove = 0.0f;
    private bool m_IsTouchOn = false;


	// Use this for initialization
	void Start () {
        m_vEnd = new Vector3(-m_vPossible.x, m_vPossible.y, 0.0f)
             * m_fSize * (m_nNum - 1)  * m_pCanvas.scaleFactor;

        m_vPossible.x = Screen.width / 1280.0f * m_vPossible.x;
        m_vPossible.y = Screen.height / 720.0f * m_vPossible.y;
	}
	
	// Update is called once per frame
	void Update () {
        if( m_IsTouchOn)
        {
            m_IsTouchOn = false;
            SceneMgr.Instance.IsCanTouch = true;
        }
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch( touch.phase)
            {
                case TouchPhase.Began:
                    m_fPlusMove = 0.0f;
                    m_vBeginMousePosition = touch.position;
                    m_vPrevMousePosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (Vector3.Distance(m_vBeginMousePosition, touch.position) > 8.0f
                        * m_pCanvas.scaleFactor)
                    {
                        Vector2 vDir = touch.position - m_vPrevMousePosition;
                        vDir.x *= m_vPossible.x;
                        vDir.y *= m_vPossible.y;
                        if (vDir.y != 0.0f)
                        {
                            if ( touch.position.x > 350.0f * m_pCanvas.scaleFactor)
                            {
                                vDir.y = 0.0f;
                            }
                        }
                        TransMove(vDir);
                        SceneMgr.Instance.IsCanTouch = false;
                    }
                    m_vPrevMousePosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    Vector2 v = touch.position - m_vBeginMousePosition;
                    v.x *= m_vPossible.x;
                    v.y *= m_vPossible.y;
                    m_fPlusMove = (v.x + v.y * 0.5f) * 7.0f;
                    m_IsTouchOn = true;
                    break;
            }
        }
        if( Application.platform != RuntimePlatform.Android)
        {
            if( Input.GetMouseButtonDown( 0 ))
            {
                m_fPlusMove = 0.0f;
                m_vBeginMousePosition = Input.mousePosition;
                m_vPrevMousePosition = Input.mousePosition;
            }
            else if( Input.GetMouseButton( 0 ) )
            {
                if (Vector3.Distance(m_vBeginMousePosition, Input.mousePosition) > 1.0f)
                {
                    Vector2 vDir = new Vector2( Input.mousePosition.x,
                        Input.mousePosition.y )
                        - m_vPrevMousePosition;
                    vDir.x *= m_vPossible.x;
                    vDir.y *= m_vPossible.y;
                    if( vDir.y != 0.0f )
                    {
                        if (Input.mousePosition.x > 350.0f * m_pCanvas.scaleFactor)
                        {
                            vDir.y = 0.0f;
                        }
                    }

                    TransMove(vDir);
                        SceneMgr.Instance.IsCanTouch = false;

                }
                m_vPrevMousePosition = Input.mousePosition;
            }
            else if( Input.GetMouseButtonUp( 0 ) )
            {
                Vector2 v = new Vector2( Input.mousePosition.x,
                    Input.mousePosition.y ) - m_vBeginMousePosition;
                v.x *= m_vPossible.x;
                v.y *= m_vPossible.y;

                m_fPlusMove = (v.x + v.y * 0.5f) * 5.0f;

                if (v.y != 0.0f)
                {
                    if (Input.mousePosition.x > 350.0f * m_pCanvas.scaleFactor)
                    {
                        m_fPlusMove = 0.0f;
                    }
                }
                m_IsTouchOn = true;
            }
        }

        if( Mathf.Abs( m_fPlusMove ) > 0.0f )
        {
            Vector3 vTrans = m_vPossible * m_fPlusMove * Time.deltaTime;
            m_fPlusMove -= ( vTrans.x + vTrans.y) * 5.0f;
            TransMove(vTrans);
        }
	}
    public void MoveDistance( int nCount )
    {
        Vector2 vPlus = ( m_vEnd * ( (float)( nCount - 1) / m_nNum ) );
        Debug.Log(m_vEnd);
        transform.Translate( vPlus );
        m_vScroll += vPlus;
    }
    private void TransMove( Vector2 vPlus )
    {
        if (!SceneMgr.Instance.IsSceneChange)
        {
         //   vPlus.x *= (m_fSize / 200.0f);

            vPlus *= (1.0f / m_pCanvas.scaleFactor);
            Vector2 vSum = m_vScroll + vPlus ;

            if (m_vPossible.x != 0.0f)
            {
                if (vSum.x < m_vEnd.x)
                {
                    vPlus.x = 0.0f;
                }
                else if (vSum.x > 0.0f)
                {
                    vPlus.x = 0.0f;
                }
            }
            if (m_vPossible.y != 0.0f)
            {
                if (vSum.y > m_vEnd.y)
                {
                    vPlus.y = 0.0f;
                }
                else if (vSum.y < 0.0f)
                {
                    vPlus.y = 0.0f;
                }
            }
            transform.Translate(vPlus );
            m_vScroll += vPlus;
        }
    }
}
