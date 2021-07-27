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
    public CanvasGameplay canvasGameplay;
    public CanvasStartScreen canvasStartScreen_script;
    public GameObject canvasStartScreen_object;
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

    public gameState getStatus()
    {
        return state;
    }

    public IEnumerator StartGame()
    {
        canvasStartScreen_script.fadeOut();
        yield return StartCoroutine(canvasStartScreen_script.fadeOut());
        yield return StartCoroutine(playArea.fadeIn());
        yield return StartCoroutine(canvasGameplay.fadeIn());
        canvasStartScreen_object.SetActive(false);
        state = gameState.Started;
        spawner.StartGame();
        Clock.enabled = true;
        Clock.timer.Start();
    }

    public void endGame()
    {
        Debug.Log("LOSS");
        Clock.enabled = false;
        state = gameState.Stopped;
    }

    public void PauseGame()
    {
        Debug.Log("Pausing!");
        Clock.enabled = false;
        state = gameState.Paused;
        pauseMenu_object.SetActive(true);
        pauseMenu_script = pauseMenu_object.GetComponent<PauseMenu>();
        pauseMenu_script.PauseGame();
    }

    public void UnpauseGame()
    {
        Debug.Log("Unpausing!");
        Clock.enabled = true;
        state = gameState.Started;
        pauseMenu_script = pauseMenu_object.GetComponent<PauseMenu>();
        pauseMenu_script.UnpauseGame();
        pauseMenu_object.SetActive(false);
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
