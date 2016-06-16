using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdsPopup : MonoBehaviour {


	public void Start()
	{
		CheckPopupDeleted ();
	}


	public void OnPopup()
	{
		gameObject.SetActive (true);

		m_goRseult.SetActive (false);
		m_goPop.SetActive (true);
		ScalingSetup (gameObject);
	}

	public void OnClose()
	{
		gameObject.SetActive (false);
	}


    // 결제 시도
	public void TryPurchase()
	{
        // 결제가 완료되면 OnReslut( true ) 호출해주시면 되요
        // 실패시 OnResult( false ) 호출 해주세요.
	}


    // 결제의 성공여부에 따른 결과 출력
    // 만약 결제가 됬을시 광고제거버튼이 보이지 않음
	public void OnResult( bool IsPay )
	{
		m_goRseult.SetActive (true);
		m_goPop.SetActive (false);

		ScalingSetup (m_goRseult);

		string strPathName = "01. Home/Ads/Fail";
		if (IsPay) {
			strPathName = "01. Home/Ads/sucess";
			StageMgr.Instance.SaveIsPayData ();
		}
		m_pResultImg.sprite = Resources.Load (strPathName, typeof(Sprite)) as Sprite;
		CheckPopupDeleted ();
	}

	private void CheckPopupDeleted()
	{
		if (StageMgr.Instance.GetIsPay ()) {
			m_goAdsButton.SetActive (false);
			GameObject.Destroy ( m_goAdsButton);
		}
	}

	private void ScalingSetup( GameObject go )
	{
		var pScaling = 
			go.AddComponent<Scaling> ();
		pScaling.m_fMaxTime = 0.5f;
		pScaling.m_vStartScale = Vector3.zero;
		pScaling.m_vEndScale = Vector3.one;
		go.transform.localScale = Vector3.zero;	
	}

	public GameObject m_goRseult = null;
	public GameObject m_goPop = null;
	public Image m_pResultImg = null;
	public GameObject m_goAdsButton = null;
}
