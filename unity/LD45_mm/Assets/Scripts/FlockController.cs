using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    Rigidbody2D rb;
    float startPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.position += new Vector3(5.0f, 0.0f, 0.0f) * Time.deltaTime;
        if(rb.transform.localPosition.x > 40) 
        {
            Vector3 v = rb.transform.localPosition;
            v.x = startPos;
            transform.localPosition = v;
        }
    }
}
