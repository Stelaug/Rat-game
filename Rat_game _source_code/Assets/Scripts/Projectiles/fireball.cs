using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    // Variable declarations
    public int damage = 35;
    public float speed = 10f;
    private float lifetime = 10f;
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
        if(direction.y > 0){
            transform.eulerAngles = Vector3.forward *90;
        }
        if(direction.y < 0){
            transform.eulerAngles = Vector3.forward *-90;
        }
        else if (direction.x < 0){
            transform.eulerAngles = Vector3.forward *180;
            transform.position = new Vector3(transform.position.x, transform.position.y -0.235f, transform.position.z);
            
        }
        else if(direction.x > 0){
            transform.eulerAngles = Vector3.back*0;
        }
        // Moves the fireball
        GetComponent<Rigidbody2D>().AddForce(direction*speed);   
    }
    void OnTriggerEnter2D(Collider2D col){
        // Destroys the object upon collision 
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(explode());
        
    } 
    public IEnumerator explode(){
        GetComponent<CircleCollider2D>().enabled = false;
        anim.SetBool("hit", true);
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}
