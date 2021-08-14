using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public Clock Clock;
    public SpawnBlock spawner;
    public GameObject pauseMenu_object;
    private PauseMenu pauseMenu_script;
    public PlayArea playArea;
    public CanvasGameplay canvasGameplay_script;
    public GameObject canvasGameplay_object;
    public CanvasStartScreen canvasStartScreen_script;
    public GameObject canvasStartScreen_object;
    private CanvasGameOver canvasGameOver_script;
    public GameObject canvasGameOver_object;
    public RollingText scoreTracker;

    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height];
    private static int score = 0;
    private static int highScore = 2000;
    private static int level = 0;

    private GameObject currentBlock;
    private GameObject currentGhost;

    public enum gameState
    {
        Started,
        Paused,
        Stopped
    }

    public static gameState state = gameState.Stopped;

    // Start is called before the first frame update
    void Start()
    {
        Clock.enabled = false;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public void SetCurrentBlock(GameObject block)
    {
        currentBlock = block;
    }

    public void ClearCurrentBlock()
    {
        currentBlock = null;
    }

    public void SetCurrentGhost(GameObject ghost)
    {
        currentGhost = ghost;
    }

    public void ClearCurrentGhost()
    {
        currentGhost = null;
    }

    public Transform GetGrid(int x, int y)
    {
        return grid[x, y];
    }

    public void SetGrid(int x, int y, Transform value)
    {
        grid[x, y] = value;
    }

    public void DestroyGrid(int x, int y)
    {
        if (grid[x, y] != null)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
        
    }

    public void MoveGrid(int x, int y, Vector3 direction)
    {
        grid[x, y].transform.position += direction;
    }

    public gameState GetStatus()
    {
        return state;
    }

    public IEnumerator StartGameFromMainMenu()
    {
        yield return StartCoroutine(canvasStartScreen_script.fadeOut());
        canvasStartScreen_object.SetActive(false);
        yield return StartCoroutine("ShowAndStartGame");
    }

    public void StartGame()
    {
        //Debug.Log("Starting Game");
        canvasGameOver_object.SetActive(false);
        state = gameState.Started;
        canvasGameplay_object.SetActive(true);
        Clock.enabled = true;
        UpdateScore();
        Clock.StartTimer();
        spawner.StartGame();
    }

    public IEnumerator EndGame()
    {
        //canvasGameplay_object.SetActive(false);
        Clock.StopTimer();
        DestroyBlockAndGhost();
        state = gameState.Stopped;
        canvasGameOver_object.SetActive(true);
        canvasGameOver_script = canvasGameOver_object.GetComponent<CanvasGameOver>();
        yield return StartCoroutine(canvasGameOver_script.fadeIn());
    }

    public void ConcedeGame()
    {
        HidePauseMenu();
        StartCoroutine("EndGame");
    }

    public void RestartGame()
    {
        HideGameOverMenu();
        ClearGame();
        StartGame();
    }

    private void DestroyBlockAndGhost()
    {
        if (currentBlock != null)
        {
            //Debug.Log("DESTROYING BLOCK!!!");
            Destroy(currentBlock);
        }

        if (currentGhost != null)
        {
            //Debug.Log("DESTROYING GHOST!!!");
            Destroy(currentGhost);
        }
    }

    public void ClearGame()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                DestroyGrid(j, i);
            }
        }

        DestroyBlockAndGhost();

        score = 0;
        Clock.ResetTimer();
    }

    private IEnumerator ShowAndStartGame()
    {
        //Debug.Log("Showing Game Screen");
        yield return StartCoroutine(playArea.fadeIn());
        yield return StartCoroutine(canvasGameplay_script.fadeIn());
        //Debug.Log("Done");
        //Debug.Log("running startGame function");
        StartGame();
    }

    void ShowPauseMenu()
    {
        pauseMenu_object.SetActive(true);
        pauseMenu_script = pauseMenu_object.GetComponent<PauseMenu>();
        pauseMenu_script.PauseGame();
    }

    void HidePauseMenu()
    {
        pauseMenu_script = pauseMenu_object.GetComponent<PauseMenu>();
        pauseMenu_script.UnpauseGame();
        pauseMenu_object.SetActive(false);
    }

    void HideGameOverMenu()
    {
        canvasGameOver_script = canvasGameOver_object.GetComponent<CanvasGameOver>();
        //canvasGameOver_script.fadeOut();
        canvasGameOver_object.SetActive(false);
    }

    public void PauseGame()
    {
        //Debug.Log("Pausing!");
        Clock.enabled = false;
        state = gameState.Paused;
        ShowPauseMenu();
    }

    public void UnpauseGame()
    {
        //Debug.Log("Unpausing!");
        Clock.enabled = true;
        state = gameState.Started;
        HidePauseMenu();
    }

    private void UpdateScore()
    {
        /*
        if (score < highScore)
        {
            scoreTracker.SetText("" + score + " / " + highScore);
        }
        else
        {
            highScore = score;
            scoreTracker.SetText("" + score);
        }
        */
        scoreTracker.SetText(score.ToString());

    }

    public void AddToScore(int lines)
    {
        int increase = 0;
        switch (lines)
        {
            default:
                break;
            case 1:
                increase = (level + 1) * 40;
                break;
            case 2:
                increase = (level + 1) * 100;
                break;
            case 3:
                increase = (level + 1) * 300;
                break;
            case 4:
                increase = (level + 1) * 1200;
                break;
        }

        score += increase;
        //Debug.Log("increasing score by " + increase);

        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == gameState.Started)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        } else if (state == gameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnpauseGame();
            }
        }

    }
}
