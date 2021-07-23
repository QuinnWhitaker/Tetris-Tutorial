using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public Clock Clock;
    public SpawnBlock spawner;
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

    public void StartGame()
    {
        state = gameState.Started;
        spawner.StartGame();
        Clock.enabled = true;
        Clock.timer.Start();
    }

    public void endGame()
    {
        Debug.Log("LOSS");
        Clock.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == gameState.Started)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Pausing!");
                Clock.enabled = false;
                state = gameState.Paused;
            }
        } else if (state == gameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Unpausing!");
                Clock.enabled = true;
                state = gameState.Started;
            }
        }

    }
}
