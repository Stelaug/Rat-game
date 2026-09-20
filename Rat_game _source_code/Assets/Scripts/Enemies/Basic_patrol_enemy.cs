using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_patrol_enemy : MonoBehaviour
{
    // Variable declarations
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool knockbackDelay = false;
    private int health = 100;
    public int ice_q = 1;
    private float speed = -2f;
    private Coroutine fire_coroutine;
    private Coroutine ice_coroutine;
    [SerializeField] private GameObject Sensor;
    private bool paused = false;
    private bool noFlip = false;
    public float offset = 100000000.2f;
    private Animator anim;
    Vector3 localscale;
    // Update is called once per frame
    void Update()
    {
        anim = GetComponent<Animator>();
        anim.speed = 1f/ice_q;
        
        if(Sensor.GetComponent<Sensor>().groundActive && !noFlip){
            StartCoroutine(StopFlip());
            StartCoroutine(flip());
            
        }
        // Destroys the enemy when it dies
        if(health <= 0){
            Destroy(gameObject);
        }
        // Movement
        if(!knockbackDelay && isGrounded() && !paused){
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed/ice_q, 0);
            }
    }
    private bool isGrounded(){
        // Checks if the enemy is on the ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void OnTriggerExit2D(Collider2D col){
        // Flips the enemy when it has reached the end of the platform
        if(col.gameObject.layer == 3 && !noFlip){
            StartCoroutine(flip());
        }
    }
    private IEnumerator flip(){
        paused = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.15f);
        speed*=-1;
        localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
        paused = false;
    }
    private IEnumerator StopFlip(){
        noFlip = true;
        yield return new WaitForSeconds(0.3f);
        noFlip = false;

    }
    

}
