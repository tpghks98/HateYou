using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SoundButton : MonoBehaviour {

    public string strSoundName = "Click";

    public void OnClickSound()
    {
        SoundMgr.Instance.CreateSound(strSoundName);
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickSound);
    }
}
