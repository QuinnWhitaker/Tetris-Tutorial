using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasObject : MonoBehaviour
{
    private bool fadeOut, fadeIn;
    public float fadeSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            float alpha = this.GetComponent<CanvasGroup>().alpha;
            this.GetComponent<CanvasGroup>().alpha = alpha - (fadeSpeed * Time.deltaTime);
            alpha = this.GetComponent<CanvasGroup>().alpha;

            if (alpha <= 0)
            {
                fadeOut = false;
            }
        }

        if (fadeIn)
        {
            float alpha = this.GetComponent<CanvasGroup>().alpha;
            this.GetComponent<CanvasGroup>().alpha = alpha + (fadeSpeed * Time.deltaTime);
            alpha = this.GetComponent<CanvasGroup>().alpha;

            if (alpha >= 1)
            {
                fadeIn = false;
            }
        }
    }

    public void FadeOutObject()
    {
        fadeOut = true;
    }

    public void FadeInObject()
    {
        fadeIn = true;
    }
}
