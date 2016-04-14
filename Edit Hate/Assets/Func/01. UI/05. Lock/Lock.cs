using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lock : MonoBehaviour {
    public Sprite m_pLockImage;
    public Sprite m_pOriImage;

    private Image m_pImage;
    private Button m_pButton;
	// Use this for initialization
	void Start () {
        m_pButton = GetComponent<Button>();
        m_pImage = GetComponent<Image>();
	}
    public void Setup()
    {
        m_pOriImage = m_pImage.sprite;

    }


    public void ImageLock()
    {
        if (m_pImage != null)
        {
            m_pImage.sprite = m_pLockImage;
        }
        if( m_pButton != null )
        {
            m_pButton.enabled = false;
        }
    }

    public void ImageUnlock()
    {
        if (m_pImage != null)
        {
            m_pImage.sprite = m_pOriImage;
        }
        if( m_pButton != null )
        {
            m_pButton.enabled = true;
        }
    }
}
