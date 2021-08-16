using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    // The actual origina of the cmaera. Go here when resetting games
    Transform originPoint;

    // When the camera slightly shifts while moving a piece, it goes to this variable point
    Transform focusPoint;

    // As long as this is greater than 0
    float force = 0f;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform;
        focusPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns a float between 0 and 1 that a midpoint float x is on a scale between min and max
    private float GetLinearY(float min, float max, float x)
    {
        float percentage = System.Math.Abs(
            (x - min) / (max - min)
        );

        return percentage;
    }

    public void Strike(float initialForce)
    {
        force = initialForce;
    }

    private IEnumerator Recovering()
    {
        while (true)
        {
            if (force > 0)
            {

            }
            if (transform.position.y != focusPoint.position.y)
            {

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
