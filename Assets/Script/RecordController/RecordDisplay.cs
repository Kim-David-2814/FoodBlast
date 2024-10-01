using UnityEngine;
using UnityEngine.UI;

public class RecordDisplay : MonoBehaviour
{
    public Text highScoreText;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScoreText", 0);
        highScoreText.text = "Лучший результат: " + highScore.ToString();
    }
}
