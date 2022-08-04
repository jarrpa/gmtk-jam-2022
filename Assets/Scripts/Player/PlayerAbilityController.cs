using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityController : MonoBehaviour
{
    // create movement abilities aswell

    public GameObject[] weapons = new GameObject[3];
    public Ability[] abilities = new Ability[3];
    public Transform weaponHolder;
    
    private AbilityHolder abilityHolder;

    private int selectedWeapon;
    private int selectedAbility;

    public event Action<int, int> OnAbilityChange;
    private void Awake()
    {
        abilityHolder = GetComponent<AbilityHolder>();
    }

    public void ChoseAbilities()
    {
        int[] combatChoice = Die.RollDice(weapons.Length, 1);
        int[] movementChoice = Die.RollDice(abilities.Length, 1);
        
        ActivateWeapon(combatChoice[0]);
        ActiveAbility(movementChoice[0]);
        
        OnAbilityChange?.Invoke(selectedAbility, selectedWeapon);
        
    }

    private void ActivateWeapon(int index)
    {
        selectedWeapon = index;
        Destroy(weaponHolder.transform.GetChild(0).gameObject);
        Instantiate(weapons[selectedWeapon], weaponHolder.transform);
    }
    
    private void ActiveAbility(int index)
    {
        selectedAbility = index;
        abilityHolder.ability = abilities[selectedAbility];
    }

}