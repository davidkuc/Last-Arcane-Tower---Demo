using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

[DefaultExecutionOrder(1)]
public class AdManager : DebuggableBaseClass, IUnityAdsListener
{
    const string ANDROID_GAME_ID = "5016499";

    private WaitForSeconds waitForInitialization;

    private void Awake()
    {
        waitForInitialization = new WaitForSeconds(0.5f);
#if UNITY_EDITOR
        Advertisement.AddListener(this);
        Advertisement.Initialize(ANDROID_GAME_ID, testMode: true);
#elif UNITY_ANDROID
        Advertisement.AddListener(this);
        Advertisement.Initialize(ANDROID_GAME_ID, testMode: false);
#endif
    }

    private void Start()
    {
#if UNITY_ANDROID              
        StartCoroutine(ShowBannerWhenInitialized());
#endif
    }

    private IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
            yield return waitForInitialization;

        Advertisement.Banner.Show("bannerAndroid");
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    [ContextMenu("Show Ad")]
    public void ShowAd()
    {
#if UNITY_ANDROID
        Advertisement.Show("rewardedVideo");
#endif
    }

    public void OnUnityAdsReady(string placementId) => PrintDebugLog($"ADS READY");

    public void OnUnityAdsDidError(string message) => PrintDebugLog($"AD SHOW ERROR - {message}");

    public void OnUnityAdsDidStart(string placementId) => PrintDebugLog("AD SHOW STARTED");

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                PrintDebugLog("AD SHOW FAILED");
                break;
            case ShowResult.Skipped:
                PrintDebugLog("AD SHOW SKIPPED");
                break;
            case ShowResult.Finished:
                PrintDebugLog("AD SHOW FINISHED");
                break;
            default:
                break;
        }
    }
}
