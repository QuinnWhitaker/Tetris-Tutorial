using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.8f;
    public bool stopped = false;
    public static GameRunner gameRunner;
    public GameObject ghostRoot;
    public Sprite[] squareSprites;
    private GameObject ghost = null;

    // Start is called before the first frame update
    void Start()
    {
        gameRunner = FindObjectOfType<GameRunner>();
        GenerateSprites();
    }

    private void OnEnable()
    {
        gameRunner = FindObjectOfType<GameRunner>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (gameRunner.GetStatus() == GameRunner.gameState.Started && !stopped)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AttemptMove(-1, 0, 0);
                UpdateGhost();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                AttemptMove(1, 0, 0);
                UpdateGhost();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                AttemptRotate(90);
                UpdateGhost();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptRotate(-90);
                UpdateGhost();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = ghost.transform.position;
                UpdateGhost();
            }

            if (Time.time - previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
            {
                AttemptFall(0, -1, 0);
                previousTime = Time.time;
                UpdateGhost();
            }
        }
    }
    void UpdateGhost()
    {
        Destroy(ghost);

        if (!stopped)
        {
            ghost = Instantiate(ghostRoot, transform.position, transform.rotation);
            gameRunner.SetCurrentGhost(ghost);
            while (ValidMove(ghost.transform))
            {
                ghost.transform.position += new Vector3(0, -1, 0);
            }
            ghost.transform.position += new Vector3(0, 1, 0);

            foreach (Transform ghostChild in ghost.transform)
            {
                float roundedX = ghostChild.transform.position.x;
                float roundedY = ghostChild.transform.position.y;

                foreach (Transform mainChild in transform)
                {
                    if (roundedX == mainChild.transform.position.x &&
                        roundedY == mainChild.transform.position.y)
                    {
                        Destroy(ghostChild.gameObject);
                    }
                }
            }
        }
        
    }

    void GenerateSprites()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = squareSprites[Random.Range(0, squareSprites.Length)];
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
        for (int i = gameRunner.GetHeight()-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                numRows++;
                DeleteLine(i);
                RowDown(i);
            }
        }
        //Debug.Log("Number of lines: " + numRows);
        gameRunner.AddToScore(numRows);
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < gameRunner.GetWidth(); j++)
        {
            if (gameRunner.GetGrid(j, i) == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < gameRunner.GetWidth(); j++)
        {
            gameRunner.DestroyGrid(j, i);
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < gameRunner.GetHeight(); y++)
        {
            for (int j = 0; j < gameRunner.GetWidth(); j++)
            {
                if (gameRunner.GetGrid(j, y) != null)
                {
                    gameRunner.SetGrid(j, y - 1, gameRunner.GetGrid(j, y));
                    gameRunner.SetGrid(j, y, null);
                    gameRunner.MoveGrid(j, y - 1, new Vector3(0, -1, 0));
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

            gameRunner.SetGrid((int)roundedX, (int)roundedY, child);
            //Debug.Log("X: " + (int)roundedX + " Y: " + (int)roundedY);
        }

        CheckForLines();
        this.enabled = false;
        stopped = true;
        FindObjectOfType<SpawnBlock>().NewBlock();
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
            if (roundedX < 0 || roundedX >= gameRunner.GetWidth() || roundedY < 0 || roundedY >= gameRunner.GetHeight()+1)
            {
                //Debug.Log("Out of bounds");
                return false;
            }

            if (gameRunner.GetGrid((int)roundedX, (int)roundedY) != null)
            {
                return false;
            }
        }

        return true;
    }

    public void SetStopped(bool stop)
    {
        stopped = stop;
    }
}
