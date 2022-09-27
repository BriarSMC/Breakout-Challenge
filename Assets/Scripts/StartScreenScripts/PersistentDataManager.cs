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

    private string saveFilePath = Application.persistentDataPath + "/highscores.json";

    public static string CurrentPlayer;
    public static int CurrentScore;

    public const int MAXHIGHSCORE = 5;
    public const int SCENEMENU = 0;
    public const int SCENEGAME = 1;
    public const int SCENEGAMEOVER = 2;

    private void Awake()
    {
        /*
         * Make sure we are are a singleton
         * Tell Unity not to trash us on scene unload
         * Load high scores from storage
         */
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class HighScore
    {
        public string playerName;
        public int score;
    }

    public static List<HighScore> HighScores = new List<HighScore>();

    public void SaveHighScores()
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
        HighScore highScore = new HighScore();
        highScore.playerName = CurrentPlayer;
        highScore.score = CurrentScore;

        // If the list is empty, then add it as the only element.
        if (HighScores.Count == 0)
        {
            HighScores.Add(highScore);
        }
        else
        // Otherwise, insert it where it goes
        {
            for (i = 0; i < HighScores.Count; i++)
            {
                if (highScore.score > HighScores[i].score)
                {
                    HighScores.Insert(i, highScore);
                    break;
                }
            }
            if (i >= MAXHIGHSCORE)
            {
                HighScores.Add(highScore);
            }
        }

        // Trim the list to the maximum size
        if (HighScores.Count > MAXHIGHSCORE)
        {
            HighScores.RemoveAt(MAXHIGHSCORE);
        }

        // Let's write that sucker out
        string json = JsonUtility.ToJson(HighScores);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadHighScores()
    {

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            HighScores = JsonUtility.FromJson<List<HighScore>>(json);
        }
    }
}
