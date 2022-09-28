using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistentDataManager : MonoBehaviour
{
    /*
     * We are going to retain the last five (5) high scores in a list
     * and store/load it in a list.
     */

    public static PersistentDataManager Instance;

    private static HighScores highScores;

    private static string saveFilePath;
    public static string CurrentPlayer;

    public static int CurrentScore;
    public static int sessionHighScore = 0;

    public const int MAXHIGHSCORE = 5;
    public const int SCENEMENU = 0;
    public const int SCENEGAME = 1;
    public const int SCENEGAMEOVER = 2;

    [System.Serializable]
    public class HighScoreStruct
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    public class HighScores
    {
        public List<HighScoreStruct> highScoresList;
    }

    private void Awake()
    {
        /*
         * Make sure we are are a singleton
         * Tell Unity not to trash us on scene unload
         */
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveFilePath = Application.persistentDataPath + "/highscores.json";
    }

    public static void SaveHighScores()
    {
        int i;

        /*
         * Ok. What we are going to do here is check to see if the score of the 
         * current game (hopefully set by the main scene)
         * 
         * We need to insert the current player/score into the high score list:
         *   - Search the list to see where to put it
         *   - Insert it where it needs to go
         *   - Kill the last entry if the list length exceeds MAXHIGHSCORE
         *   
         * Then write that sucker out.
         */

        // Create current player/score pair
        HighScoreStruct highScore = new HighScoreStruct();
        highScore.playerName = CurrentPlayer;
        highScore.score = CurrentScore;

        // If the list is empty, then add it as the only element.
        if (highScores.highScoresList.Count == 0)
        {
            highScores.highScoresList.Add(highScore);
        }
        else
        // Otherwise, insert it where it goes
        {
            bool done = false;
            i = 0;

            while (!done && i < highScores.highScoresList.Count)
            {
                if (CurrentScore > highScores.highScoresList[i].score)
                {
                    highScores.highScoresList.Insert(i, highScore);
                    done = true;
                }
                i++;
            }
        }

        // Trim the list to the maximum size
        if (highScores.highScoresList.Count > MAXHIGHSCORE)
        {
            highScores.highScoresList.RemoveAt(MAXHIGHSCORE);
        }

        // Let's write that sucker out
        string json = JsonUtility.ToJson(highScores);
        File.WriteAllText(saveFilePath, json);
    }

    public static void LoadHighScores()
    {
        highScores = new HighScores();
        HighScoreStruct tmp = new HighScoreStruct();

        highScores.highScoresList = new List<HighScoreStruct>();

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            highScores = JsonUtility.FromJson<HighScores>(json);
        }
    }

    public static List<string> GetFormattedHighScoresList()
    {
        List<string> list = new List<string>();

        for (int i = 0; i < highScores.highScoresList.Count; i++)
        {
            list.Add(highScores.highScoresList[i].score + " " + 
                highScores.highScoresList[i].playerName);
        }

        return list;
    }

    public static void SetScores(int score)
    {
        CurrentScore = score;
        sessionHighScore = sessionHighScore < score ? score : sessionHighScore;
    }
}
