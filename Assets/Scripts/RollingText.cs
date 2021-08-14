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
    private float delayTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
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

    // Each character in the current string that is set to rotate in or out will do so, with a slight delay
    // Characters rotating on the same index will do so at the same time
    private IEnumerator RotateSet(string newText)
    {

        m_TextComponent.ForceMeshUpdate();

        // Getting the width and height of the text pre-addition
        Vector2 preSize = m_TextComponent.GetRenderedValues(true);

        //Debug.Log("preWidth: " + preWidth.x);

        // Getting the margin pre-addition
        preMargin = m_TextComponent.margin;

        //Debug.Log("preMargin: " + preMargin.x);

        // Making the addition
        m_TextComponent.text = currentText + newText;


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

        bool initialized = false;
        float totalMoved = 0f;

        Debug.Log("presize y =" + preSize.y);
        while (totalMoved < preSize.y)
        {
            TMP_TextInfo textInfo = m_TextComponent.textInfo;

            if (!initialized)
            {
                m_TextComponent.renderMode = TextRenderFlags.DontRender;
                ResetGeometry();


                // Get the current vertex information of each character

                
                newVertexPositions = textInfo.meshInfo[0].vertices;
                // Only go through the characters in the new string
                for (int i = currentTextArray.Length; i < currentTextArray.Length + newTextArray.Length; i++)
                {
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Shift the character x distance left and y distance up,
                    // Where x is the width of the string pre-addition, and y is the height of the string pre-addition

                    for (int j = 0; j < 4; j++)
                    {
                        newVertexPositions[vertexIndex + j].x -= preSize.x;
                        newVertexPositions[vertexIndex + j].y += preSize.y;
                    }

                }

                initialized = true;
            } else
            {
                newVertexPositions = textInfo.meshInfo[0].vertices;
                for (int i = 0; i < System.Math.Max(currentTextArray.Length, newTextArray.Length); i++)
                {
                    int vertexIndex_currentText = textInfo.characterInfo[i].vertexIndex;
                    int vertexIndex_newText = textInfo.characterInfo[i + currentTextArray.Length].vertexIndex;

                    bool isRotatingIn  = rotateInIndices.Contains(i);
                    bool isRotatingOut = rotateOutIndices.Contains(i);

                    // Shift the character x distance left and y distance up,
                    // Where x is the width of the string pre-addition, and y is the height of the string pre-addition

                    for (int j = 0; j < 4; j++)
                    {

                        if (isRotatingIn)
                        {
                            newVertexPositions[vertexIndex_newText + j].y -= rotateSpeed;
                        }
                        if (isRotatingOut)
                        {
                            newVertexPositions[vertexIndex_currentText + j].y -= rotateSpeed;
                        }

                        totalMoved += rotateSpeed;

                    }

                }
            }

            // Apply the change

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds(0.01f);
        }

        //m_TextComponent.text = newText;
        //m_TextComponent.margin = preMargin;

    }

    // Adds the new string to the old one, and if the textmeshpro is centered, re-adjusts the offset that this causes
    // Then takes the newly added string, and instantly moves each character from it above the one it will be replacing
    // If it is not rotating in, it will be set to invisible
    private IEnumerator InitRotation(string newText)
    {
        yield return null;
    }

    public void SetText(string newText)
    {
        StartCoroutine(SetTextIEnumerator(newText));
    }

    public IEnumerator SetTextIEnumerator(string newText)
    {
        currentText = m_TextComponent.text;

        if (newText != currentText)
        {
            currentTextArray = currentText.ToCharArray();
            newTextArray = newText.ToCharArray();

            yield return StartCoroutine(InitRotation(newText));

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
                    if (!currentTextArray[i].Equals(newTextArray[i]))
                    {
                        AddToOut(i);
                        AddToIn(i);
                    }

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

            StartCoroutine(RotateSet(newText));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
