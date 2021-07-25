using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFadeInScreen : MonoBehaviour
{
    private FadeObject fade;
    private bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        fade = this.GetComponent<FadeObject>();
        StartCoroutine("StartFadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            finished = true;
            fade.skipFade();
        }
    }

    IEnumerator FinishInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        finished = true;
    }

    IEnumerator StartFadeIn()
    {
        StartCoroutine(FinishInSeconds(2));
        yield return new WaitUntil(() => finished);
        yield return StartCoroutine(fade.FadeOut());
        this.gameObject.SetActive(false);
    }
}
