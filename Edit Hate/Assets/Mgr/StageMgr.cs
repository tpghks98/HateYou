using UnityEngine;
using System.Collections;

public class StageMgr : SingleTon<StageMgr> {
    private int m_nSellectChapter = 1;
    private int m_nOpenChapter = 1;
    private int m_nClearStage = 0;
    private int m_nSellectStage = 1;
    private int m_nSellectPlanet = 1;

    private PLAYTYPE m_ptID = PLAYTYPE.COLOR;

    private bool m_IsGameSound = true;
    private bool m_IsSongSound = true;
    public bool IsGameSound
    {
        get { return m_IsGameSound; }
    }

    public PLAYTYPE GetPlayType() {  return m_ptID;}
    public void SetPlayType(PLAYTYPE ptID) { m_ptID = ptID; }
    public void IsGameSoundOn() { m_IsGameSound = true; }
    public void IsGameSoundOff() { m_IsGameSound = false; }

    public void SetMonoType() { m_ptID = PLAYTYPE.MONO; }
    public void SetColorType () { m_ptID = PLAYTYPE.COLOR; }

    public bool IsSongSound
    {
        get { return m_IsSongSound; }
    }
    public void IsSongSoundOn() { m_IsSongSound = true; }
    public void IsSongSoundOff() { m_IsSongSound = false; } 
    public int OpenChapter
    {
        get { return m_nOpenChapter; }
        set { m_nOpenChapter = value; }
    }
    public int SellectChapter
    {
        get { return m_nSellectChapter; }
        set
        {
            if (value != m_nSellectChapter) { m_nSellectPlanet = 1; } m_nSellectChapter = value;
            if (m_nSellectChapter > 5) m_nSellectChapter = 5;
        }
    }
    public int ClearStgae
    {
        get { return m_nClearStage; }
        set { m_nClearStage = value; }
    }

    public int SellectStage
    {
        get { return m_nSellectStage; }
        set
        {
            m_nSellectStage = value;
            if (StageMgr.Instance.SellectStage > 125) StageMgr.Instance.SellectStage = 125;
        }
    }
    public int SellectPlanet
    {
        get { return m_nSellectPlanet; }
        set { m_nSellectPlanet = value; }
    }

    public override void Initialize()
    {
        if( GameObject.Find( gameObject.name ) !=
            gameObject )
        {
            GameObject.Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        LoadData();
    }
    public void LockSetting()
    {
        LockStage();
    }

    private void LockStage()
    {
        if( SceneMgr.Instance.SceneName ==
            "ChoiceScene" )
        {
            var LockCtrl =
                GameObject.Find("Sign_Parent").GetComponent<LockController>();
            LockCtrl.AllLocking();
            LockCtrl.UnLocking(m_nOpenChapter);
                
        }
        else if( SceneMgr.Instance.SceneName == "PlanetScene")
        {
            GameObject go = GameObject.Find( "Planet");
            go.GetComponent<Planet_Parent>().LoadData(m_nSellectChapter, 5);

            var LockCtrl = go.GetComponent<LockController>();
            LockCtrl.OriSetup();
            LockCtrl.AllLocking();

            int nStage = m_nClearStage - (m_nSellectChapter - 1) * 25;
            nStage /= 5;
            nStage += 1;

            if( nStage > 5 )
            {
                nStage = 5;
            }
            
            LockCtrl.UnLocking(nStage);
        }
    }
    private void LoadData()
    {
        m_nClearStage = PlayerPrefs.GetInt( "ClearStage" );
        m_nOpenChapter = PlayerPrefs.GetInt("OpenChapter" );

        if( m_nClearStage < 0)
        {
            m_nClearStage = 0;
        }
        if (m_nOpenChapter < 1)
        {
            m_nOpenChapter = 1;
        }
    }

    public void SoundSetup()
    {
        if (!m_IsGameSound)
        {
            GameObject.Find("Game_Button").GetComponent<EnableButton>()
                .OnClickButton();
        }
        if (!m_IsSongSound)
        {
            GameObject.Find("Song_Button").GetComponent<EnableButton>()
               .OnClickButton();
        }
    }

    public void ColorSetup()
    {
        if (m_ptID == PLAYTYPE.COLOR)
        {
            GameObject.Find("PlayType_Button").GetComponent<EnableButton>()
               .OnClickButton();
        }
    }
}
