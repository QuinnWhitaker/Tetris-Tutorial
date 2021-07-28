using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Clock : MonoBehaviour
{
    private Text textClock;
    public static Stopwatch timer = new Stopwatch();
    public GameRunner gameRunner;
    // Start is called before the first frame update
    void Start()
    {
        textClock = GetComponent<Text>();
    }
    private void Awake()
    {
        
    }

    public void StartTimer()
    {
        timer.Start();
    }

    public void ResetTimer()
    {
        timer.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        textClock.text = (timer.Elapsed.ToString().Remove(11));
    }
}
