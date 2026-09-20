using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorClose : MonoBehaviour
{
    public Animator Door;
    [SerializeField] private GameObject FK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        FK.SetActive(true);
        Door.SetBool("playerPassed", true);
        Door.SetBool("doorOpening", false);
    }
}
