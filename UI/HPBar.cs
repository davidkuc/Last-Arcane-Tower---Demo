using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] HPMP_SO hPMP_SO;
    private TextMeshProUGUI HPtextValue;
    private Slider hpSlider;

    void Start()
    {
        HPtextValue = GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        hpSlider = GetComponentInChildren(typeof(Slider)) as Slider;
    }

    void Update()
    {
        HPtextValue.text = hPMP_SO.CurrentHP + "/" + hPMP_SO.MaxHP;
        hpSlider.value = SliderValueCalculate(hPMP_SO.CurrentHP, hPMP_SO.MaxHP);
    }

    float SliderValueCalculate(float currentHP, float maxHP) => currentHP / maxHP;
}
