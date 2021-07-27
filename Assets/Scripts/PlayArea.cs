using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{

    public FadeObject fade;

    // Start is called before the first frame update
    void Start()
    {
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 0;
        this.GetComponent<SpriteRenderer>().color = c;
        fade = this.GetComponent<FadeObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator fadeIn()
    {
        yield return StartCoroutine(fade.FadeIn());
    }
}
