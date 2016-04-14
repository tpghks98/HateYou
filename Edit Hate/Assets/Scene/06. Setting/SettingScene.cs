using UnityEngine;
using System.Collections;

public class SettingScene : BaseScene {
    public override void SetupName()
    {
        m_strSceneName = "SettingScene";
    }

    public override void Initialize()
    {
        StageMgr.Instance.SoundSetup();
    }
}
