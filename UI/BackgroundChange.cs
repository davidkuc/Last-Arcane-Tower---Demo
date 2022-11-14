using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField] ScenesSettings_SO scenesSetting;
    private SpriteRenderer backgroundSprite;
    private void Start()
    {
        backgroundSprite = GetComponent<SpriteRenderer>();
        backgroundSprite.sprite = scenesSetting.background;     
    }
}
