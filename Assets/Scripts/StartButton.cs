using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public CanvasStartScreen canvasStartScreen;
    public CanvasGameplay canvasGameplay;
    public PlayArea playArea;
    public GameRunner gameRunner;
    // Start is called before the first frame update
    void Start()
    {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("Clocked!");
        StartCoroutine("TransitionToGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TransitionToGame()
    {
        canvasStartScreen.fadeOut();
        yield return StartCoroutine(canvasStartScreen.fadeOut());
        yield return StartCoroutine(playArea.fadeIn());
        yield return StartCoroutine(canvasGameplay.fadeIn());
        canvasStartScreen.SetButtons(false);
        gameRunner.StartGame();
    }
}
