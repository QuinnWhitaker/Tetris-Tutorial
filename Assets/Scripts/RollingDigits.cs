using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingDigits : MonoBehaviour
{
    public GameObject referenceText;
    private string currentValue;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = referenceText.GetComponent<Text>().text;
        createChars();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Create and position each individual text object for each character in the string
    void createChars()
    {
        foreach (char letter in currentValue.ToCharArray())
        {
            GameObject newLetter = Instantiate(referenceText, referenceText.gameObject.transform.position, Quaternion.identity);
            Debug.Log("Character: " + letter);
            newLetter.transform.SetParent(this.GetComponent<Canvas>().transform);
        }
    }

    void changeTo(string newValue)
    {
        // Make changes

        currentValue = newValue;
    }
}
