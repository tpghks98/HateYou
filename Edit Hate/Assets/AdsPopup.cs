using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdsPopup : MonoBehaviour {


	public void Start()
	{
		CheckPopupDeleted ();
	}

	System.Action delegateAction;
	public void OnAdBuyPopup()
	{
		gameObject.SetActive (true);

		m_goRseult.SetActive (false);
		m_goPop.SetActive (true);
		m_goPop.transform.FindChild("Text").GetComponent<Text>().text = "0.99$로 광고를 제거하실수 있습니다.\n구입하시겠습니까?";
		delegateAction = TryPurchase;
		ScalingSetup (gameObject);
	}
	public void OnRecoverPopup(){
		gameObject.SetActive (true);

		m_goRseult.SetActive (false);
		m_goPop.SetActive (true);
		m_goPop.transform.FindChild("Text").GetComponent<Text>().text = "기존에 구매한 내용을 복구하시겠습니까?";
		delegateAction = TryRecover;
		ScalingSetup (gameObject);
	}

	public void OnClose()
	{
		gameObject.SetActive (false);
	}

	public void OnPressOkay(){
		delegateAction();
	}


    // 결제 시도
	void TryPurchase()
	{
        // 결제가 완료되면 OnReslut( true ) 호출해주시면 되요
        // 실패시 OnResult( false ) 호출 해주세요.
		GameObject.Find("IAP").GetComponent<InAppPurchase>().BuyConsumable(res=>{
			OnResult(res);
		});
	}
	void TryRecover(){
		GameObject.Find("IAP").GetComponent<InAppPurchase>().RestorePurchases(res=>{
			OnResult(res);
		});
	}


    // 결제의 성공여부에 따른 결과 출력
    // 만약 결제가 됬을시 광고제거버튼이 보이지 않음
	public void OnResult( bool IsPay )
	{
		m_goRseult.SetActive (true);
		m_goPop.SetActive (false);

		ScalingSetup (m_goRseult);

//		string strPathName = "01. Home/Ads/Fail";
		if (IsPay) {
//			strPathName = "01. Home/Ads/sucess";
			m_goRseult.transform.FindChild("Text").GetComponent<Text>().text = "구매에 성공하였습니다";
//			StageMgr.Instance.SaveIsPayData ();
		}else{
			m_goRseult.transform.FindChild("Text").GetComponent<Text>().text = "구매에 실패하였습니다";
		}

//		m_pResultImg.sprite = Resources.Load (strPathName, typeof(Sprite)) as Sprite;
		CheckPopupDeleted ();
	}

	private void CheckPopupDeleted()
	{
		if (StageMgr.Instance.GetIsPay ()) {
			m_goAdsButton.SetActive (false);
			m_goRestoreButton.SetActive(false);
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
	public GameObject m_goRestoreButton = null;
}
