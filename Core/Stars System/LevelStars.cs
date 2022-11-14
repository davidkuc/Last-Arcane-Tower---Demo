using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStars : MonoBehaviour
{
    private Sprite starSprite0;
    private Sprite starSprite1;
    private Sprite starSprite2;
    private Sprite starSprite3;

    private TextMeshProUGUI levelText;

    private int levelIndex;
    private int levelStars;

    private Image levelSprite;

    private void Awake()
    {
        starSprite0 = Resources.Load<Sprite>("Art/Stars/star_1");
        starSprite1 = Resources.Load<Sprite>("Art/Stars/star_2");
        starSprite2 = Resources.Load<Sprite>("Art/Stars/star_3");
        starSprite3 = Resources.Load<Sprite>("Art/Stars/star_4");
        levelText = transform.Find("Number").GetComponent<TextMeshProUGUI>();
        levelSprite = transform.Find("Star").GetComponent<Image>();
        levelIndex = int.Parse(levelText.text);
    }

    private void OnEnable()
    {
        levelStars = GameManager.Instance.PlayerProgressDataController.Player_SO.GetLevelStarsAmount(levelIndex);

        ChangeStarSprite();
    }

    private void ChangeStarSprite()
    {
        if (levelStars == 0) levelSprite.sprite = starSprite0;
        else if (levelStars == 1) levelSprite.sprite = starSprite1;
        else if (levelStars == 2) levelSprite.sprite = starSprite2;
        else if (levelStars == 3) levelSprite.sprite = starSprite3;
    }
}
