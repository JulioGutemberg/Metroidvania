using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public TMP_Text txtScore;
    public int score;
    public GameObject gameOverPanel;
    public GameObject panelPause;
    public ResourceSystem resourceSystem;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("score") > 0)    //Carrega os valores do score salvo
        {
            score = PlayerPrefs.GetInt("score");
            txtScore.text = "x" + score.ToString();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }
    public void GetCoin()
    {
        score++;
        txtScore.text = "x" + score.ToString();

        PlayerPrefs.SetInt("score", score); //Salva localmente o valor do score
    }

    #region Actions
    public void NextLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.DeleteAll();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void PauseMenu()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        panelPause.SetActive(false);
    }
    public void ReturnGame()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.DeleteAll();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
