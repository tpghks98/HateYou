using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdManager{
	static int counter = 0;
	static System.Action<bool> onEndAction;
	public static void ShowInterstitialAds(){
		if(counter < 3){
			counter++;
			return;
		}
		counter = 0;

		InterstitialAd interstitial = new InterstitialAd("ca-app-pub-5708876822263347/1299219710");
		interstitial.OnAdLoaded += (object sender, System.EventArgs e) => {
			//			interstitial.
			Debug.Log("Interstitial AdLoaded Value="+interstitial.IsLoaded());
			interstitial.Show();
		};
		interstitial.OnAdOpening += (object sender, System.EventArgs e) => {
			Debug.Log("Interstitial AdOpended  Value="+interstitial.IsLoaded());
		};
		interstitial.OnAdClosed += (object sender, System.EventArgs e) => {
			Debug.Log("Interstitial AdClosed");
		};
		interstitial.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) => {
			Debug.Log("Interstitial AdFailedToLoad");
		};
		interstitial.OnAdLeavingApplication += (object sender, System.EventArgs e) => {
			Debug.Log("Interstitial AdLeftApplication");
		};

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}

}
