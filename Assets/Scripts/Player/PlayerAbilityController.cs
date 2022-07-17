using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    // create movement abilities aswell
    public Transform[] weapons;
    public Ability[] abilities;

    public TMP_Text abilityText;
    public TMP_Text weaponText;
    
    private AbilityHolder abilityHolder;

    private int selectedWeapon;
    private int selectedAbility;

    // Start is called before the first frame update

    private void Awake()
    {
        abilityHolder = GetComponent<AbilityHolder>();
        ChoseAbilities();

        abilityText.text = "Ability: " + abilities[selectedAbility].name;
        weaponText.text = "Weapon: " + selectedWeapon;
    }

    public void ChoseAbilities()
    {
        int[] combatChoice = Die.RollDice(weapons.Length, 1);
        int[] movementChoice = Die.RollDice(abilities.Length, 1);
        
        ActivateWeapon(combatChoice[0]);
        ActiveAbility(movementChoice[0]);
        
    }

    private void ActivateWeapon(int index)
    {
        weapons[selectedWeapon].gameObject.SetActive(false);
        selectedWeapon = index;
        weapons[selectedWeapon].gameObject.SetActive(true);
    }
    
    private void ActiveAbility(int index)
    {
        selectedAbility = index;
        abilityHolder.ability = abilities[selectedAbility];
    }

}
