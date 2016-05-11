using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class ExpandEff : BaseSceneEff {

    private GameObject m_goObjectParent = null;
    private GameObject m_goTarget   =   null;
    private float m_fExpandValue    =   0.0f;
    private Image m_pImage = null;

    private List<GameObject> m_lstParent = new List<GameObject>();

    private Vector3 m_vStartScale;
    private Vector3 m_vEndScale;

    public void Setup( GameObject goTarget, GameObject goParent, float fValue
        , Image Img)
    {
        m_pImage = Img;
        m_goTarget = goTarget;
        m_goObjectParent = goParent;
        m_fExpandValue = fValue;
        
        GameObject.Find("Star_Parent")
            .GetComponent<Star_Parent>().OnScaling(0.5f);
        SceneMgr.Instance.StartScale = 0.0f;

        GameObject go = goTarget.transform.parent.gameObject;
        while( go.name != "ButtonParent" )
        {
            m_lstParent.Add(go);
            go = go.transform.parent.gameObject;
            if( go == null )
            {
                Debug.Log(" ExpandEff , ButtonParent NotFind ");
                break;
            }
        }
        m_vStartScale = m_goObjectParent.transform.localScale;
        m_vEndScale = m_vStartScale + Vector3.one * 0.5f;
    }

    public override void UpdateEff(float fTime)
    {
        SceneMgr.Instance.AlphaColor = 1.05f - fTime ;

        m_goObjectParent.transform.localScale
            = Vector3.Lerp( m_vStartScale, m_vEndScale,EasingUtil.easeOutBack(0, 1, fTime));


        m_pImage.color = new Color( 1, 1, 1, 1.0f - fTime );
        


        Vector3 v = -m_goTarget.transform.localPosition *
            m_goObjectParent.transform.localScale.x;

        for (int n = 0; n < m_lstParent.Count; ++n)
        {
            v -= m_lstParent[n].transform.localPosition
                * m_goObjectParent.transform.localScale.x; ;
        }

        Vector3 vTest = v - m_goObjectParent.transform.localPosition;

        var Trans = (v - m_goObjectParent.transform.localPosition) * m_fExpandValue
             * Time.deltaTime
              * EasingUtil.easeOutBack(0, 1, fTime) * 2;
        m_goObjectParent.transform.Translate(Trans );

    }
}
