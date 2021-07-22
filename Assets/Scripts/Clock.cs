using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Clock : MonoBehaviour
{
    private Text textClock;
    public static Stopwatch timer = new Stopwatch();
    private bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        textClock = GetComponent<Text>();
        SetVisible(false);
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            textClock.text = (timer.Elapsed.ToString().Remove(11));
        }
    }

    public void StopRunning()
    {
        running = false;
        timer.Stop();
    }

    public void StartRunning()
    {
        running = true;
        timer.Start();
    }

    public void SetVisible(bool show)
    {
        textClock.gameObject.GetComponent<Renderer>().enabled = show;
    }
}
