using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    FadeObject fade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        fade = this.GetComponent<FadeObject>();
        fade.FadeInObject();
    }

    public void UnpauseGame()
    {
        fade = this.GetComponent<FadeObject>();
        fade.FadeOutObject();
    }
}
