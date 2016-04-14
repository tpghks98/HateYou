using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class State : MonoBehaviour {

    private bool m_IsOn = false;
    private float m_fScaleTime = 0.0f;
    private float m_fSpeed = 2.0f;
    private STATEID m_stateID = STATEID.PAUSE;

    public GameObject m_pNext = null;
    public Image m_pWord = null;

    public GameObject[] m_goStar;
    public GameObject m_goClose;
    public MessageBox m_pMsgBox;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if( m_IsOn )
        {
            m_fScaleTime += Time.fixedDeltaTime * m_fSpeed;
            if( m_fScaleTime > 1.0f )
            {
                m_fScaleTime = 1.0f;
            }
        }
        else
        {
            m_fScaleTime -= Time.fixedDeltaTime * m_fSpeed * 4.0f;
            if( m_fScaleTime < 0.0f )
            {
                m_fScaleTime = 0.0f;
            }
        }
        transform.localScale =
            Vector3.Lerp(Vector3.zero, Vector3.one, m_fScaleTime);
	}
    public void OnState( bool IsOn)
    {
        m_IsOn = IsOn;
        if (IsOn)
        {
            InGameController.Instance.TimePause();
        }
        else
        {
            m_stateID = STATEID.PAUSE;
            InGameController.Instance.TimeStart();
        }
    }
    public void ReverseState()
    {
        if (m_stateID == STATEID.PAUSE)
        {
            OnState(!m_IsOn);
        }
    }
    public void OnClearMsg( STATEID ID)
    {
        m_pMsgBox.gameObject.SetActive(true);
        m_pMsgBox.TextLoad( ID);
    }
    public void SetState( STATEID nID)
    {
        if (nID == STATEID.GREAT ||
            nID == STATEID.PERFECT )
        {
            if ( !m_pMsgBox.GetIsClear() )
            {
                return;
            }
        }
        if (m_stateID == STATEID.PAUSE)
        {
            m_goClose.SetActive(false);
            m_stateID = nID;
            switch (nID)
            {
                case STATEID.PAUSE:
                    OnWord(
                        Resources.Load("02. InGame/02. UI/stop", typeof(Sprite)) as Sprite
                        , false);
                    m_pNext.SetActive(false);
                    StarEnable(0);

                    string str = (PlayerPrefs.GetString(
                 "ST" + ( StageMgr.Instance.SellectStage)));

                    if( str == "Great" )
                    {
                        StarEnable(2 , false );
                        m_pNext.SetActive(true);
                    }
                    else if( str == "Perfect" )
                    {
                        StarEnable(3 , false );
                        m_pNext.SetActive(true);

                    }
                    m_goClose.SetActive(true);
                    break;
                case STATEID.FAIL:
                    OnWord(
                        Resources.Load("02. InGame/02. UI/fail", typeof(Sprite)) as Sprite
                        , false);
                    m_pNext.SetActive(false);
                    StarEnable(0);
                    break;
                case STATEID.GREAT:
                    OnWord(
                        Resources.Load("02. InGame/02. UI/great", typeof(Sprite)) as Sprite,
                        true);
                    m_pNext.SetActive(true);
                    StarEnable(2);
                    break;
                case STATEID.PERFECT:
                    OnWord(
                        Resources.Load("02. InGame/02. UI/perfect", typeof(Sprite)) as Sprite
                        , true);
                    m_pNext.SetActive(true);
                    StarEnable(3);
                    break;
            }
        }
    }
    private void OnWord( Sprite sprite, bool IsScaling)
    {
        m_pWord.sprite = sprite;
        m_pWord.SetNativeSize();
        if( IsScaling )
        {
            var Scaling = m_pWord.gameObject.AddComponent<Scaling>();
            Scaling.m_vStartScale = new Vector3(4, 4, 4);
            Scaling.m_vEndScale = Vector3.one;
            Scaling.m_fMaxTime = 0.75f;
            Scaling.m_IsFixedTime = true;
            Scaling.m_fDelayTime = 0.0f;

            var Alpha = m_pWord.gameObject.AddComponent<Alpha>();
            Alpha.m_fMaxTime = 0.25f;
            Alpha.m_fEndAlpha = 1.0f;
            Alpha.m_fStartAlpha = 0.0f;
            Alpha.m_IsFixedTime = true;
            Alpha.m_fDelayTime = 0.1f; 
        }
    }
    private void StarEnable( int nNum, bool IsDirectional = true )
    {
        for (int n = 0; n < m_goStar.Length; ++n )
        {
            m_goStar[n].SetActive(false);
        }

        for (int n = 2; n >= ( 3 - nNum); --n)
        {
            if (m_goStar.Length < n)
            {
                break;
            }
            m_goStar[n].gameObject.SetActive(true);

            if (IsDirectional)
            {
                var Scaling =
                    m_goStar[n].gameObject.AddComponent<Scaling>();
                Scaling.m_vStartScale = new Vector3(6, 6, 6);
                Scaling.m_vEndScale = Vector3.one;
                Scaling.m_fMaxTime = 1.0f;
                Scaling.m_IsFixedTime = true;
                Scaling.m_fDelayTime = n * 0.65f;

                var Alpha =
                    m_goStar[n].gameObject.AddComponent<Alpha>();
                Alpha.m_fMaxTime = 1.0f;
                Alpha.m_fEndAlpha = 1.0f;
                Alpha.m_fStartAlpha = 0.0f;
                Alpha.m_IsFixedTime = true;
                Alpha.m_fDelayTime = 0.05f + n * 0.65f;
            }
        }
    }
}
