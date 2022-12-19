using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public FMODUnity.EventReference entityAttackSound;
    public FMODUnity.EventReference entityHitSound;
    public FMODUnity.EventReference entityDeathSound;
    public FMODUnity.EventReference weaponFireSound;

    public EntityEvent entityHitEvent;
    public EntityEvent entityDeathEvent;
    public AttackEvent enemyAttackEvent;
    public AttackEvent weaponFireEvent;
    public GameEvent explosionEvent;

    private static SoundManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        // Events we listen
        entityHitEvent ??= GameEventLoader.Load<EntityEvent>("EntityHitEvent");
        entityDeathEvent ??= GameEventLoader.Load<EntityEvent>("EntityDeathEvent");
        enemyAttackEvent ??= GameEventLoader.Load<AttackEvent>("EnemyAttackEvent");
        weaponFireEvent ??= GameEventLoader.Load<AttackEvent>("WeaponFireEvent");
        explosionEvent ??= GameEventLoader.Load<GameEvent>("GrenadeExplosionEvent");
    }

    private void OnEnable()
    {
        entityHitEvent.AddListener(OnDamageTaken);
        entityDeathEvent.AddListener(OnDeath);
        enemyAttackEvent.AddListener(OnEnemyAttack);
        weaponFireEvent.AddListener(OnWeaponFire);
        explosionEvent.AddListener(OnExplosion);
    }

    private void OnEnemyAttack(AttackPayload hit)
    {
        //Debug.Log("OnDamageTaken Audio: " + hit.entity.name + " " + hit.damage);
        PlayEntitySound(entityAttackSound, Enum.GetName(typeof(EntityKind), hit.attacker.kind));
    }

    private void OnDamageTaken(EntityPayload hit)
    {
        //Debug.Log("OnDamageTaken Audio: " + hit.entity.name + " " + hit.damage);
        PlayEntitySound(entityHitSound, Enum.GetName(typeof(EntityKind), hit.entity.kind));
    }

    private void OnDeath(EntityPayload hit)
    {
        //Debug.Log("OnDeath Audio: " + hit.entity.name + " " + hit.damage);
        PlayEntitySound(entityDeathSound, Enum.GetName(typeof(EntityKind), hit.entity.kind));
    }

    private void PlayEntitySound(FMODUnity.EventReference sound, string kind)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(sound);
        instance.setParameterByNameWithLabel("EntityKind", kind);
        instance.start();
    }

    private void OnWeaponFire(AttackPayload weapon)
    {
        //Debug.Log("WeaponFire Audio: " + weapon.weapon.name);
        var instance = FMODUnity.RuntimeManager.CreateInstance(weaponFireSound);
        instance.setParameterByNameWithLabel("WeaponKind", Enum.GetName(typeof(WeaponKind), weapon.kind));
        instance.start();
    }

    private void OnExplosion()
    {
        //Debug.Log("Explosion Audio: " + weapon.weapon.name);
        var instance = FMODUnity.RuntimeManager.CreateInstance(weaponFireSound);
        instance.setParameterByNameWithLabel("WeaponKind", Enum.GetName(typeof(WeaponKind), WeaponKind.Explosion));
        instance.start();
    }

    private void OnDisable()
    {
        entityHitEvent.RemoveListener(OnDamageTaken);
        entityDeathEvent.RemoveListener(OnDeath);
        enemyAttackEvent.RemoveListener(OnEnemyAttack);
        weaponFireEvent.RemoveListener(OnWeaponFire);
        explosionEvent.RemoveListener(OnExplosion);
    }
}
