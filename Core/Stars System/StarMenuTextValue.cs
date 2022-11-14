using UnityEngine;
using TMPro;
public class StarMenuTextValue : MonoBehaviour
{
    private TextMeshProUGUI textGoldValue;

    private void Awake() => textGoldValue = GetComponent<TextMeshProUGUI>();

    private void OnEnable() => VariableChangeHandler();

    private void VariableChangeHandler()
    {
        int value = GameManager.Instance.PlayerProgressDataController.Player_SO.StarsSum;
        textGoldValue.text = value.ToString();
    }
}
