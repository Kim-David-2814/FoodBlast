using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameBoardController gameBoard;

    private void Start()
    {
        gameBoard = FindObjectOfType<GameBoardController>();
    }

    private void OnMouseDown()
    {
        gameBoard.ExplodeBall(gameObject);
    }
}
