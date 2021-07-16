using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    private static int score = 0;
    private static int level = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScore()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = "SCORE: " + score;
    }

    public void addToScore(int lines)
    {
        switch (lines)
        {
            default:
                break;
            case 1:
                score += (level + 1) * 40;
                break;
            case 2:
                score += (level + 1) * 100;
                break;
            case 3:
                score += (level + 1) * 300;
                break;
            case 4:
                score += (level + 1) * 1200;
                break;
        }

        UpdateScore();
    }
}
