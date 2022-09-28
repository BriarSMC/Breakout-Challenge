using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI errorMessage;          // SET IN EDITOR
    [SerializeField] TextMeshProUGUI playerName;            // SET IN EDITOR    

    private void Start()
    {
        PersistentDataManager.LoadHighScores();
    }

    public void StartNewGame()
    {
        string name = playerName.text.Trim((char)8203); // https://forum.unity.com/threads/textmesh-pro-ugui-hidden-characters.505493/
        name = name.Trim();

        if(string.IsNullOrEmpty(name))
        {
            errorMessage.SetText("Please enter a player name");
            return;
        }

        PersistentDataManager.CurrentPlayer = name;
        PersistentDataManager.CurrentScore = 0;
        PersistentDataManager.sessionHighScore = 0;
        errorMessage.SetText("");

        PersistentDataManager.LoadHighScores();
        SceneManager.LoadScene(PersistentDataManager.SCENEGAME, LoadSceneMode.Single);
    }

    public static void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scence, LoadSceneMode mode)
    {
        errorMessage.SetText("");           // Erase any text in the error message line
    }
}

