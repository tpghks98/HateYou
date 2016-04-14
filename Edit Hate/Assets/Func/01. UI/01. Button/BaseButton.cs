using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseButton : MonoBehaviour {
    public void ExpandEffSetup( GameObject goTarget )
    {
        if (!SceneMgr.Instance.IsSceneChange &&
    SceneMgr.Instance.IsCanTouch)
        {
            ExpandEff pEff
            = SceneMgr.Instance.ChangeSceneEff<ExpandEff>() as ExpandEff;
            pEff.Setup(goTarget, GameObject.Find("ButtonParent"), 5.0f
                , GetComponent<Image>());
        }
        
    }

    public void ReduceEffSetup( GameObject goTarget )
    {
        if (!SceneMgr.Instance.IsSceneChange &&
    SceneMgr.Instance.IsCanTouch)
        {
            ReduceEff pEff
                = SceneMgr.Instance.ChangeSceneEff<ReduceEff>() as ReduceEff;
            pEff.Setup(goTarget, 3.5f, GetComponent<Image>());
        }
    }

    public void SellectChapter( int n )
    {
        StageMgr.Instance.SellectChapter = n;
    }

    public void SendMsgToStageMgr( string str )
    {
        StageMgr.Instance.SendMessage(str);
    }

}
