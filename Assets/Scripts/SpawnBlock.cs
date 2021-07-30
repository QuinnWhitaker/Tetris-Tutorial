using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Blocks;
    public GameObject previewSpawner;
    private GameObject previewBlock;
    public GameRunner gameRunner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        NewPreviewBlock();
        NewBlock();
        TetrisBlock newBlock = FindObjectOfType<TetrisBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NewPreviewBlock()
    {
        Debug.Log("NewPreviewBlock!");
        Destroy(previewBlock);
        previewBlock = Instantiate(Blocks[Random.Range(0, Blocks.Length)], previewSpawner.transform.position, Quaternion.identity);
        TetrisBlock previewScript = previewBlock.GetComponent<TetrisBlock>();
        previewScript.SetStopped(true);
    }

    public void NewBlock()
    {
        if (gameRunner.GetStatus() == GameRunner.gameState.Started)
        {
            GameObject obj = Instantiate(previewBlock, transform.position, Quaternion.identity);
            gameRunner.SetCurrentBlock(obj);
            TetrisBlock objScript = obj.GetComponent<TetrisBlock>();
            objScript.SetStopped(false);
            NewPreviewBlock();

            Debug.Log("object: " + obj);
            if (!objScript.ValidMove(obj.transform))
            {
                Destroy(obj);
                gameRunner.ClearCurrentBlock();
                gameRunner.ClearCurrentGhost();
                StartCoroutine(gameRunner.EndGame());
            }
        }
        
        
        
    }
}
