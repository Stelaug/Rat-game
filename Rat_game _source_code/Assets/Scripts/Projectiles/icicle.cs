using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icicle : MonoBehaviour
{
    // Variable declaration
    public int damage = 40;
    private float lifetime = 10f;
    public float speed = 10f;
    public Vector2 direction;
    private Animator anim;
    void Update(){
        // Destroy object after lifetime
        lifetime -= Time.deltaTime;
        if(lifetime <= 0){
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // Rotates the icicle
        if(direction.y > 0){
            transform.eulerAngles = Vector3.forward *90;
        }
        else if (direction.x < 0){
            transform.eulerAngles = Vector3.forward *180;
        }
        else if(direction.x > 0){
            transform.eulerAngles = Vector3.back*0;
        }
        StartCoroutine(move());  
        

    }
    void OnTriggerEnter2D(Collider2D col){
        // Destroys object upon collision
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(explode());
    }
    private IEnumerator move(){
        // Moves the icicle after a delay
        yield return new WaitForSeconds(1);
        anim.SetBool("flying", true);
        GetComponent<Rigidbody2D>().AddForce(direction*speed);
    }
    public IEnumerator explode(){
        GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("hit", true);
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }
}
