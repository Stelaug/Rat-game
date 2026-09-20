using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny_icicle : MonoBehaviour
{
    // Varaiable declarations
    public Vector2 direction;
    private float lifetime = 60f;
    public int damage = 15;
    private PolygonCollider2D col;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = gameObject.GetComponent<PolygonCollider2D>();
        col.enabled = false;
        // Chooses a random rotaion within range
        if (direction.x < 0){
            transform.eulerAngles = Vector3.forward *Random.Range(150, 210);
        }
        else if(direction.x > 0){
            transform.eulerAngles = Vector3.back*Random.Range(-30, 30);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Destroy object after lifetime
        lifetime -= Time.deltaTime;
        if(lifetime <= 0){
            Destroy(gameObject);
        }
        // Moves the icicle
        if(Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Mouse1)){
            GetComponent<Rigidbody2D>().AddForce(transform.TransformDirection(Vector2.right)*Random.Range(200, 400));
            col.enabled = true;
            }
    }
    void OnTriggerEnter2D(Collider2D col){
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(explode());
    }
    public IEnumerator explode(){
        GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("hit", true);
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }
}
