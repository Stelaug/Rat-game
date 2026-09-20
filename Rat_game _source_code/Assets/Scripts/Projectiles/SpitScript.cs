using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitScript : MonoBehaviour
{
    private PlayerStatus playerScript;
    public int dmg = 10;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerStatus>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(6))
        {
            playerScript.health -= dmg;
        }
    }
}
