using UnityEngine;
using System.Collections;

public class StageBlockController : MonoBehaviour {

    public ShowNumber[] m_ArrShow;
    public StageStar[] m_ArrStar;

    public void OpenBlock( int nStageNum )
    {
        var LockCtrl = GetComponent<LockController>();
        LockCtrl.AllLocking();
        int nLockNum = ( StageMgr.Instance.ClearStgae -  nStageNum ) + 1 ;
        if( nLockNum > 5 )
        {
            nLockNum  = 5;
        }
        else if( nLockNum < 1 )
        {
            nLockNum = 1;
        }

        LockCtrl.UnLocking( nLockNum);

        for (int n = 0; n < m_ArrShow.Length; ++n )
        {
            m_ArrShow[n].DoNotDraw();
            m_ArrStar[n].OnClear( "Fail" );
        }
        for (int n = 0; n < nLockNum; ++n)
        {
            m_ArrShow[n].Draw(n + nStageNum + 1);
            m_ArrStar[n].OnClear(PlayerPrefs.GetString(
                 "ST" + ( n + nStageNum + 1 ) ));

        }
    }
    
    public void OnClickBlcok( int nNum)
    {
        int nStage =
            (( StageMgr.Instance.SellectChapter - 1) * 25) +
            ( (StageMgr.Instance.SellectPlanet - 1 ) * 5) + nNum;

        StageMgr.Instance.SellectStage = nStage;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
