using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool active = false;
    public bool groundActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == 6){
            active = true;       
        }
        else if(col.gameObject.layer == 3){
            groundActive = true;       
        }
    }
    private void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.layer == 6){
            active = false;       
        }
        else if(col.gameObject.layer == 3){
            groundActive = false;       
        }
    }
}
