using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] highScores = new TextMeshProUGUI[PersistantDataManager.MAXHIGHSCORE];
    [SerializeField] List<PersistantDataManager.HighScore> HighScores;

    public void StartNewGame()
    {
        SceneManager.LoadScene(PersistantDataManager.SCENEGAME);
    }

    public void ChangePlayer()
    {
        SceneManager.LoadScene(PersistantDataManager.SCENEMENU);
    }

    public void Exit()
    {
        StartScreenManager.Exit();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    { 
        /*
         * This is where we load up the high scores
         */

        for(int i = 0; i < highScores.Length; i++)
        {
            highScores[i].SetText("");
        }

        HighScores = PersistantDataManager.HighScores;

        for(int i = 0; i < HighScores.Count; i++)
        {
            highScores[i].SetText(HighScores[i].score + " " + HighScores[i].playerName);
        }
    }
}
