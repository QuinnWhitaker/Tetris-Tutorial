using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            CanvasGroup childCG = child.GetComponent<CanvasGroup>();
            childCG.alpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator fadeIn()
    {
        foreach (Transform child in transform)
        {
            FadeCanvasObject childFade = child.GetComponent<FadeCanvasObject>();
            childFade.fadeSpeed = 0.01f;
            childFade.FadeInObject();
            yield return new WaitForSeconds(0.8f);

        }
    }
}
