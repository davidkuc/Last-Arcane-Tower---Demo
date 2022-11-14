using UnityEngine;
using TMPro;

public class GoldSystem : MonoBehaviour
{
    [SerializeField] ScenesSettings_SO scenesSetting;
    private TextMeshProUGUI textGoldValue;

    private void Start() => textGoldValue = GetComponent<TextMeshProUGUI>();

    private void Update() => textGoldValue.text = GameManager.Instance.PlayerProgressDataController.Player_SO.gold.ToString();
}
