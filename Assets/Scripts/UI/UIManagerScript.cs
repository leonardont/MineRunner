using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    public GameObject pauseButton, panel, pointsUI, gameOverUI, pauseUI, highScoreMessageUI;

    public AudioClip click;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    /// Toca som de click para feedback sonoro da UI
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(click);
    }

    /// Game pause section
    public void pauseGame()
    {
        Time.timeScale = 0;

        pointsUI.SetActive(false);
        pauseButton.SetActive(false);

        pauseUI.SetActive(true);
        panel.SetActive(true);
    }

    public void continueGame()
    {
        Time.timeScale = 1;

        pauseUI.SetActive(false);
        panel.SetActive(false);

        pointsUI.SetActive(true);
        pauseButton.SetActive(true);
    }
    /// Game pause section

    /// Game over section
    public void gameOver()
    {
        pointsUI.SetActive(false);
        pauseButton.SetActive(false);

        gameOverUI.SetActive(true);
        panel.SetActive(true);

        if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().playerScore >= PlayerPrefs.GetInt("highScore"))
            {
                highScoreMessageUI.SetActive(true);
            }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene("Menu");
    }
    /// Game over section
}
