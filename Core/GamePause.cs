using UnityEngine;

public class GamePause : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void OnClickPauseGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
