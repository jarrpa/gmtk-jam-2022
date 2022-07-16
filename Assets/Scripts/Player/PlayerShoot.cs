using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float secBtwnShot;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    public Transform fireLocation;
    private float nextShotTime = 0;

    
    private void Update()
    {
        nextShotTime += Time.fixedDeltaTime;
        if (nextShotTime >= secBtwnShot)
        {
            Shoot();
            nextShotTime = 0;
        }
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            GameObject projectile = Instantiate(projectilePrefab,fireLocation.position, fireLocation.rotation);
        }
    }
}
