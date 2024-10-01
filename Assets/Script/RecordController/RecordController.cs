using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordController : MonoBehaviour
{
    public GameBoardController _conit;
    public int _highTurns;
    public Text _scoreTurnsText;
    public Text _scoreHighText;
    public Text _scoreCoinText;

    private void Update()
    {
        _scoreTurnsText.text = "����� ����� " + _conit._currentTurns.ToString();
        _scoreCoinText.text = "����� ����� " + _conit._currentCoins.ToString();
        _scoreHighText.text = "����� ������� " + PlayerPrefs.GetInt("HighScoreText");
        _highTurns = _conit._currentCoins;

        if (PlayerPrefs.GetInt("HighScoreText") <= _highTurns)
        {
            PlayerPrefs.SetInt("HighScoreText", _conit._currentCoins);
        }
    }
}

