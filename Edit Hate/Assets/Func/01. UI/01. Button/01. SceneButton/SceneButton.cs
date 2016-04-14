using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class SceneButton : BaseButton {

    public SCENEID m_pTargetScene;
    private bool m_IsScaling = false;
    public bool m_IsChangeScene = true;

    public void OnClickButton()
    {
        if (!SceneMgr.Instance.IsSceneChange &&
            SceneMgr.Instance.IsCanTouch)
        {
            SceneMgr.Instance.ChangeScene(m_pTargetScene);
            transform.localScale = transform.localScale * 0.55f;
            m_IsScaling = true;
        }
    }
    public void ImmediateChangeScene()
    {
        SceneMgr.Instance.ImmediateChangeScene(m_pTargetScene);
    }
    void Start()
    {
        if (m_IsChangeScene)
        {
            GetComponent<Button>().onClick.AddListener(OnClickButton);
        }
    }
	// Update is called once per frame
	void Update () {
        if( m_IsScaling )
        {
            transform.localScale = transform.localScale + Vector3.one * Time.deltaTime;
        }

	}

    public void SellectStagePlus()
    {
        StageMgr.Instance.SellectStage =
            StageMgr.Instance.SellectStage + 1;
    }

    public void NoChangeLoadScene()
    {
        InGameController.Instance.NotSceneChLoadStage();
    }
}
