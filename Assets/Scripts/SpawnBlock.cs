using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Blocks;
    public Clock Clock;
    public GameObject previewSpawner;
    private GameObject previewBlock;
    private bool lost = false;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        NewPreviewBlock();
        NewBlock();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NewPreviewBlock()
    {
        if (started)
        {
            Debug.Log("NewPreviewBlock!");
            Destroy(previewBlock);
            previewBlock = Instantiate(Blocks[Random.Range(0, Blocks.Length)], previewSpawner.transform.position, Quaternion.identity);
            TetrisBlock previewScript = previewBlock.GetComponent<TetrisBlock>();
            previewScript.SetStopped(true);
        }
        
    }

    public void NewBlock()
    {
        if (started)
        {
            if (!lost)
            {
                GameObject obj = Instantiate(previewBlock, transform.position, Quaternion.identity);
                TetrisBlock objScript = obj.GetComponent<TetrisBlock>();
                objScript.SetStopped(false);
                NewPreviewBlock();

                Debug.Log("object: " + obj);
                if (!FindObjectOfType<TetrisBlock>(false).ValidMove(obj.transform))
                {
                    Destroy(obj);
                    lost = true;
                    Debug.Log("LOSS");
                    Clock.StopRunning();
                }
            }
        }
        
        
        
    }
}
