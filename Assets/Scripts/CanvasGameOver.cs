using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetChildrenNoAlpha();
    }

    private void OnEnable()
    {
        SetChildrenNoAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetChildrenNoAlpha()
    {
        foreach (Transform child in transform)
        {
            CanvasGroup childCG = child.GetComponent<CanvasGroup>();
            childCG.alpha = 0;
        }
    }

    public IEnumerator fadeIn()
    {
        foreach (Transform child in transform)
        {
            FadeObject childFade = child.GetComponent<FadeObject>();
            childFade.fadeSpeed = 0.01f;
            childFade.FadeInObject();
            yield return new WaitForSeconds(0.8f);

        }
    }
}
