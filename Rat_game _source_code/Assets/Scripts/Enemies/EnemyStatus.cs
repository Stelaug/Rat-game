using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;

    public float knockbackForce;
    public GameObject popupManager;
    private Coroutine ice_coroutine;
    private Coroutine fire_coroutine;
    private bool fireDelay = false;
    private float t = 0;
    private float offset;
    private int ice_q = 1;
    public int dmg;

    // Experimental---------------------------------------------------------------------------------------------------------------------------------------------------------
    private int coinAmount;
    private GameObject coinIns;
    [SerializeField] private GameObject coin;
    // Experimental---------------------------------------------------------------------------------------------------------------------------------------------------------
    
    public GameObject player;
    [SerializeField] private int coinMin;
    [SerializeField] private int coinMax;
    // Start is called before the first frame update
    void Start()
    {
        try{offset = GetComponent<Basic_patrol_enemy>().offset;}
        catch{}
        
        updateStatus("null");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (getIceQ() == 1)
        {
            updateStatus("null");
        }
        if(health <= 0){
            dropCoins(coinMin, coinMax);
            Destroy(gameObject);
        }
        else if(player.GetComponent<PlayerStatus>().health <= 0) {
            gameObject.GetComponent<Rigidbody2D>().constraints =  RigidbodyConstraints2D.FreezePosition;
        }
    }
    public void takeDamage(int dmg, string type){
        
        if(type == "fire"){
            updateStatus("fire");
            health -= dmg;
            popupManager.GetComponent<Damage_popup>().display_damage(dmg, new Vector3(transform.position.x, (transform.position.y-offset), transform.position.z), Color.red);

        }
        if(type == "ice"){
            updateStatus("ice");
            health -= dmg;
            popupManager.GetComponent<Damage_popup>().display_damage(dmg, new Vector3(transform.position.x, (transform.position.y-offset), transform.position.z), Color.cyan);
        }
        if(type == "fire_null"){
            health -= dmg;
            popupManager.GetComponent<Damage_popup>().display_damage(dmg, new Vector3(transform.position.x, (transform.position.y-offset), transform.position.z), Color.red);
        }
        if(type == "null"){
            health -= dmg;
            popupManager.GetComponent<Damage_popup>().display_damage(dmg, new Vector3(transform.position.x, (transform.position.y-offset), transform.position.z), Color.white);
        }
        
    }
    public void updateStatus(string type){
        switch(type){
            case "fire":
            fire_coroutine = StartCoroutine(fire());
            GetComponent<SpriteRenderer>().color = Color.red;
            try{StopCoroutine(ice_coroutine);}
            catch{}
            setIceQ(1);
            break;
            case "ice":
                GetComponent<SpriteRenderer>().color = Color.cyan;
                try {StopCoroutine(fire_coroutine);}
            catch{}
            ice_coroutine = StartCoroutine(ice());
            break;
            case "null":
                GetComponent<SpriteRenderer>().color = Color.white;
            break;
        }
    }
    private void setIceQ(int x){
        try{gameObject.GetComponent<Basic_patrol_enemy>().ice_q = x;}
        catch{}
        try{gameObject.GetComponent<smart_enemy>().ice_q = x;}
        catch{}
        try{gameObject.GetComponent<Tounge_enemy>().ice_q = x;}
        catch{}
        try{gameObject.GetComponent<Cockroach_enemy>().ice_q = x;}
        catch{}
        try{gameObject.GetComponent<Temp_frog_movement>().ice_q = x;}
        catch{}
    }
    private int getIceQ(){
        try{return gameObject.GetComponent<Basic_patrol_enemy>().ice_q;}
        catch{}
        try{return gameObject.GetComponent<smart_enemy>().ice_q;}
        catch{}
        try{return gameObject.GetComponent<Tounge_enemy>().ice_q;}
        catch{}
        try{return gameObject.GetComponent<Cockroach_enemy>().ice_q;}
        catch{}
        try{return gameObject.GetComponent<Temp_frog_movement>().ice_q;}
        catch{return 1;}
        
    }
    private void setKnockbackDelay(bool x){
        try{gameObject.GetComponent<Basic_patrol_enemy>().knockbackDelay = x;}
        catch{}
        try{gameObject.GetComponent<smart_enemy>().knockbackDelay = x;}
        catch{}
        try{gameObject.GetComponent<Cockroach_enemy>().knockbackDelay = x;}
        catch{}
    }
    private void addIceQ(int x){
        try{gameObject.GetComponent<Basic_patrol_enemy>().ice_q += x;
        print(gameObject.GetComponent<Basic_patrol_enemy>().ice_q);
        }
        catch{}
        try{gameObject.GetComponent<smart_enemy>().ice_q += x;
        print(gameObject.GetComponent<smart_enemy>().ice_q);
        }
        catch{}
        try{gameObject.GetComponent<Tounge_enemy>().ice_q += x;
        print(gameObject.GetComponent<Tounge_enemy>().ice_q);
        }
        catch{}
        try{gameObject.GetComponent<Cockroach_enemy>().ice_q += x;
        print(gameObject.GetComponent<Cockroach_enemy>().ice_q);
        }
        catch{}
        try{gameObject.GetComponent<Temp_frog_movement>().ice_q += x;
        }
        catch{}
    }
    void OnTriggerExit2D(Collider2D col){
        // Very shitty damage system
        if(col.gameObject.layer == 7){
            knockback(knockbackForce);
            try{takeDamage(col.gameObject.GetComponent<fireball>().damage, "fire");}
            catch{}
            try{takeDamage(col.gameObject.GetComponent<icicle>().damage, "ice");}
            catch{}
            try{takeDamage(col.gameObject.GetComponent<Tiny_icicle>().damage, "ice");}
            catch{}

        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == 15){
            knockback(knockbackForce);
            try{takeDamage(col.gameObject.GetComponent<Melee>().damage, "null");}
            catch{}
    
    }
        
        
    }
    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.layer == 12){
            t += Time.deltaTime;
            if(t > 0.1f){knockback(knockbackForce/1.9f);
            try{takeDamage(col.gameObject.GetComponent<Flamethrower>().damage, "fire");}
            catch{}
            t = 0;
            }
        }

    }
    private IEnumerator Delay(){
        // Stops the enemy from moving while taking knockback
        setKnockbackDelay(true);
        yield return new WaitForSeconds(0.1f);
        setKnockbackDelay(false);

    }
    private IEnumerator ice(){
        addIceQ(1);
        yield return new WaitForSeconds(5);
        if(getIceQ()> 1){ addIceQ(-1);}
    }
    private IEnumerator fire(){
        for(int x = 0; x < 5; x+=1){
            takeDamage(5, "fire_null");
            yield return new WaitForSeconds(1);
            }
    }
    void knockback(float force){
        // Knockback
        StartCoroutine(Delay());
        Vector2 dir = (transform.position - new Vector3(player.transform.position.x, transform.position.y, transform.position.z)).normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir*force, ForceMode2D.Impulse);
    }
    //Experimental------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void dropCoins(int max, int min){
        coinAmount = Random.Range(max, min);
        for(int x = 0; x < coinAmount; x+=1){
            coinIns = Instantiate(coin, transform.position, Quaternion.identity);
            coinIns.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200f, 200f), Random.Range(0f, 200f)));
        }

    }
    //Experimental------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
