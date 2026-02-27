using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }

    public int Highscore { get; private set; }

    const string HIGHSCORE_KEY = "Highscore";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Highscore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);
    }

    public void StartRun()
    {
        Score = 0;
        SceneManager.LoadScene("1-Level");
    }

    // When the player kills an enemy
    public void AddScore(int amount = 1)
    {
        Score += amount;
    }

    // Whewn the run ends (death or goal reached)
    public void EndRun()
    {
        if (Score > Highscore)
        {
            Highscore = Score;
            PlayerPrefs.SetInt(HIGHSCORE_KEY, Highscore);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("0-Menu");
    }
}