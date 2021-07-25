using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStartScreen : MonoBehaviour
{
    private FadeObject fade;
    private bool finished = false;
    private bool skip = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            CanvasGroup childCG = child.GetComponent<CanvasGroup>();
            childCG.alpha = 0;
        }
        fade = this.GetComponent<FadeObject>();
        fadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            skip = true;
            fade.skipFade();
        }
    }

    IEnumerator FinishInSeconds(float seconds)
    {
        finished = false;
        if (!skip)
        {
            yield return new WaitForSeconds(seconds);
            finished = true;
        }
    }

    public void fadeIn()
    {
        StartCoroutine("StartFadeIn");
    }

    public IEnumerator fadeOut()
    {
        yield return (fade.FadeOut());
    }

    IEnumerator StartFadeIn()
    {
        StartCoroutine(FinishInSeconds(7));
        yield return new WaitUntil(() => finished || skip);

        foreach (Transform child in transform)
        {
            FadeObject childFade = child.GetComponent<FadeObject>();
            childFade.fadeSpeed = 0.01f;
            childFade.FadeInObject();
            if (child.name.Equals("Logo"))
            {
                StartCoroutine(FinishInSeconds(1.5f));
                yield return new WaitUntil(() => finished || skip);
            }
            else
            {
                StartCoroutine(FinishInSeconds(0.8f));
                yield return new WaitUntil(() => finished || skip);
            }

        }
    }

    public void SetButtons(bool enabled)
    {
        foreach (Transform child in transform)
        {
            Button childButton = child.GetComponent<Button>();

            if (childButton != null)
            {
                childButton.interactable = enabled;
            }
        }
    }
}
