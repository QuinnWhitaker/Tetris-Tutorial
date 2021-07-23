using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFadeInScreen : MonoBehaviour
{
    private FadeCanvasObject fade;
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        fade = this.GetComponent<FadeCanvasObject>();
        StartCoroutine("StartFadeIn");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartFadeIn()
    {
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(fade.FadeOut());
        this.gameObject.SetActive(false);
    }
}
