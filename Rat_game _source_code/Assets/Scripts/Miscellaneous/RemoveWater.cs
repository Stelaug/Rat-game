using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("pipe_opened") == 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
