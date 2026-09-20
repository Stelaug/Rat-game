using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit_script : MonoBehaviour
{
    public int dmg = 20;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
