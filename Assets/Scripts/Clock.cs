using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    private Text textClock;
    DateTime timeStart;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        textClock = GetComponent<Text>();
        timeStart = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        DateTime timeNow = DateTime.Now; 
        string hour = LeadingZero(-(timeStart.Hour - timeNow.Hour));
        string minute = LeadingZero(-(timeStart.Minute - timeNow.Minute));
        string second = LeadingZero(-(timeStart.Second - timeNow.Second));
        string millisecond = LeadingZero(-(timeStart.Millisecond - timeNow.Millisecond));
        textClock.text = hour + ":" + minute + ":" + second + ":" + millisecond;
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
