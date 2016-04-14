using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnableButton : BaseButton {
    public Sprite m_pOnImage;
    public Sprite m_pOffImage;

    public string m_strOnMsg;
    public string m_strOffMsg;


    private bool m_IsOn = true;
    private Image m_pImage = null;

    public bool IsOn
    {
        get { return m_IsOn; }
    }

    void Start()
    {
        m_pImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    public void OnClickButton()
    {
        m_IsOn = !m_IsOn;
        if( m_IsOn )
        {
            m_pImage.sprite = m_pOnImage;
            SendMsgToStageMgr(m_strOnMsg);
        }
        else
        {
            m_pImage.sprite = m_pOffImage;
            SendMsgToStageMgr(m_strOffMsg);
        }
    }
}
