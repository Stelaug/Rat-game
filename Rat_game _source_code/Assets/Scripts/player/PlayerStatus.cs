using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerStatus : MonoBehaviour
{
    //public Coin coin;
    public int health;
    public int mana = 100;
    public int coins = 0;
    public bool invincible = false;

    public int currCoins = 0;

    //Items
    [SerializeField] private int healthPotions = 0;
    [SerializeField] private int manaPotions = 0;
    

    private Rigidbody2D rb;
    [SerializeField] private GameObject coins_display;
    [SerializeField] public Slider hpBar;
    [SerializeField] public Slider mpBar;
    private AudioSource audioSource;

    private Animator anim;
    private bool dPadUp = true;
    private bool dPad2Up = true;

    private void Start(){
        
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        
        if(PlayerPrefs.GetInt("maxHealth") == 0){
            PlayerPrefs.SetInt("maxHealth", 100);
            PlayerPrefs.Save();
        }
        if(PlayerPrefs.GetInt("currHealth") == 0){
            PlayerPrefs.SetInt("currHealth", 100);
            PlayerPrefs.Save();
        }
        health = PlayerPrefs.GetInt("currHealth");

        try{if(PlayerPrefs.GetInt("checkpoint") == 1){
            transform.position = GameObject.Find("Checkpoint").transform.position;
        }
        else if(PlayerPrefs.GetInt("checkpoint") == 2){
            transform.position = GameObject.Find("Checkpoint (1)").transform.position;
        }
        else if(PlayerPrefs.GetInt("checkpoint") == 3){
            transform.position = GameObject.Find("Checkpoint (2)").transform.position;
        }}
        catch{}
        
    }

    private void Update() {
        

        if(Input.GetAxis("Horizontal_d") == 0){
            dPadUp = true;
        }
        if(Input.GetAxis("Vertical_d") == 0){
            dPad2Up = true;
        }
        coins_display.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("bal").ToString();
        hpBar.maxValue = PlayerPrefs.GetInt("maxHealth");
        mana = Mathf.Clamp(mana, 0, 100);
        health = Mathf.Clamp(health, 0, PlayerPrefs.GetInt("maxHealth"));
        if(health <= 0){
            anim.SetBool("isDead", true);
            rb = GetComponent<Rigidbody2D>();
            rb.constraints =  RigidbodyConstraints2D.FreezePosition;
            rb.constraints =  RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<playerMovement>().enabled = false;
            StartCoroutine(isDying());
        }
        hpBar.GetComponent<Slider>().value = health;
        mpBar.GetComponent<Slider>().value = mana;

        //detect if the player uses items
        if(Input.GetKeyDown(KeyCode.Alpha1) && PlayerPrefs.GetInt("HP_P") > 0 || Input.GetAxis("Horizontal_d") == 1 && PlayerPrefs.GetInt("HP_P") > 0 && dPadUp == true)
        {
            dPadUp = false;
            print("yeet");
            UseItem("Health");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("MP_P") > 0 || Input.GetAxis("Horizontal_d") == -1 && PlayerPrefs.GetInt("MP_P") > 0 && dPadUp == true)

        {
            dPadUp = false;
            UseItem("Mana");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 8 && !invincible){
            knockback(5f, other.gameObject);
            if (anim.GetBool("hurting") == false)
            {
                health -= other.gameObject.GetComponent<EnemyStatus>().dmg;
                StartCoroutine(isHurting());
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 11 && !invincible)
        {
            knockback(5f, other.gameObject);
            if (anim.GetBool("hurting") == false)
            {
                health -= other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatus>().dmg;
                StartCoroutine(isHurting());
            }
        }
        if(other.gameObject.layer == 17 && !invincible){
            knockback(5f, other.gameObject);
            if (anim.GetBool("hurting") == false)
            {
                health -= other.gameObject.GetComponent<Spit_script>().dmg;
                StartCoroutine(isHurting());
            }
        }
    }
    void knockback(float force, GameObject enemy){
        // Knockback
        StartCoroutine(Delay());
        Vector2 dir = (transform.position - enemy.transform.position).normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir*force, ForceMode2D.Impulse);
    }
    private IEnumerator Delay(){
        // Stops the enemy from moving while taking knockback
        gameObject.GetComponent<playerMovement>().isTakingKnockback = true;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<playerMovement>().isTakingKnockback = false;

    }
    public void heal(int num){
        health += num;
        GameObject.Find("Damage_popup_manager").GetComponent<Damage_popup>().display_damage(num, transform.position, Color.green);
    }
    public IEnumerator isHurting()
    {
        anim.SetBool("hurting", true);
        yield return new WaitForSeconds(0.20f);
        anim.SetBool("hurting", false);

    }
    public IEnumerator isDying()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("currHealth", 100);
        SceneManager.LoadScene(PlayerPrefs.GetInt("scene"));
    }
    private void UseItem(string itemType) //Uses the specified item
    {
        switch (itemType)
        {
            case "Health":
                PlayerPrefs.SetInt("HP_P", PlayerPrefs.GetInt("HP_P")-1);
                heal(50);
                break;
            case "Mana":
                PlayerPrefs.SetInt("MP_P", PlayerPrefs.GetInt("MP_P")-1);
                GameObject.Find("Damage_popup_manager").GetComponent<Damage_popup>().display_damage(50, transform.position, Color.cyan);
                mana += 50;
                break;
            default:
                break;
        }
    }
    public void PlayCoinPickup(){
        audioSource.Play();
    }
        public void BuyItem(string itemType, int itemCost) //detects if the player buys an item and subtracts the appropriate ammount of coins
    {
        if(Input.GetKeyDown(KeyCode.Return) && itemCost <= PlayerPrefs.GetInt("bal") || Input.GetAxis("Vertical_d") == -1 && itemCost <= PlayerPrefs.GetInt("bal") && dPad2Up ) 
        {
            dPad2Up = false;
            Debug.Log("ItemPurchased");
            switch (itemType)
            {
                case "Health":
                    PlayerPrefs.SetInt("HP_P", PlayerPrefs.GetInt("HP_P")+1);
                    break;
                case "Mana":
                    PlayerPrefs.SetInt("MP_P", PlayerPrefs.GetInt("MP_P")+1);
                    break;
                default:
                    Debug.LogWarning("item name not recognised");
                    break;
            }
            PlayerPrefs.SetInt("bal", (PlayerPrefs.GetInt("bal")- itemCost));
            PlayerPrefs.Save();
        }
    }
}
