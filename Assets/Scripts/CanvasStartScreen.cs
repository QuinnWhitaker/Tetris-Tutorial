using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasStartScreen : MonoBehaviour
{
    private FadeCanvasObject fade;
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        fade = this.GetComponent<FadeCanvasObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            fade.FadeInObject();
        }
    }
}
