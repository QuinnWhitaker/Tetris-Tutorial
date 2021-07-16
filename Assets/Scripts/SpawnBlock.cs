using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Blocks;
    private bool lost = false;

    // Start is called before the first frame update
    void Start()
    {
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewBlock()
    {
        if (!lost)
        {
            GameObject obj = Instantiate(Blocks[Random.Range(0, Blocks.Length)], transform.position, Quaternion.identity);

            if (!FindObjectOfType<TetrisBlock>().ValidMove(obj.transform))
            {
                Destroy(obj);
                lost = true;
                Debug.Log("LOSS");
            }
        }
        
        
    }
}
