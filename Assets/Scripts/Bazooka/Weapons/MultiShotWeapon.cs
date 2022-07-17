using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotWeapon : Weapon
{
    public float shootAngle;

    public int bulletSpread = 45;
    public int numberOfBullets = 10;
    

    // angle in degrees
    public Vector2 DirectionFromAngle(float angle)
    {
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad),Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    
    protected override void Shoot()
    {
        float facingAngle = Mathf.Atan2(firePosition.up.y, firePosition.up.x) * Mathf.Rad2Deg;
        float startAngle = facingAngle + bulletSpread / 2f - 90f;
        float angleIncrease = bulletSpread / (numberOfBullets - 1f);
        
        for (int i = 0; i < numberOfBullets; ++i)
        {
            float angle = startAngle - angleIncrease * i;
            Vector2 dir = DirectionFromAngle(angle);
            InstantiateBullet(dir, firePosition.rotation);
        }

    }
}
