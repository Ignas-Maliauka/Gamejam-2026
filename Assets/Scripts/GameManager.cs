using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public float timer = 0;
    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time Survived: " + timer.ToString();
        Time.timeScale = 0f;
    }
    public void gameWon()
    {
        winPanel.SetActive(true);
        winPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Target found in: " + timer.ToString();

        Time.timeScale = 0f;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
