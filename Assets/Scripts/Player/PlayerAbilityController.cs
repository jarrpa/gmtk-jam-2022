using System;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    // create movement abilities aswell

    public GameObject[] weapons = new GameObject[3];
    public Ability[] abilities = new Ability[3];
    public Transform weaponHolder;
    
    private AbilityHolder abilityHolder;

    private int selectedWeapon;
    private int selectedAbility;
    public GameEvent bazookaPickupEvent;

    public event Action<int, int> OnAbilityChange;
    private void Awake()
    {
        abilityHolder = GetComponent<AbilityHolder>();

        bazookaPickupEvent ??= GameEventLoader.Load<GameEvent>("BazookaPickupEvent");
        bazookaPickupEvent?.AddListener(ChoseAbilities);
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
        weaponHolder.gameObject.SetActive(true);
        Destroy(weaponHolder.transform.GetChild(0).gameObject);
        Instantiate(weapons[selectedWeapon], weaponHolder.transform);
    }
    
    private void ActiveAbility(int index)
    {
        selectedAbility = index;
        abilityHolder.ability = abilities[selectedAbility];
    }

    private void OnDestroy() {
        bazookaPickupEvent?.RemoveListener(ChoseAbilities);
    }
}