using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] highScores = new TextMeshProUGUI[PersistentDataManager.MAXHIGHSCORE];
    //[SerializeField] List<PersistentDataManager.HighScore> HighScores;

    public void StartNewGame()
    {
        SceneManager.LoadScene(PersistentDataManager.SCENEGAME, LoadSceneMode.Single);
    }

    public void ChangePlayer()
    {
        PersistentDataManager.sessionHighScore = 0;
        SceneManager.LoadScene(PersistentDataManager.SCENEMENU, LoadSceneMode.Single);
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
        for(int i = 0; i < highScores.Length; i++)
        {
            highScores[i].SetText("");
        }

        List<string> list = PersistentDataManager.GetFormattedHighScoresList();

        for(int i = 0; i < list.Count; i++)
        {
            highScores[i].SetText(list[i]);
        }
    }
}
