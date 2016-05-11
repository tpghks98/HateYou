using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReduceEff : BaseSceneEff {

    private GameObject m_goTarget = null;

    private float m_fReduceValue = 0.0f;
    private Image m_pImage = null;
    private Vector3 m_vStartScale;
    private Vector3 m_vEndScale;

    public void Setup(GameObject goTarget, float fValue, Image Img )
    {
        m_goTarget = goTarget;
        m_fReduceValue = fValue;
        m_pImage = Img;
        GameObject.Find("Star_Parent")
            .GetComponent<Star_Parent>().OnScaling(-0.25f);
       // SceneMgr.Instance.StartScale = 2.0f
        m_vStartScale = m_goTarget.transform.localScale;
        m_vEndScale = m_goTarget.transform.localScale - Vector3.one * fValue;
    }

    public override void UpdateEff(float fTime)
    {
        SceneMgr.Instance.AlphaColor = 1.1f - fTime;

        m_goTarget.transform.localScale =
            m_goTarget.transform.localScale - Vector3.one * m_fReduceValue
            * EasingUtil.easeOutBack(0, 1, fTime) * Time.deltaTime * 2;
        if( m_goTarget.transform.localScale.x < 0.0f )
        {
            m_goTarget.transform.localScale = Vector3.zero;
        }

        m_pImage.color = new Color(1, 1, 1, 1.0f - fTime);
    }
}
