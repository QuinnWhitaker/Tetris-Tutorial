using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Clock : MonoBehaviour
{
    private Text textClock;
    public static Stopwatch timer = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        textClock = GetComponent<Text>();
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        textClock.text = timer.Elapsed.ToString();
    }

    public static void Stop()
    {
        timer.Stop();
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
