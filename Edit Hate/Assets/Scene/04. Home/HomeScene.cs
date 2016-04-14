using UnityEngine;
using System.Collections;

public class HomeScene : BaseScene {

    public override void SetupName()
    {
        m_strSceneName = "HomeScene";
    }


    public override void Initialize()
    {
        StageMgr.Instance.Initialize();

    }
}
