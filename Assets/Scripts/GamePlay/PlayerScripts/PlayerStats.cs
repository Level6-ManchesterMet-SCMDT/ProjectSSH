using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    
    [SerializeField] GameObject healthBarRef;

    public int maxHealth = 100;

    private int currentHealth;
    private HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar = healthBarRef.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
    void HealDamage(int damage)
    {
        currentHealth += damage;

        healthBar.SetHealth(currentHealth);
    }
}
