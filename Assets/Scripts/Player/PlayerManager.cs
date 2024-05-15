using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;
    public TextMeshProUGUI coinsText;
    public GameObject gameMenuPanel; // Assign this reference in the inspector for the "Level" scene
    public Button button1;
    public Button button2;

    public static int numberOfCoins;

    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "Coins: " + numberOfCoins;

        if (SceneManager.GetActiveScene().name == "Level" && numberOfCoins >=75)
        {
            StopGame();
        }

        if (SwipeManager.tap && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }

    public void StopGame()
    {
        isGameStarted = false;
        Time.timeScale = 0;

        // Check if gameMenuPanel reference is assigned before using it
        if (gameMenuPanel != null)
        {
            gameMenuPanel.SetActive(true);

            button1.onClick.AddListener(OnButton1Click);
            button2.onClick.AddListener(OnButton2Click);
        }
        else
        {
            Debug.LogWarning("gameMenuPanel reference is not assigned.");
        }
    }

    void OnButton1Click()
    {
        Time.timeScale = 1;
        gameMenuPanel.SetActive(false);
    }

    void OnButton2Click()
    {
        Application.Quit();
    }
}
