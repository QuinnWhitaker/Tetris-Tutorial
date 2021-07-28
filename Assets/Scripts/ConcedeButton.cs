using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcedeButton : MonoBehaviour
{
    public GameRunner gameRunner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton()
    {
        StartCoroutine(gameRunner.ConcedeGame());
    }
}
