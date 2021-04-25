using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonHealth : MonoBehaviour
{
    private float myHealth = 50;
    public bool isDeaad = false;

    public ParticleSystem DeathFx;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myHealth < 0 && !isDeaad)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        myHealth -= amount;
        Debug.Log(name + " health down to " + myHealth);
        // TODO: Play damage anim (flash mesh) 
        Instantiate(DeathFx, transform.position, Quaternion.identity);
    }

    void Die()
    {
        Debug.Log(name + " has died");
        isDeaad = true;
        // TODO: death anim
        FindObjectOfType<GameController>().BroadcastMessage("PlayerDied");
        Destroy(gameObject);
//        transform.root.tag = "Untagged";
        
    }

    public void ResetHealth()
    {
        myHealth = 50;
    }
}
