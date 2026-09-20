using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEngine.GraphicsBuffer;

public class shieldEnemy : MonoBehaviour
{
    //States
    private bool mustFlip = false;
    [SerializeField] private bool isFlipped;

    //Declare variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float playerAngle;
    [SerializeField] private float fixedAngle;

    //Components and other piss
    private fireball fireballScript;
    private icicle icicleScript;
    private Tiny_icicle tinyIcicleScript;

    [SerializeField] private GameObject player;

    private Rigidbody2D rb;
    public LayerMask groundLayer;
    private RaycastHit2D groundHit;
    private RaycastHit2D shieldHit;
    private RaycastHit2D backupShieldHit;

    private Transform shieldStart;
    private Transform shieldEnd;
    private Transform backupShieldStart;
    private Transform backupShieldEnd;
    private Transform groundCheckStart;
    private Transform groundCheckEnd;

    [SerializeField] private bool isDefending = false;
    private bool isFlipping;

    private void Start()
    {
        //getting the child's transform, this one is fucky cuz the parent is part of the array, and therefore you start at 1. (the number is based on the order in the hirearcy)
        shieldStart = gameObject.GetComponentsInChildren<Transform>()[1];
        shieldEnd = gameObject.GetComponentsInChildren<Transform>()[2];
        backupShieldStart = gameObject.GetComponentsInChildren<Transform>()[3];
        backupShieldEnd = gameObject.GetComponentsInChildren<Transform>()[4];
        groundCheckStart = gameObject.GetComponentsInChildren<Transform>()[5];
        groundCheckEnd = gameObject.GetComponentsInChildren<Transform>()[6];

        //Getting components
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        //Debug lines for linecasts
        Debug.DrawLine(groundCheckStart.position, groundCheckEnd.position);

        //Linecast
        groundHit = Physics2D.Linecast(groundCheckStart.position, groundCheckEnd.position, 1 << 3);


        if(distanceFromPlayer < 7f)
        {
            isDefending = true;
        }
        else
        {
            isDefending = false;
        }

        if (isDefending)
        {
            Defend();
        }
        else
        {
            Patrol();
        }

        DetectHits();
    }

    private void Defend()
    {
        if (isFlipped && !isFlipping)
        {
            if(player.transform.position.x > transform.position.x)
            {
                isFlipping = true;
                StartCoroutine(SlowFlip(2));
            }
        }
        else if (!isFlipped && !isFlipping)
        {
            if(player.transform.position.x < transform.position.x)
            {
                isFlipping = true;
                StartCoroutine(SlowFlip(2));
            }
        }
        rb.velocity = new Vector2(moveSpeed*0.7f, rb.velocity.y);
    }

    IEnumerator SlowFlip(float seconds)
    {
        isFlipped = !isFlipped;
        yield return new WaitForSeconds(seconds);
        transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        moveSpeed *= -1;
        isFlipping = false;
    }

    private void Patrol()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        if (!groundHit)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        moveSpeed *= -1;
        isFlipped = !isFlipped;
    }

    private void DetectHits()
    {
        shieldHit = Physics2D.Linecast(shieldStart.position, shieldEnd.position, 1 << 7);
        backupShieldHit = Physics2D.Linecast(backupShieldStart.position, backupShieldEnd.position, 1 << 7);
        Debug.DrawLine(shieldStart.position, shieldEnd.position);
        Debug.DrawLine(backupShieldStart.position, backupShieldEnd.position);

        if (shieldHit)
        {
            shieldHit.rigidbody.velocity = new Vector2(0, 0);

            switch (shieldHit.collider.name)
            {
                case "Circle(Clone)":
                    fireballScript = shieldHit.collider.gameObject.GetComponent<fireball>();
                    StartCoroutine(fireballScript.explode());
                    Debug.Log("blocked fire");
                    break;
                case "Triangle(Clone)":
                    icicleScript = shieldHit.collider.gameObject.GetComponent<icicle>();
                    StartCoroutine(icicleScript.explode());
                    Debug.Log("blocked ice");
                    break;
                case "Triangle 1(Clone)":
                    tinyIcicleScript = shieldHit.collider.gameObject.GetComponent<Tiny_icicle>();
                    StartCoroutine(tinyIcicleScript.explode());
                    Debug.Log("blocked tiny ice");
                    break;
                default:
                    Debug.LogWarning(shieldHit.collider.name + " is an undefined projectile");
                    break;
            }
        }
        else if (backupShieldHit)
        {
            switch (backupShieldHit.collider.name)
            {
                case "Circle(Clone)":
                    fireballScript = backupShieldHit.collider.gameObject.GetComponent<fireball>();
                    StartCoroutine(fireballScript.explode());
                    Debug.Log("backup blocked fireball");
                    break;
                case "Triangle(Clone)":
                    icicleScript= backupShieldHit.collider.gameObject.GetComponent<icicle>();
                    StartCoroutine(icicleScript.explode());
                    Debug.Log("backup blocked icicle");
                    break;
                case "Triangle 1(Clone)":
                    tinyIcicleScript = backupShieldHit.collider.gameObject.GetComponent<Tiny_icicle>();
                    StartCoroutine(tinyIcicleScript.explode());
                    Debug.Log("backup blocked tiny icicle");
                    break;
                default:
                    Debug.LogWarning(backupShieldHit.collider.name + " is an undefined projectile");
                    Destroy(backupShieldHit.collider.gameObject);
                    break;
            }
        }
    }
}
