using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    public float fadeSpeed = 0.10f;
    private bool skip = false;
    private bool canvasObject = false;
    // Start is called before the first frame update
    void Start()
    {
        checkType();
    }

    private void OnEnable()
    {
        checkType();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAlpha(float value)
    {
        if(canvasObject)
        {
            this.GetComponent<CanvasGroup>().alpha = value;
        }
        else
        {
            Color c = this.GetComponent<SpriteRenderer>().color;
            c.a = value;
            this.GetComponent<SpriteRenderer>().color = c;
        }
    }
    void checkType()
    {
        if (this.GetComponent<CanvasGroup>() != null)
        {
            canvasObject = true;
        }
    }

    public void skipFade()
    {
        skip = true;
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

    private IEnumerator FadeOut_Canvas()
    {
        for (float ft = 1f; ft >= 0; ft -= fadeSpeed)
        {
            if (!skip)
            {
                this.GetComponent<CanvasGroup>().alpha = ft;
                yield return null;
            } else
            {
                this.GetComponent<CanvasGroup>().alpha = 0;
                skip = false;
                break;
            }
        }
    }

    private IEnumerator FadeIn_Canvas()
    {
        for (float ft = 0f; ft <= 1; ft += fadeSpeed)
        {
            if (!skip)
            {
                this.GetComponent<CanvasGroup>().alpha = ft;
                yield return null;
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha = 1;
                skip = false;
                break;
            }
        }
    }

    private IEnumerator FadeOut_GameObject()
    {
        for (float ft = 1f; ft >= 0; ft -= fadeSpeed)
        {
            if (!skip)
            {
                Color c = this.GetComponent<SpriteRenderer>().color;
                c.a = ft;
                this.GetComponent<SpriteRenderer>().color = c;
                yield return null;
            }
            else
            {
                Color c = this.GetComponent<SpriteRenderer>().color;
                c.a = 0;
                this.GetComponent<SpriteRenderer>().color = c;
                skip = false;
                break;
            }
        }
    }

    private IEnumerator FadeIn_GameObject()
    {
        for (float ft = 0f; ft <= 1; ft += fadeSpeed)
        {
            if (!skip)
            {
                Color c = this.GetComponent<SpriteRenderer>().color;
                c.a = ft;
                this.GetComponent<SpriteRenderer>().color = c;
                yield return null;
            }
            else
            {
                Color c = this.GetComponent<SpriteRenderer>().color;
                c.a = 1;
                this.GetComponent<SpriteRenderer>().color = c;
                skip = false;
                break;
            }
        }
    }

    public IEnumerator FadeOut()
    {
        if (canvasObject)
        {
            yield return StartCoroutine("FadeOut_Canvas");
        } else
        {
            yield return StartCoroutine("FadeOut_GameObject");
        }
    }

    public IEnumerator FadeIn()
    {
        if (canvasObject)
        {
            yield return StartCoroutine("FadeIn_Canvas");
        }
        else
        {
            yield return StartCoroutine("FadeIn_GameObject");
        }
    }
}
