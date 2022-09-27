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



    // Start is called before the first frame update
    void Start()
    {
           
    }

    public void StartNewGame()
    {
        string name = playerName.text.Trim();

        if(name == null)
        {
            errorMessage.SetText("Please enter a player name");
            return;
        }

        PersistentDataManager.CurrentPlayer = name;
        PersistentDataManager.CurrentScore = 0;
        errorMessage.SetText("");
    }

    public static void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
