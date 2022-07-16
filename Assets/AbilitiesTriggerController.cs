using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesTriggerController : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
         col.GetComponent<PlayerAbilityController>().ChoseCombatAbility();
   }
}
