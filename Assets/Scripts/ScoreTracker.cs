using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    private static int score = 0;
    private static int level = 0;
    private UnityEngine.UI.Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = gameObject.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScore()
    {
        scoreText.text = "" + score;
    }

    public void addToScore(int lines)
    {
        int increase = 0;
        switch (lines)
        {
            default:
                break;
            case 1:
                increase = (level + 1) * 40;
                break;
            case 2:
                increase = (level + 1) * 100;
                break;
            case 3:
                increase = (level + 1) * 300;
                break;
            case 4:
                increase = (level + 1) * 1200;
                break;
        }

        score += increase;
        //Debug.Log("increasing score by " + increase);

        UpdateScore();
    }
}
