using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealthValue;
    [SerializeField] private float defenseValue;
    private float healthValue;
    
    public void TakeDamage(float opposingDamageAmount)
    {
        if (opposingDamageAmount > defenseValue)
        {
            healthValue -= (opposingDamageAmount - defenseValue);
            Debug.Log("Player Took Damage");
        }
    }

    void Start()
    {
        healthValue = maxHealthValue;
    }

    private void Update()
    {
        if (healthValue <= 0)
        {
            Destroy(this.gameObject);
            Debug.Log("This character has been slain");
        }
    }
}
