using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoardController : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public int gridWidth = 6;
    public int gridHeight = 6;
    public float spacing = 0.6f;
    private GameObject[,] grid;
    public int _currentTurns = 10;
    public int _currentCoins;
    public GameObject instructionText;

    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];
        CreateInitialBoard();
        HighlightFirstMove();

        instructionText.SetActive(true);
        StartCoroutine(HideInstructionsAfterDelay(5));
    }

    private void CreateInitialBoard()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject ball = Instantiate(ballPrefabs[Random.Range(0, ballPrefabs.Length)],
                    new Vector3(x * spacing, y * spacing, -1), Quaternion.identity);
                grid[x, y] = ball;
            }
        }
    }

    private IEnumerator HideInstructionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        instructionText.SetActive(false);
    }

    private void HighlightFirstMove()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                List<GameObject> potentialMatch = new List<GameObject>();
                CheckMatches(x, y, potentialMatch);

                if (potentialMatch.Count >= 3)
                {
                    foreach (GameObject ball in potentialMatch)
                    {
                        HighlightBall(ball);
                    }
                    return;
                }
            }
        }
    }

    private void HighlightBall(GameObject ball)
    {

        ball.GetComponent<Renderer>().material.color = Color.black;
    }

    private void RemoveHighlight(GameObject ball)
    {

        ball.GetComponent<Renderer>().material.color = Color.white;
    }

    public void ExplodeBall(GameObject ball)
    {
        RemoveHighlight(ball);

        int x = GetBallPositionX(ball);
        int y = GetBallPositionY(ball);

        if (x == -1 || y == -1) return;

        List<GameObject> ballsToRemove = new List<GameObject>();
        CheckMatches(x, y, ballsToRemove);

        if (ballsToRemove.Count >= 2)
        {
            foreach (GameObject b in ballsToRemove)
            {
                Destroy(b);
                UpdateGrid(b);
            }
            UpdateTurns(ballsToRemove.Count);
        }
        else
        {
            Destroy(ball);
            _currentCoins++;
            UpdateGrid(ball);
            _currentTurns--;
        }

        MoveBallsDown();
        CheckGameOver();
    }

    private void UpdateTurns(int count)
    {
        if (count == 2)
        {
            _currentTurns--;
            _currentCoins++;
        }
        else if (count == 3)
        {
            _currentTurns += 1;
            _currentCoins += 1;
        }
        else if (count == 4)
        {
            _currentTurns += 2;
            _currentCoins += 2;
        }
        else if (count >= 5)
        {
            _currentTurns += 3;
            _currentCoins += 3;
        }
    }

    private void CheckMatches(int x, int y, List<GameObject> ballsToRemove)
    {
        CheckDirection(ballsToRemove, x, y, 1, 0);
        CheckDirection(ballsToRemove, x, y, 0, 1);
    }

    private void CheckDirection(List<GameObject> ballsToRemove, int startX, int startY, int directionX, int directionY)
    {
        string colorTag = GetBallTag(startX, startY);
        if (colorTag == null) return;
        ballsToRemove.Add(grid[startX, startY]);

        for (int i = 1; i < gridWidth; i++)
        {
            int newX = startX + directionX * i;
            int newY = startY + directionY * i;

            if (IsInBounds(newX, newY) && GetBallTag(newX, newY) == colorTag)
            {
                ballsToRemove.Add(grid[newX, newY]);
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < gridWidth; i++)
        {
            int newX = startX - directionX * i;
            int newY = startY - directionY * i;

            if (IsInBounds(newX, newY) && GetBallTag(newX, newY) == colorTag)
            {
                ballsToRemove.Add(grid[newX, newY]);
            }
            else
            {
                break; 
            }
        }
    }

    private string GetBallTag(int x, int y)
    {
        GameObject ball = grid[x, y];
        return ball != null ? ball.tag : null;
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    private void UpdateGrid(GameObject ball)
    {
        int x = GetBallPositionX(ball);
        int y = GetBallPositionY(ball);

        if (x != -1 && y != -1)
        {
            grid[x, y] = null;
        }
    }

    private int GetBallPositionX(GameObject ball)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == ball)
                {
                    return x;
                }
            }
        }
        return -1;
    }

    private int GetBallPositionY(GameObject ball)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == ball)
                {
                    return y;
                }
            }
        }
        return -1;
    }

    private void MoveBallsDown()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight - 1; y++)
            {
                if (grid[x, y] == null)
                {
                    for (int newY = y + 1; newY < gridHeight; newY++)
                    {
                        if (grid[x, newY] != null)
                        {
                            grid[x, y] = grid[x, newY];
                            grid[x, newY] = null;

                            grid[x, y].transform.position = new Vector3(x * spacing, y * spacing, -1);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CheckGameOver()
    {
        if (_currentTurns <= 0 || AreAllBallsGone())
        {
            SceneManager.LoadScene("RecordScene");
        }
    }

    private bool AreAllBallsGone()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

