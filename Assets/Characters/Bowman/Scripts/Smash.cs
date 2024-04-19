using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Smash script");
        Debug.Log(collision.gameObject.tag);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
