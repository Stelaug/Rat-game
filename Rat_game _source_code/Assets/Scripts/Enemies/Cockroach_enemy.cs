using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockroach_enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private RaycastHit2D edgeHit;
    private RaycastHit2D wallHit;
    private Animator anim;
    private bool paused = false;
    private float time = 0f;
    private Vector3 localscale;
    public int ice_q = 1;
    public bool knockbackDelay = false;
    [SerializeField] private GameObject wallTarget;
    [SerializeField] private GameObject edgeTarget;
    [SerializeField] private GameObject wallOrigin;
    [SerializeField] private GameObject edgeOrigin;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private float speed = 2f;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;
        if(!paused && !knockbackDelay && isGrounded()){gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed/ice_q, 0);}
        Debug.DrawLine(wallOrigin.transform.position, wallTarget.transform.position);
        Debug.DrawLine(edgeOrigin.transform.position, edgeTarget.transform.position);
        edgeHit = Physics2D.Linecast(edgeOrigin.transform.position, edgeTarget.transform.position);
        wallHit = Physics2D.Linecast(wallOrigin.transform.position, wallTarget.transform.position, 1 << LayerMask.NameToLayer("Ground"));
        if(edgeHit == false && time > 0.5f && isGrounded()){
            StartCoroutine(flip());
            time = 0;
        }
        if(wallHit == true && time > 0.5f && isGrounded()){
            StartCoroutine(flip());
            time = 0;
        }
    }
    void FixedUpdate(){
        
    }
    private IEnumerator flip(){
        paused = true;
        anim.speed = 0.0f;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        speed*=-1;
        localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
        anim.speed = 1.0f/ice_q;
        paused = false;
    }
    private bool isGrounded(){
        // Checks if the enemy is on the ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
