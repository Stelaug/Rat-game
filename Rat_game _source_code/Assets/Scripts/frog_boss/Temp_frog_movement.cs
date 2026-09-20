using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;

public class Temp_frog_movement : MonoBehaviour
{
    [SerializeField] private Transform dp_1;
    [SerializeField] private Transform dp_2;
    [SerializeField] private Transform dp_3;
    [SerializeField] private Transform dp_4;
    [SerializeField] private Transform dp_5;
    [SerializeField] private Transform dp_6;
    [SerializeField] private Transform dp_7;
    [SerializeField] private Transform dp_8;
    [SerializeField] private Transform dp_9;
    [SerializeField] private Transform dp_10;
    [SerializeField] private Transform dp_11;
    [SerializeField] private Transform dp_12;
    [SerializeField] private Transform dp_13;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    private List<Transform> dropPoints;
    private float cooldown = 3.0f;
    private int moveCounter = 0;
    public int ice_q = 1;
    private int phase = 1;

    //Spit
    [SerializeField] private Transform spitter;
    [SerializeField] private float angle;
    [SerializeField] private GameObject projectile;
    private GameObject spit;
    private int directionalMultiplyer;

    void Start()
    {
        dropPoints = new List<Transform>(){dp_1, dp_2, dp_3, dp_4, dp_5, dp_6, dp_7, dp_8, dp_9, dp_10, dp_11, dp_12, dp_13};
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<EnemyStatus>().health < 1001){
            phase = 2;
        }
        angle = Vector2.SignedAngle(spitter.position, player.transform.position - spitter.position);
        cooldown += Time.deltaTime;

        if (player.position.x > transform.position.x){
            directionalMultiplyer = -1;
        }
        else{
            directionalMultiplyer = 1;
        }

        transform.localScale = new Vector3(2 * directionalMultiplyer, transform.localScale.y, transform.localScale.z);

        if (cooldown >= 7f && phase == 2)
        {
            StartCoroutine(Spit());
            cooldown = 0.0f;
        }
        else if(cooldown >= 7f && phase == 1){
            StartCoroutine(jump());
            cooldown = 0.0f;
        }
    }
    private IEnumerator jump()
    {
        List<float> distances = new List<float>();
        foreach (var DP in dropPoints)
        {
            distances.Add(Vector2.Distance(DP.position, player.transform.position));
        }
        anim.SetBool("isJumping", true);
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("isJumping", false);
        transform.position = dropPoints[distances.IndexOf(distances.Min())].position;

        moveCounter++;
        if (moveCounter == 3)
        {
            if(phase == 1){cooldown = -3.0f;}
            moveCounter = 0;
        }
    }

    private IEnumerator Spit()
    {
        anim.SetBool("isSpitting", true);
        for(int x = 0; x<3; x++){spit = Instantiate(projectile, spitter.position, Quaternion.Euler(0, 0 ,angle));
        spit.GetComponent<Rigidbody2D>().AddForce(spit.transform.TransformDirection(Vector2.right*500f));
        yield return new WaitForSeconds(1f);}
        StartCoroutine(jump());
        anim.SetBool("isSpitting", false);
    }
}
