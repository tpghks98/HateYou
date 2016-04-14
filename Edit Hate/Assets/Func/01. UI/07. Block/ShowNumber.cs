using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShowNumber : MonoBehaviour {

    private Sprite[] m_pSprite = new Sprite[10];
    private List<Image> m_lstImage = new List<Image>();

    private int m_nPrevNum = -1;

    public void DoNotDraw()
    {
        m_nPrevNum = -1;
        for (int n = 0; n < m_lstImage.Count; ++n)
        {
            m_lstImage[n].enabled = false;
        }
    }
    public void Draw( int nNumber )
    {
        if (nNumber != m_nPrevNum)
        {
            m_nPrevNum = nNumber;
            string str = nNumber.ToString();
            for (int n = 0; n < m_lstImage.Count; ++n)
            {
                m_lstImage[n].enabled = false;
            }
            for (int n = 0; n < str.Length; ++n)
            {
                while (m_lstImage.Count <= n)
                {
                    CreateNum();
                }
                int nNum = System.Convert.ToInt32(str[n].ToString());

                m_lstImage[n].enabled = true;
                m_lstImage[n].sprite = m_pSprite[nNum];

                m_lstImage[n].SetNativeSize();
            }
            Sorting();
        }
    }
    private void CreateNum()
    {
        GameObject go = Instantiate(
            Resources.Load("04. Planet/NumberObject", typeof(GameObject)) as GameObject );

        Image Img= go.GetComponent<Image>();
        Img.SetNativeSize();
        m_lstImage.Add( Img );

        go.transform.SetParent(transform);
       
    }

    private void Sorting()
    {
        float fSize = 40.0f * (Screen.width / 1280.0f);

        int nCount = 0;
        for (int n = 0; n < m_lstImage.Count; ++n)
        {
            if (m_lstImage[n].enabled)
            {
                nCount++;
            }
        }

        for (int n = 0; n < m_lstImage.Count; ++n)
        {
            m_lstImage[n].gameObject.transform.position
                = transform.position +   new Vector3(1, 0, 0) * (
                (n * fSize) - (((nCount - 1) * 0.5f) * fSize));

        }

    }
	// Use this for initialization
	void Start () {
        LoadResource("04. Planet/");
        for ( int n =0; n < 3; ++n )
        {
            CreateNum();
        }
	}
    private void LoadResource( string str )
    {
        for( int n =0; n < 10; ++n )
        {
            m_pSprite[n] = Resources.Load(str + n, typeof(Sprite)) as Sprite;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
