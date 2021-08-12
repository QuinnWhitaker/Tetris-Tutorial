using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class for a text mesh pro allows the individual characters to have a "rolling" effect
// Whenever the content of the text changes, any characters that "changed" or are removed slide down below and fade out,
// while any characters that replaced a changed one or were newly added slide down from above and fade in
// When the size of the text changes and becomes re-centered, this process is also animated
public class RollingText : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        textMeshPro = this.GetComponent<TextMeshPro>();
        textMeshPro.ForceMeshUpdate();
    }

    void SetText(string newText)
    {
        string currentText = textMeshPro.text;

        // Compare the two strings. Determine which characters need to be rotated out and rotated in

        for (int i = 0; i < currentText.Length; i++)
        {
            // TODO: FINISH
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
