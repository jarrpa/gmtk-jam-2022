using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityController : MonoBehaviour
{
    // create movement abilities aswell
    public Transform[] weapons;
    public Ability[] abilities;

    public Image abilityImage;
    public Image weaponImage;
    
    private AbilityHolder abilityHolder;

    private int selectedWeapon;
    private int selectedAbility;

    // Start is called before the first frame update

    private void Awake()
    {
        abilityHolder = GetComponent<AbilityHolder>();
        ChoseAbilities();
        // transform abilities[selectedAbility].name to lowe case
        switch (abilities[selectedAbility].name.ToLower())
        {
            case "dash":
                Debug.Log("abilities[selectedAbility].name.ToLower():" + abilities[selectedAbility].name.ToLower());
                abilityImage.sprite = Resources.Load<Sprite>("Sprites/Ability=Dash Icon");
                break;
            case "smash":
                abilityImage.sprite = Resources.Load<Sprite>("Sprites/Ability=Slam Icon");
                break;
            case "teleport":
                abilityImage.sprite = Resources.Load<Sprite>("Sprites/Ability=Teleport Icon");
                break;
            default:
                break;
        }
        switch (selectedWeapon)
        {
            case 0:
                weaponImage.sprite = Resources.Load<Sprite>("Sprites/fireball");
                break;
            case 1:
                weaponImage.sprite = Resources.Load<Sprite>("Sprites/iceball");
                break;
            case 2:
                weaponImage.sprite = Resources.Load<Sprite>("Sprites/lightningball");
                break;
            default:
                break;
        }
        
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