using UnityEngine;
using System.Collections;

public class InGameScene : BaseScene{

    public override void SetupName()
    {
        m_strSceneName = "InGameScene";
    }

    public override void Initialize()
    {
        InGameController.Instance.Initialize();
    }

    public override void Release()
    {
        InGameController.Instance.SetUnActiveBackSt();
    }
}
