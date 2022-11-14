using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogFinishButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.Instance.UnloadTutorialScene);
    }

    private void CleanupBackToMainMenuButtonListeners()
    {
        button.onClick.RemoveListener(GameManager.Instance.UnloadTutorialScene);
        button.onClick.RemoveListener(CleanupBackToMainMenuButtonListeners);
    }
}
