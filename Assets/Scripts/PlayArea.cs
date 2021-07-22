using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{

    public GameObject canvasPlayArea;

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVisible(bool show)
    {
        GetComponent<Renderer>().enabled = show;
        canvasPlayArea.SetActive(show);
    }
}
