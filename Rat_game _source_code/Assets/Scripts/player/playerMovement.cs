using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Variable Declarations
    private float speed = 5f;
    private float currentSpeed;
    private float horizontalInput;
    private float vericalInput;
    private float jumpingPower = 10f;
    private float currentJumpingPower;
    public bool isFacingRight = true;
    private float cyoteTime = 0.2f;
    private float cyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float meleeCooldown = 0.0f;
    private bool flamethrowerActive = false;
    private bool canDash = true;
    private bool charging = false;
    private bool isDashing;
    private bool fireMode = true;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool casting = false;
    private Animator anim;
    public bool isTakingKnockback = false;
    private int fireballCost = 20;
    private int icicleCost = 20;
    private int tinyIcicleCost = 6;
    private int flamethrowerCost = 40;
    private float regenTimer = 0f;
    private float regen = 0;
    private bool isLookingUp = false;
    private bool isLookingDown = false;
    [SerializeField] private GameObject firebar;
    [SerializeField] private GameObject icebar;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject icicle;
    [SerializeField] private GameObject tinyIcicle;
    void Start(){
        // I don't even know what this is honestly
        anim = gameObject.GetComponent<Animator>();
        Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
    }
    void Update()
    {
        if(Time.timeScale == 1){
            vericalInput = Input.GetAxisRaw("Vertical");
        meleeCooldown += Time.deltaTime;
        regenTimer += Time.deltaTime;
        if(regenTimer >= 3){
            regen+=Time.deltaTime;
            if(regen >= 0.05f){GetComponent<PlayerStatus>().mana += 1;regen = 0f;}
        }
        
        if(isDashing){
            // Skips the Update function when you are dashing
            return;
        }
        // Animations
        if(horizontalInput != 0){
            anim.SetBool("run", true);
        }
        if(horizontalInput == 0){
            anim.SetBool("run", false);
        }
        if(!isGrounded()){
            anim.SetBool("jump", true);
        }
        else{
            anim.SetBool("jump", false);
        }
        // Cyote Time
        if (isGrounded()){
            cyoteTimeCounter = cyoteTime;
        }
        else{
            cyoteTimeCounter -= Time.deltaTime;
        }
        // Jumping Cyote Time
        if (Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBufferTime;
        }
        else{
            jumpBufferCounter -= Time.deltaTime;
        }
        // Stops Horizontal input when you are using the flamethrower or the ice scattershot
        if(!charging && !flamethrowerActive){horizontalInput = Input.GetAxisRaw("Horizontal");}
        flip();
        if (jumpBufferCounter > 0f && cyoteTimeCounter > 0f){
            jumpBufferCounter = 0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        // Jumping
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
            cyoteTimeCounter = 0f;
        }
        // Dash input
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash || Input.GetKeyDown(KeyCode.Joystick1Button1) && canDash){
            StartCoroutine(Dash());
        }
        // Fireball
        if(Input.GetKeyDown(KeyCode.Joystick1Button5)&& fireMode && !casting || Input.GetKeyDown(KeyCode.Mouse0)&& fireMode && !casting){
            regenTimer = 0f;
            if(GetComponent<PlayerStatus>().mana > fireballCost){StartCoroutine(CastFireball());}
            
        }
        // Flamethrower activation
        if(Input.GetKeyDown(KeyCode.Joystick1Button4) && fireMode && !casting || Input.GetKeyDown(KeyCode.Mouse1) && fireMode && !casting){
            if(GetComponent<PlayerStatus>().mana > flamethrowerCost){
            regenTimer = 0f;
            anim.SetBool("casting", true);
            GetComponent<PlayerStatus>().mana -= flamethrowerCost;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            fire.GetComponent<Animator>().SetBool("active", true);
            flamethrowerActive = true;}
        }
        // Flamethrower deactivation
        if(Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Mouse1)){
            anim.SetBool("casting", false);
            fire.GetComponent<Animator>().SetBool("active", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            flamethrowerActive = false;
            
        }
        // Ice scattershot activation
        if(Input.GetKeyDown(KeyCode.Joystick1Button4) && !fireMode && !casting || Input.GetKeyDown(KeyCode.Mouse1) && !fireMode && !casting){
            
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            charging = true;
            StartCoroutine(Charge());
        }
        // Ice scatterchot deactivation
        if(Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Mouse1)){
            regenTimer = 0f;
            charging = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StopCoroutine(Charge());
        }
        // Icicle
        if(Input.GetKeyDown(KeyCode.Joystick1Button5)&& !fireMode && !casting || Input.GetKeyDown(KeyCode.Mouse0)&& !fireMode && !casting){
            
            if(GetComponent<PlayerStatus>().mana > icicleCost){StartCoroutine(CastIcicle());regenTimer = 0f;}
            
        }
        // Activate ice mode
        if(Input.GetKeyDown(KeyCode.Joystick1Button3) && fireMode || Input.GetKeyDown(KeyCode.E) && fireMode){
            fireMode = false;
            icebar.SetActive(true);
            firebar.SetActive(false);
            GetComponent<PlayerStatus>().mpBar.fillRect = icebar.GetComponent<RectTransform>();
        }
        // Activate fire mode
        else if(Input.GetKeyDown(KeyCode.Joystick1Button3) && !fireMode || Input.GetKeyDown(KeyCode.E) && !fireMode){
            fireMode = true;
            firebar.SetActive(true);
            icebar.SetActive(false);
            GetComponent<PlayerStatus>().mpBar.fillRect = firebar.GetComponent<RectTransform>();
        }
        if(Input.GetKeyDown(KeyCode.W) || vericalInput > 0.5f){
            isLookingUp = true;
        }
        if(Input.GetKeyUp(KeyCode.W) || vericalInput < 0.5f){
            isLookingUp = false;
        }
        if(Input.GetKeyDown(KeyCode.S) || vericalInput < -0.5f){
            isLookingDown = true;
        }
        if(Input.GetKeyUp(KeyCode.S) || vericalInput > -0.5f){
            isLookingDown = false;
        }
        // TEMPORARY-----------------------------------------------------------------------------------------------------------------------------------------
        /*
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            PlayerPrefs.SetInt("maxHealth", PlayerPrefs.GetInt("maxHealth")+ 10);
            print(PlayerPrefs.GetInt("maxHealth"));
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)){
            PlayerPrefs.SetInt("maxHealth", PlayerPrefs.GetInt("maxHealth")- 10);
            print(PlayerPrefs.GetInt("maxHealth"));
        }
        */
        
        
        // TEMPORARY-----------------------------------------------------------------------------------------------------------------------------------------
        if(Input.GetKeyDown(KeyCode.Q) && meleeCooldown > 0.5f || Input.GetKeyDown(KeyCode.JoystickButton2) && meleeCooldown > 0.5f){
            meleeCooldown = 0.0f;
            StartCoroutine(Meleee());
        }
        }
    }
    void FixedUpdate(){
        if(isDashing){
            // Skips the FixedUpdate function when dashing 
            return;
        }
        // Moves the player horizontaly according to input
        if(!isTakingKnockback){rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);}
    }
    private bool isGrounded(){
        // Checks if the player is on the ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void flip(){
        // Flips the player when it is turning
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f){
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private IEnumerator Dash(){
        // Dash
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(-transform.localScale.x*dashingPower, 0f);
        tr.emitting = true;
        anim.SetBool("dash", true);
        yield return new WaitForSeconds(dashingTime);
        anim.SetBool("dash", false);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator Charge(){
        // Instantiates the tiny icicles in scatterhot
        casting = true;
        anim.SetBool("casting", true);
        yield return new WaitForSeconds(0.45f);
        while (charging && GetComponent<PlayerStatus>().mana > tinyIcicleCost){
            GetComponent<PlayerStatus>().mana -= tinyIcicleCost;
            regenTimer = 0f;
            GameObject projectile = Instantiate(tinyIcicle, firePoint.transform.position, transform.rotation) as GameObject;
        projectile.GetComponent<Tiny_icicle>().direction = new Vector2(-transform.localScale.x, 0);yield return new WaitForSeconds(0.2f);}
        casting = false;
        anim.SetBool("casting", false);
    }
    private IEnumerator CastFireball(){
        casting = true;
        anim.SetBool("casting", true);
        GetComponent<PlayerStatus>().mana -= fireballCost;
        yield return new WaitForSeconds(0.45f);
        GameObject projectile = Instantiate(fireball, new Vector3(firePoint.transform.position.x, transform.position.y+0.1f, transform.position.z), transform.rotation) as GameObject;
        projectile.GetComponent<fireball>().direction = new Vector2(-transform.localScale.x, 0);
        casting = false;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("casting", false);
    }
    private IEnumerator CastIcicle(){
        casting = true;
        anim.SetBool("casting", true);
        GetComponent<PlayerStatus>().mana -= icicleCost;
        yield return new WaitForSeconds(0.45f);
        GameObject projectile = Instantiate(icicle, new Vector3(firePoint.transform.position.x+(0.15f)*-transform.localScale.x, firePoint.transform.position.y, firePoint.transform.position.z), transform.rotation) as GameObject;
        projectile.GetComponent<icicle>().direction = new Vector2(-transform.localScale.x, 0);
        casting = false;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("casting", false);
    }
    private IEnumerator CastFlamethrower(){

        yield return new WaitForSeconds(0.1f);
    }  
    private IEnumerator Meleee(){
        anim.SetBool("attacking", true);
        gameObject.GetComponent<PlayerStatus>().invincible = true;
        isTakingKnockback = true;
        StartCoroutine(MeleeMove(new Vector2((3*-transform.localScale.x), rb.velocity.y)));
        yield return new WaitForSeconds(0.5f);
        isTakingKnockback = false;
        gameObject.GetComponent<PlayerStatus>().invincible = false;
        anim.SetBool("attacking", false);
    }
    private IEnumerator MeleeMove(Vector2 force){
        rb.velocity = force;
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        
    }
}
