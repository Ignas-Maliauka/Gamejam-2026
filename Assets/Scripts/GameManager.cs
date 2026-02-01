using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool controlLock = true;
    public static bool successfulScan = false;

    public GameObject tutorialPanel;

    public GameObject cameraGameobject;
    public GameObject playerGameobject;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject enemies;
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
    private void Start()
    {
        controlLock = true;
        successfulScan = false;
    }
    private void startCutscene()
    {
        tutorialPanel.SetActive(false);
        cameraGameobject.GetComponent<PlayableDirector>().Play();
        Invoke("startGame", 5f);
    }
    private void startGame()
    {
        timer = 0;
        controlLock = false;
        cameraGameobject.GetComponent<PositionConstraint>().enabled = true;

    }
    private void Update()
    {
        if (successfulScan)
        {
            startCutscene();
            successfulScan = false;
            enemies.SetActive(true);
        }
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
