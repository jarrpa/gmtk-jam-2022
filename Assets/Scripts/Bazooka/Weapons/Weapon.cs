using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum WeaponKind {
    Unknown,
    Single,
    Shotgun,
    Grenade,
    Explosion,
}

public class Weapon : MonoBehaviour
{
    #region Weapon Attributes

    public WeaponKind kind = WeaponKind.Unknown;
    public float fireRate = 1f;
    public float spread;
    public int numberOfBullets = 4;
    public bool isMultiShot;
    public ParticleSystem muzzleFlash;
    
    private bool _canShoot;
    private Vector2 _shootingDir;
    private int _damage;
    
    
    #endregion
    
    #region Bullet Attributes

    public Transform firePosition;
    public GameObject bulletPrefab;
    public GameObject player;

    #endregion


    public AttackEvent weaponFireEvent;
    private AttackPayload weaponAttack = new AttackPayload();

    public void Start()
    {
        _canShoot = true;
        player = GameObject.FindWithTag("Player");
        weaponAttack.attacker = player.GetComponent<Entity>();
        weaponAttack.kind = this.kind;
        weaponFireEvent ??= GameEventLoader.Load<AttackEvent>("WeaponFireEvent");
    }

    public void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && _canShoot)
        {
            var main = muzzleFlash.main;
            main.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            muzzleFlash.Play();
            weaponFireEvent?.Invoke(weaponAttack);
            StartCoroutine(FireWeapon());
        }
    }

    private void Shoot()
    {
        if (isMultiShot)
        {
            var right = firePosition.right;
            var facingAngle = Mathf.Atan2(right.y, right.x) * Mathf.Rad2Deg;
            var startRotation = facingAngle + spread / 2f;
            var angleIncrease = spread / ((float)numberOfBullets - 1);

            for (int i = 0; i < numberOfBullets; i++)
            {
                var rot = startRotation - angleIncrease * i;
                var bullet = Instantiate(bulletPrefab, firePosition.position, Quaternion.Euler(0f, 0f, rot));
                var startPosition = new Vector2(Mathf.Cos(rot * Mathf.Deg2Rad), Mathf.Sin(rot * Mathf.Deg2Rad));
                bullet.GetComponent<Bullet>().Setup(startPosition, weaponAttack);
            }
        }
        else
        {
            InstantiateBullet();
        }
    }
    
    private IEnumerator FireWeapon()
    {
        _canShoot = false;
        Shoot();
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }
    
    private void InstantiateBullet()
    {
        var bullet = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Setup(firePosition.right, weaponAttack);
    }
}
