using UnityEngine;
using TMPro;

[DefaultExecutionOrder(1)]
public class MenuGoldTextValue : MonoBehaviour
{
    private TextMeshProUGUI textGoldValue;

    private void Awake() => textGoldValue = GetComponent<TextMeshProUGUI>();

    private void OnEnable()
    {
        GameManager.Instance.PlayerProgressDataController.OnGoldChangeEvent += VariableChangeHandler;
        VariableChangeHandler(GameManager.Instance.PlayerProgressDataController.Player_SO.gold);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerProgressDataController.OnGoldChangeEvent -= VariableChangeHandler;
    }

    private void VariableChangeHandler(int value) => textGoldValue.text = value.ToString();
}
