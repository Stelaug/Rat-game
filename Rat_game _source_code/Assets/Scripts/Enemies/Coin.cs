using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3){
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 6){
            PlayerPrefs.SetInt("bal", PlayerPrefs.GetInt("bal") +1);
            other.gameObject.GetComponent<PlayerStatus>().PlayCoinPickup();
            Destroy(gameObject);
        }

    }
}
