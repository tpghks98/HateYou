using UnityEngine;
using System.Collections;

public class PlanetScene : BaseScene {

    public override void SetupName()
    {
        m_strSceneName = "PlanetScene";
    }

    public override void Initialize()
    {
        StageMgr.Instance.LockSetting();
    }
}
