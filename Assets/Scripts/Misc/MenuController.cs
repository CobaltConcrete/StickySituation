using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Tooltip("Display Highscore")]
    public TextMeshProUGUI highscoreText;
    [Tooltip("Display Lastcore")]
    public TextMeshProUGUI lastScoreText;

    void Start()
    {
        highscoreText.text = "Highscore: " + GameManager.Instance.Highscore;
        lastScoreText.text = "Last Score: " + GameManager.Instance.Score;
    }

    public void OnStartButton()
    {
        GameManager.Instance.StartRun();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}