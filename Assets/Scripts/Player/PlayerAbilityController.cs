using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    // create movement abilities aswell
    public Transform[] weapons;
    private int selectedWeapon = 0;

    // Start is called before the first frame update

    private void Awake()
    {
        ActivateWeapon(0);
    }

    public void ChoseCombatAbility()
    {
        int[] choice = Die.RollDice(weapons.Length, 1);
        ActivateWeapon(choice[0]);
    }

    private void ActivateWeapon(int index)
    {
        weapons[selectedWeapon].gameObject.SetActive(false);
        selectedWeapon = index;
        weapons[selectedWeapon].gameObject.SetActive(true);
        
    }
    
}
