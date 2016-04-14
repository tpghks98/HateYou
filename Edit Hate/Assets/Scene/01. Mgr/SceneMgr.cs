using UnityEngine;
using System.Collections;


public enum SCENEID
{
    INGAME, HOME, CHOICE, PLANET, SETTING, INTRO
};

public class SceneMgr : SingleTon<SceneMgr> {
    private BaseSceneEff m_pSceneEff;
    private SCENEID m_nSceneID;
    private BaseScene   m_pScene;
    private float m_fCurrTime;
    private float m_fMaxTime;
    private bool m_IsSceneChange;
    private bool m_IsInit;
    private float m_fStartScale = 0.0f;
    private float m_fAlphaColor = 1.0f;
    private bool m_IsCanTouch = true; 


    public float StartScale
    {
        get { return m_fStartScale; }
        set { m_fStartScale = value; }
    }
    public float AlphaColor
    {
        get { return m_fAlphaColor; }
        set { m_fAlphaColor = value; }
    }
    public string SceneName
    {
        get { return m_pScene.SceneName; }
    }

    public bool IsSceneChange
    {
        get { return m_IsSceneChange; }
    }
    public bool IsCanTouch
    {
        get { return m_IsCanTouch; }
        set { m_IsCanTouch = value; }
    }
    public void ChangeScene(SCENEID nID)
    {
        m_nSceneID = nID;
        m_IsSceneChange = true;
        if (m_pScene != null)
        {
            m_pScene.Release();
        }
        Debug.Log(nID);
    }
    public BaseSceneEff ChangeSceneEff<T>() where T : BaseSceneEff, new()
    {
        m_pSceneEff = null;
        m_pSceneEff = new T();

        return m_pSceneEff;
    }
    public void ImmediateChangeScene( SCENEID nID)
    {
        m_nSceneID = nID;
        LoadScene();
        m_IsSceneChange = false;
        m_pSceneEff = null;
    }

    void Start()
    {
        m_pSceneEff = null;
        m_IsInit = true;
        m_fMaxTime = 0.5f;
        m_fCurrTime = m_fMaxTime;
        m_IsSceneChange = false;
        ChangeScene(SCENEID.INTRO);
    }

    void Update()
    {
        SceneProcessUpdate();
    }

    private void SceneProcessUpdate()
    {
        if (!m_IsInit)
        {
            m_pScene.Initialize();
            m_IsInit = true;
        }
        if (m_IsSceneChange)
        {
            m_fCurrTime += Time.deltaTime;
            if (m_fCurrTime > m_fMaxTime)
            {
                m_fCurrTime = m_fMaxTime;
                m_IsSceneChange = false;
                m_fCurrTime = 0.0f;
                m_pSceneEff = null;
                LoadScene();
            }
            if (m_pSceneEff != null)
            {
                m_pSceneEff.UpdateEff(m_fCurrTime / m_fMaxTime);
            }
        }
    }

    private void LoadScene()
    {
        switch (m_nSceneID)
        {
            case SCENEID.INGAME:
                ChangeScene<InGameScene>();
                break;
            case SCENEID.HOME:
                ChangeScene<HomeScene>();
                break;
            case SCENEID.CHOICE:
                ChangeScene<ChoiceScene>();
                break;
            case SCENEID.PLANET:
                ChangeScene<PlanetScene>();
                break;
            case SCENEID.SETTING:
                ChangeScene<SettingScene>();
                break;
            case SCENEID.INTRO:
                ChangeScene<IntroScene>();
                break;
        }
    }

    private void ChangeScene<T>() where T : BaseScene, new()
    {
        System.GC.Collect();     

        m_pScene = null;
        m_pScene = new T();

        m_pScene.SetupName();
        m_pScene.LoadLevel();
        m_fAlphaColor = 1.0f;
        m_IsInit = false;
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
