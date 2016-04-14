using UnityEngine;
using System.Collections;

public abstract class BaseScene {
    protected string m_strSceneName;

    public string SceneName
    {
        get { return m_strSceneName; }
    }
    public void LoadLevel()
    {
        Application.LoadLevel(m_strSceneName);
    }

    public abstract void Initialize();
    public abstract void SetupName();
    public virtual void Release() { }
}
