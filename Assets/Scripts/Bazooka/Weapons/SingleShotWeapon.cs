using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    protected override void Shoot()
    {
        InstantiateBullet(firePosition.up, firePosition.rotation);
    }
}
