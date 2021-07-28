using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameRunner gameRunner;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ClickButton()
    {
        StartCoroutine(gameRunner.StartGameFromMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
