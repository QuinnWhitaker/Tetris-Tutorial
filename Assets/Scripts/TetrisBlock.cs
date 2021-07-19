using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height+1];

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Initializing TetrisBlock Script!");
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttemptMove(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AttemptMove(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            AttemptRotate(90);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptRotate(-90);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
        {
            AttemptFall(0, -1, 0);
            previousTime = Time.time;
        }
    }

    // Makes a move in the given direction, then checks to see if the move is valid. If not, it reverts the change.
    void AttemptMove(float xMove, float yMove, float zMove)
    {
        //Debug.Log("Attempting move: " + xMove + ", " + yMove + ", " + zMove);
        transform.position += new Vector3(xMove, yMove, zMove);
        if (!ValidMove(transform))
        {
            //Debug.Log("Undoing move as: " + -xMove + ", " + -yMove + ", " + -zMove);
            transform.position += new Vector3(-xMove, -yMove, -zMove);
        }
    }

    // Almost identical to AttemptMove. If the move is not valid however, it reverts the change, disables this script, and spawns a new block.
    void AttemptFall(float xMove, float yMove, float zMove)
    {
        //Debug.Log("Attempting move: " + xMove + ", " + yMove + ", " + zMove);
        transform.position += new Vector3(xMove, yMove, zMove);
        if (!ValidMove(transform))
        {
            //Debug.Log("Undoing move as: " + -xMove + ", " + -yMove + ", " + -zMove);
            transform.position += new Vector3(-xMove, -yMove, -zMove);
            AddToGrid();
            CheckForLines();
            this.enabled = false;
            FindObjectOfType<SpawnBlock>().NewBlock();
        }
    }

    // Makes a rotation in the given direction, then checks to see if the move is valid. If not, it reverts the change.
    [System.Obsolete]
    void AttemptRotate(float degrees)
    {
        //Debug.Log("Attempting rotation: " + degrees + " degrees, " + rotationPoint.x + ", " + rotationPoint.y + ", " + rotationPoint.z);
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), degrees);
        if (!ValidMove(transform))
        {
            //Debug.Log("Undoing rotation as: " + -degrees + " degrees, " + rotationPoint.x + ", " + rotationPoint.y + ", " + rotationPoint.z);
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -degrees);
        }
    }

    void CheckForLines()
    {
        int numRows = 0;
        for (int i = height-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                numRows++;
                DeleteLine(i);
                RowDown(i);
            }
        }
        Debug.Log("Number of lines: " + numRows);
        FindObjectOfType<ScoreTracker>().addToScore(numRows);
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            float roundedX = child.transform.position.x;
            float roundedY = child.transform.position.y;

            grid[(int)roundedX, (int)roundedY] = child;
            Debug.Log("X: " + (int)roundedX + " Y: " + (int)roundedY);
        }
    }
    // Sees if a move was valid by checking each of the child squares and ensuring they are in bounds
    public bool ValidMove(Transform thisTransform)
    {
        // Debug.Log("Confirming valid move...");
        // For each child object
        foreach (Transform child in thisTransform)
        {
            // Get its coordinates in rounded form
            float roundedX = child.transform.position.x;
            float roundedY = child.transform.position.y;

            //Debug.Log("Coordinates: " + roundedX + ", " + roundedY);

            // If any of the coordinates go out of bounds, return false
            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height+1)
            {
                //Debug.Log("Out of bounds");
                return false;
            }

            if (grid[(int)roundedX, (int)roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
        
}