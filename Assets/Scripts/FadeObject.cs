using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
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

    }

    public void FadeOutObject()
    {
        //fadeOut = true;
        StartCoroutine("FadeOut");
    }

    public void FadeInObject()
    {
        //fadeIn = true;
        StartCoroutine("FadeIn");
    }

    public IEnumerator FadeOut()
    {
        for (float ft = 1f; ft >=0; ft -= fadeSpeed)
        {
            Color c = this.GetComponent<Renderer>().material.color;
            c.a = ft;
            this.GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        for (float ft = 0f; ft <= 1; ft += fadeSpeed)
        {
            Color c = this.GetComponent<Renderer>().material.color;
            c.a = ft;
            this.GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }
}
