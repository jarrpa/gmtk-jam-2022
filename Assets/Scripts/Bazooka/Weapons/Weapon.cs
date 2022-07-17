using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Weapon attribute
    private int damage;
    public float fireRate;
    private bool canShoot;
    private Vector2 shootingDir;

    // Bullet attributes
    public float bulletSpeed = 20f;
    public Transform firePosition;

    public GameObject bulletPrefab;

    private void Start()
    {
        canShoot = true;
    }

    public void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && canShoot)
            StartCoroutine(FireWeapon());
    }

    protected abstract void Shoot();
    
    protected IEnumerator FireWeapon()
    {
        canShoot = false;
        Shoot();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    
    protected void InstantiateBullet(Vector2 dir, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePosition.position, rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
    }
}
