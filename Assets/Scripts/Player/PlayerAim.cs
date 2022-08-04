using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Transform weaponSlotTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponSlotTransform = transform.Find("Weapon Slot");

    }

    private void HandleAiming()
    {
        Vector3 mousePosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        weaponSlotTransform.eulerAngles = new Vector3(0, 0, angle);
        
        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }

        weaponSlotTransform.localScale = localScale;
    }

    private void Update()
    {
        HandleAiming();
    }
}
