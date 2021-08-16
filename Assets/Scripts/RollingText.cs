using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// This class for a text mesh pro allows the individual characters to have a "rolling" effect
// Whenever the content of the text changes, any characters that "changed" or are removed slide down below and fade out,
// while any characters that replaced a changed one or were newly added slide down from above and fade in
// When the size of the text changes and becomes re-centered, this process is also animated
public class RollingText : MonoBehaviour
{

    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    private TMP_Text m_TextComponent;
    private string currentText;

    private char[] currentTextArray;
    private char[] newTextArray;

    private List<int> rotateOutIndices;
    private List<int> rotateInIndices;

    private string rotateOut;
    private string rotateIn;

    private float rotateSpeed = 0.1f;

    public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 2.0f), new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f), new Keyframe(1, 0f));

    // The lefthand margin will be temporarily changed until a rotation is complete
    // This keeps track of its original value so that it can be replaced when the rotation is finished
    private Vector4 preMargin;

    // The amount of time between each letter rotation
    private float delayTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        StartCoroutine(CoroutineCoordinator());
    }

    void OnEnable()
    {
        m_TextComponent.ForceMeshUpdate();
    }

    void ResetGeometry()
    {
        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var newVertexPositions = textInfo.meshInfo[i].vertices;

            // Upload the mesh with the revised information
            UpdateMesh(newVertexPositions, 0);
        }

        m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
    }
    private void UpdateMesh(Vector3[] _vertex, int index)
    {
        m_TextComponent.mesh.vertices = _vertex;
        m_TextComponent.mesh.uv = m_TextComponent.textInfo.meshInfo[index].uvs0;
        m_TextComponent.mesh.uv2 = m_TextComponent.textInfo.meshInfo[index].uvs2;
        m_TextComponent.mesh.colors32 = m_TextComponent.textInfo.meshInfo[index].colors32;
    }

    // Adds a given character index to the list of characters to be rotated out
    private void AddToOut(int i)
    {
        rotateOutIndices.Add(i);
        rotateOut += currentTextArray[i];
    }

    // Adds a given character index to the list of characters to be rotated in
    private void AddToIn(int i)
    {
        rotateInIndices.Add(i);
        rotateIn += newTextArray[i];
    }

    // Takes a float between 0 and 1 and converts it to a byte between 0 and 255
    private byte PercentageToByte(float percentage)
    {
        if (percentage > 1)
        {
            percentage = 1;
        }
        else if (percentage < 0)
        {
            percentage = 0;
        }

        byte result = Convert.ToByte(255 * percentage);
        return result;
    }

    // Returns a float between 0 and 1 that a midpoint float x is on a scale between min and max
    private float GetLinearY(float min, float max, float x)
    {
        float percentage = System.Math.Abs(
            (x - min) / (max - min)
        );

        return percentage;
    }

    private IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (coroutineQueue.Count > 0)
            {
                yield return StartCoroutine(coroutineQueue.Dequeue());
            }
            yield return null;
        }
    }


    // Each character in the current string that is set to rotate in or out will do so, with a slight delay
    // Characters rotating on the same index will do so at the same time
    private IEnumerator RotateSet(string newText)
    {
        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        m_TextComponent.ForceMeshUpdate();

        // Getting the width and height of the text pre-addition
        Vector2 preSize = m_TextComponent.GetRenderedValues(true);

        //Debug.Log("preWidth: " + preWidth.x);

        // Getting the margin pre-addition
        preMargin = m_TextComponent.margin;

        Debug.Log("preMargin: " + preMargin);

        // Making the addition
        m_TextComponent.text = currentText + newText;

        Debug.Log("text now: " + m_TextComponent.text);

        m_TextComponent.ForceMeshUpdate();

        // Getting the width of the text post-addition
        float postWidth = m_TextComponent.GetRenderedValues(true).x;

        // Debug.Log("postWidth: " + postWidth);


        // The lefthand margin is shifted to the difference between the pre and post width
        Vector4 postMargin = preMargin;
        postMargin.x += postWidth - preSize.x;

        m_TextComponent.margin = postMargin;


        // Move all characters in the new string above the old string

        // Initialize the move
        VertexCurve.preWrapMode = WrapMode.Loop;
        VertexCurve.postWrapMode = WrapMode.Loop;
        Vector3[] newVertexPositions;
        Color32[] newVertexColors;

        bool initialized = false;
        newVertexPositions = textInfo.meshInfo[0].vertices;


        // The time that the rotation started
        float startTime = Time.time;

        // The points in which each character type starts moving
        float startPoint_currentText = newVertexPositions[textInfo.characterInfo[0].vertexIndex].y;
        float startPoint_newText = newVertexPositions[textInfo.characterInfo[0].vertexIndex].y + preSize.y;

        // The points in which each character type should stop moving

        // stopPoint current is a point exactly y distance below the current Text, where y is the height of the text
        // This is where fading out text should stop
        float stopPoint_currentText = newVertexPositions[textInfo.characterInfo[0].vertexIndex].y - preSize.y;

        // stopPoint new is where the old text used to be.
        // This is where fading in text should stop
        float stopPoint_newText = newVertexPositions[textInfo.characterInfo[0].vertexIndex].y;

        //Debug.Log("startCurrent: " + startPoint_currentText + ", stopCurrent: " + stopPoint_currentText + ", startNew: " + startPoint_newText + ", stopNew: " + stopPoint_newText);



        while (rotateOutIndices.Count > 0 || rotateInIndices.Count > 0)
        {
            textInfo = m_TextComponent.textInfo;

            if (!initialized)
            {
                // Initializing the rotation by adding the new string onto the current one, and shifting it to the top so that it can fall downwards

                m_TextComponent.renderMode = TextRenderFlags.DontRender;
                ResetGeometry();

                // Get the current vertex information of each character

                newVertexPositions = textInfo.meshInfo[0].vertices;
                newVertexColors = textInfo.meshInfo[0].colors32;

                // Only go through the characters in the new string
                for (int i = currentTextArray.Length; i < currentTextArray.Length + newTextArray.Length; i++)
                {
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Shift the character x distance left and y distance up,
                    // Where x is the width of the string pre-addition, and y is the height of the string pre-addition

                    // Set the alpha of the letter to 0

                    for (int j = 0; j < 4; j++)
                    {
                        newVertexPositions[vertexIndex + j].x -= preSize.x;
                        newVertexPositions[vertexIndex + j].y += preSize.y;

                        Color32 newColor = newVertexColors[vertexIndex + j];
                        newColor.a = 0;
                        newVertexColors[vertexIndex + j] = newColor;
                    }

                }

                //ResetGeometry();

                initialized = true;
            }
            else
            {
                newVertexPositions = textInfo.meshInfo[0].vertices;
                newVertexColors = textInfo.meshInfo[0].colors32;

                for (int i = 0; i < System.Math.Max(currentTextArray.Length, newTextArray.Length); i++)
                {
                    int vertexIndex_currentText = textInfo.characterInfo[i].vertexIndex;
                    int vertexIndex_newText = textInfo.characterInfo[i + currentTextArray.Length].vertexIndex;

                    //bool shouldStop_currentText = newVertexPositions[vertexIndex_currentText].y <= stopPoint_currentText;
                    //bool shouldStop_newText = newVertexPositions[vertexIndex_newText].y <= stopPoint_newText;

                    byte alpha_currentText = 255;
                    byte alpha_newText = 0;

                    if (newVertexPositions[vertexIndex_currentText].y <= stopPoint_currentText + (rotateSpeed / 2))
                    {
                        rotateOutIndices.Remove(i);
                        alpha_currentText = 0;
                    }
                    else
                    {
                        alpha_currentText = PercentageToByte(GetLinearY(stopPoint_currentText, startPoint_currentText, newVertexPositions[vertexIndex_currentText].y));
                    }
                    if (newVertexPositions[vertexIndex_newText].y <= stopPoint_newText + (rotateSpeed / 2))
                    {
                        rotateInIndices.Remove(i);
                        alpha_newText = 255;
                    }
                    else
                    {
                        alpha_newText = PercentageToByte(GetLinearY(startPoint_newText, stopPoint_newText, newVertexPositions[vertexIndex_newText].y));
                    }

                    bool isRotatingIn = rotateInIndices.Contains(i);
                    bool isRotatingOut = rotateOutIndices.Contains(i);

                    // Each character drops one at a time, with a delay between them equal to delay time
                    bool shouldStart = (Time.time >= (startTime + delayTime * i));


                    float rotationSpeed_newText = rotateSpeed * GetLinearY(stopPoint_newText, startPoint_newText, newVertexPositions[vertexIndex_newText].y);
                    float rotationSpeed_currentText = rotateSpeed * GetLinearY(stopPoint_currentText, startPoint_currentText, newVertexPositions[vertexIndex_currentText].y);

                    for (int j = 0; j < 4; j++)
                    {
                        if (shouldStart)
                        {
                            if (isRotatingIn)
                            {
                                newVertexPositions[vertexIndex_newText + j].y -= rotationSpeed_newText;
                                newVertexColors[vertexIndex_newText + j].a = alpha_newText;
                            }
                            if (isRotatingOut)
                            {
                                newVertexPositions[vertexIndex_currentText + j].y -= rotationSpeed_currentText;
                                newVertexColors[vertexIndex_currentText + j].a = alpha_currentText;
                            }
                            if (alpha_newText == 255)
                            {
                                newVertexColors[vertexIndex_currentText + j].a = 255;
                            }
                            if (alpha_currentText == 0)
                            {
                                newVertexColors[vertexIndex_currentText + j].a = 0;
                            }
                        }
                    }
                }
            }

            /*
            if (m_TextComponent.margin.x > preMargin.x)
            {
                Vector4 tempMargin = m_TextComponent.margin;
                tempMargin.x = tempMargin.x - 0.03f;
                m_TextComponent.margin = tempMargin;
            }
            */

            // Apply the change

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textInfo.meshInfo[i].mesh.colors32 = newVertexColors;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("<color=red><b>DONE</b></color>");
        m_TextComponent.text = newText;
        //Debug.Log("m_TextComponent.text: " + m_TextComponent.text);
        m_TextComponent.margin = preMargin;
        //Debug.Log("m_TextComponent.margin: " + m_TextComponent.margin);
        ResetGeometry();

    }

    public void SetText(string newText)
    {
        coroutineQueue.Enqueue(SetTextEnumerator(newText));
    }

    public IEnumerator SetTextEnumerator(string newText)
    {
        currentText = m_TextComponent.text;

        if (newText != currentText)
        {
            currentTextArray = currentText.ToCharArray();
            newTextArray = newText.ToCharArray();

            // Compare the two strings. Determine which characters need to be rotated out and rotated in

            // Contains the indices of the characters in the current String that need to rotate out
            rotateOutIndices = new List<int>();
            rotateOut = "";

            // Contains the indices of the characters in the new string that need to rotate in
            rotateInIndices = new List<int>();
            rotateIn = "";

            for (int i = 0; i < System.Math.Max(currentTextArray.Length, newTextArray.Length); i++)
            {
                bool inCurrent = (i < currentTextArray.Length);
                bool inNew = (i < newTextArray.Length);

                if (inCurrent && inNew)
                {
                    /*
                    if (!currentTextArray[i].Equals(newTextArray[i]))
                    {
                        AddToOut(i);
                        AddToIn(i);
                    }
                    */
                    AddToOut(i);
                    AddToIn(i);

                }
                else if (inCurrent && !inNew)
                {
                    AddToOut(i);
                }
                else if (!inCurrent && inNew)
                {
                    AddToIn(i);
                }
            }

            // Each character set to rotate out will fade out and move downwards

            yield return StartCoroutine(RotateSet(newText));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
