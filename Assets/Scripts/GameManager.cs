using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
