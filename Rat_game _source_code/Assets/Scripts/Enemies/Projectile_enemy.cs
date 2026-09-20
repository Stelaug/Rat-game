using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_enemy : MonoBehaviour
{
    [SerializeField] private GameObject left_sensor;
    [SerializeField] private GameObject right_sensor;
    [SerializeField] private GameObject bullet;
    private RaycastHit2D leftHit;
    private RaycastHit2D rightHit;
    private float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void shoot(string dir){
        if(dir == "right"){
            bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0));
        }
        else if(dir == "left"){
            bullet = Instantiate(bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
        }
    }

    // Update is called once per frame
    void Update()
    { 
        t+=Time.deltaTime;
        Debug.DrawLine(transform.position, left_sensor.transform.position);
        Debug.DrawLine(transform.position, right_sensor.transform.position);
        leftHit = Physics2D.Linecast(transform.position, left_sensor.transform.position, 1 << LayerMask.NameToLayer("Player"));
        rightHit = Physics2D.Linecast(transform.position, right_sensor.transform.position, 1 << LayerMask.NameToLayer("Player"));
        if(leftHit == true && t >= 2.0f){
            t = 0.0f;
            shoot("right");
        }
        else if(rightHit == true && t >= 2.0f){
            t = 0.0f;
            shoot("left");
        }
    }
}
