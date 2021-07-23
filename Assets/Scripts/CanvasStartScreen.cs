using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStartScreen : MonoBehaviour
{
    private FadeCanvasObject fade;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            CanvasGroup childCG = child.GetComponent<CanvasGroup>();
            childCG.alpha = 0;
        }
        fade = this.GetComponent<FadeCanvasObject>();
        fadeIn();
    }

    // Update is called once per frame
    void Update()
    {

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
        yield return new WaitForSeconds(7);

        foreach (Transform child in transform)
        {
            FadeCanvasObject childFade = child.GetComponent<FadeCanvasObject>();
            childFade.fadeSpeed = 0.01f;
            childFade.FadeInObject();
            if (child.name.Equals("Logo"))
            {
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.8f);
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
