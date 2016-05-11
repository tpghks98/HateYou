using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class MessageBox : MonoBehaviour {

    List<string> m_lstString = new List<string>();
    string m_strToken;

    public Text m_pTopText;
    public Text m_pBotText;
    public State m_pState;
    public STATEID m_nStateID = STATEID.GREAT;

    private float m_fWordTime = 0.0f;
    private string m_ViewString = "";

    

    public void TextLoad( STATEID ID)
    {
        InGameController.Instance.TimePause();

        m_nStateID = ID;
        m_lstString.Clear();
        string strFileName = "Data/Message/ST_" + 
            System.Convert.ToInt32( StageMgr.Instance.SellectStage );

        TextAsset data
            = Resources.Load(strFileName) as TextAsset;

        if (data != null)
        {
            string strLoad = data.text;
            int nCount = 0;

            string str = " ";

            while (strLoad.Length > nCount)
            {
                str = GetToken(ref strLoad, ref nCount);
 

                string strType = "";
                if (StageMgr.Instance.GetPlayType() == PLAYTYPE.MONO)
                {
                    strType = "M";
                }
                else
                {
                    strType = "C";
                }



                if (str.Substring( 0, 1 ) == strType.Substring( 0, 1 ))
                {

                    str = GetToken(ref strLoad, ref nCount);

                    while( str != "END"  && strLoad.Length > nCount)
                    {
                        if( str != " " )
                        {
                            m_lstString.Add(str); 
                        }
                        str = GetToken(ref strLoad, ref nCount);
                    }
                }
            }
        }
            ChangeWord();
    }
    private string GetToken( ref string str, ref int nCount)
    {
        m_strToken = "";
        bool IsSpaceBreak = true;
        char c;
        int n = 0;

        int nStartCount = nCount;
        bool IsQuit = false;
        int nMinus = 0;
        while( str.Length > nCount)
        {
            c = str[nCount];

            if (IsQuit)
            {
                if( c == '\"')
                {
                    nMinus = 1;
                    nCount++;
                    break;
                }
            }
            if( ( c < 33 && IsSpaceBreak ))
            {
                nCount++;
                 break;
            }
            else if( c == '\"')
            {
                IsQuit = true;
                IsSpaceBreak = false;
                nCount++;
                nStartCount++;
                continue;
            }


            nCount++;
        }

        Debug.Log(str);
        m_strToken = str.Substring(nStartCount, nCount
            - nStartCount - nMinus);

        Debug.Log(m_strToken);

        if( m_strToken.Length >= 3 )
        {
            if( m_strToken.Substring(0, 3) == "END" )
            {
                return "END";
            }
        }
        return m_strToken;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        m_fWordTime += Time.fixedDeltaTime;
        if (m_pTopText.text.Length < m_ViewString.Length)
        {
            if( m_fWordTime > 0.05f )
            {
                m_pTopText.text += m_ViewString[m_pTopText.text.Length];
                m_fWordTime = 0.0f;
            }
        }
	}

    public void NextWord()
    {
        if( m_lstString.Count > 0 )
        {
            m_lstString.RemoveAt(0);
            if( m_lstString.Count == 0)
            {
                Clear();   
            }
            else
            {
                ChangeWord();
            }
        }
    }

    private void ChangeWord()
    {
        if( m_lstString.Count == 1 )
        {
            m_pBotText.text = "END";
        }
        else
        {
            m_pBotText.text = "NEXT";
        }

        if( m_lstString.Count > 0 )
        {
            m_ViewString = m_lstString[0];
            m_pTopText.text = "";
        }
        else
        {
            Clear();
        }
    }


    private void Clear()
    {
        
        m_pState.SetState( m_nStateID );
        m_pState.OnState(true);
        gameObject.SetActive(false);
    }

    public bool GetIsClear()
    {
        if( m_lstString.Count == 0 )
        {
            return true;
        }
        return false;
    }
}
