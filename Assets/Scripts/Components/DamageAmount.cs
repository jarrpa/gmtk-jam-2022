using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmount : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    public float GetDamageAmount()
    {
        return damageAmount;
    }
}
