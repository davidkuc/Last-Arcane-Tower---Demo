using UnityEngine;

public class GameSceneUI_Setup : MonoBehaviour
{
    [SerializeField] private Vector2 wavesProgressBarPosition;
    [SerializeField] private Vector2 goldWindowPosition;

    private Transform wavesProgressBar;
    private Transform goldWindow;

    private void Awake()
    {
        var container = transform.Find("canvas").Find("gameSceneUIElementsContainer");
        wavesProgressBar = container.Find("WavesProgressBar");
        goldWindow = container.Find("GoldWindow");
    }

    private void Start() => SetupUIPositions();

    [ContextMenu("Set UI Positions")]
    private void SetupUIPositions()
    {
#if UNITY_WEBGL
        wavesProgressBar.transform.position = new Vector3(wavesProgressBarPosition.x, wavesProgressBarPosition.y, 0);
        goldWindow.transform.position = new Vector3(goldWindowPosition.x, goldWindowPosition.y, 0);
#endif
    }
}
