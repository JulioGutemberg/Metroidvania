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

    public void GetCoin()
    {
        score++;
        txtScore.text = "x" + score.ToString();

        PlayerPrefs.SetInt("score", score); //Salva localmente o valor do score
    }
    public void NextLvl()
    {
        Debug.Log("Proxima fase");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.DeleteAll();
    }

}
