using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMgr : SingleTon<SoundMgr> {

    private List<CustomSound> m_lstSound = new List<CustomSound>();

    private CustomSound m_pBackGroundSound;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	    foreach( var it in m_lstSound )
        {
            if( it.m_IsDelete && it.gameObject.activeInHierarchy)
            {
                it.gameObject.SetActive(false);
            }
        }

        if( !StageMgr.Instance.IsSongSound)
        {
            if( m_pBackGroundSound != null )
            {
                if( m_pBackGroundSound.isActiveAndEnabled )
                {
                    m_pBackGroundSound.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (!m_pBackGroundSound.isActiveAndEnabled)
            {
                m_pBackGroundSound.gameObject.SetActive(true);
            }
        }
	}

    public void CreateSound(string strPath)
    {
        if (StageMgr.Instance.IsGameSound)
        {
            CustomSound pSound = null;

            foreach (var it in m_lstSound)
            {
                if (it.m_IsDelete)
                {
                    it.m_IsDelete = false;
                    pSound = it;
                    it.gameObject.SetActive(true);
                    Debug.Log(pSound);
                    break;
                }
            }

            if (pSound == null)
            {
                GameObject go = new GameObject();
                go.name = "Sound_" + m_lstSound.Count;
                pSound = go.AddComponent<CustomSound>();
                m_lstSound.Add(pSound);
            }

            pSound.SetupSound(strPath, false);
        }
        else
        {
            GameSoundDestory();
        }
    }
    public void CreateBackGroundSound( string strPath)
    {
        if (StageMgr.Instance.IsSongSound)
        {
            if (m_pBackGroundSound == null)
            {
                GameObject go = new GameObject();
                go.name = "background_sound";
                m_pBackGroundSound = go.AddComponent<CustomSound>();
                DontDestroyOnLoad(m_pBackGroundSound.gameObject);
            }

            m_pBackGroundSound.SetupSound(strPath, true);
        }
    }
    public void GameSoundDestory()
    {
        if (m_lstSound.Count > 0)
        {
            foreach (var it in m_lstSound)
            {
                GameObject.Destroy(it.gameObject);
            }
            m_lstSound.Clear();
        }
    }

}
