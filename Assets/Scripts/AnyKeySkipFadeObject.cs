using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeySkipFadeObject : MonoBehaviour
{
    FadeObject fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = this.GetComponent<FadeObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            fade.skipFade();
        }
    }
}
