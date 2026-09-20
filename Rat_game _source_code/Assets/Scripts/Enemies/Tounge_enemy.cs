using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tounge_enemy : MonoBehaviour
{
    private int health = 200;
    private RaycastHit2D hit_left;
    private RaycastHit2D hit_right;
    private Animator anim;
    private Coroutine fire_coroutine;
    private Coroutine ice_coroutine;
    public int ice_q = 1;
    private Vector3 localscale;
    private Vector3 rightTargetL;
    private Vector3 leftTargetL;
    private Vector3 rightTargetW;
    private Vector3 leftTargetW;

    // Start is called before the first frame update
    void Start()
    {
      anim = gameObject.GetComponent<Animator>();
      rightTargetW = transform.TransformPoint(Vector3.zero);
      leftTargetW = transform.TransformPoint(Vector3.zero);
      rightTargetW = new Vector3(rightTargetW.x+(2.5f*transform.localScale.x), rightTargetW.y, rightTargetW.z);
      leftTargetW = new Vector3(leftTargetW.x-(2.5f*transform.localScale.x), leftTargetW.y, leftTargetW.z);
      
    }

    // Update is called once per frame
    void Update()
    {
      anim.speed = 1f/ice_q;

         hit_left = Physics2D.Linecast(transform.position, leftTargetW, 1 << LayerMask.NameToLayer("Player"));
         Debug.DrawLine(transform.position, leftTargetW);
         if(hit_left){
            anim.SetBool("left", true);
            
         }
         else{
            anim.SetBool("left", false);
         }
         hit_right = Physics2D.Linecast(transform.position, rightTargetW, 1 << LayerMask.NameToLayer("Player"));
         Debug.DrawLine(transform.position, rightTargetW);
         if(hit_right){
            anim.SetBool("right", true);
            
         }
         else{
            anim.SetBool("right", false);
         }
    }
}
