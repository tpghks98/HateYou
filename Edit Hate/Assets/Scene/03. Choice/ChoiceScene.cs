using UnityEngine;
using System.Collections;

public class ChoiceScene : BaseScene {

    public override void SetupName()
    {
        m_strSceneName = "ChoiceScene";
    }


    public override void Initialize()
    {
        StageMgr.Instance.LockSetting();
        StageMgr.Instance.ColorSetup();
    }
}
