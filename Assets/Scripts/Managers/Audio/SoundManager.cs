using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public FMOD.Studio.Bus MasterBus;
    public FMOD.Studio.Bus MusicBus;
    public FMOD.Studio.Bus SfxBus;
    [Range(0f,1f)] public float MasterVolume = 1f;
    [Range(0f,1f)] public float MusicVolume = 1f;
    [Range(0f,1f)] public float SfxVolume = 1f;

    public FMODUnity.EventReference entityAttackSound;
    public FMODUnity.EventReference entityHitSound;
    public FMODUnity.EventReference entityDeathSound;
    public FMODUnity.EventReference weaponFireSound;
    public FMODUnity.EventReference pickupSound;
    public FMODUnity.EventReference gateOpenSound;

    public EntityEvent entityHitEvent;
    public EntityEvent entityDeathEvent;
    public AttackEvent enemyAttackEvent;
    public AttackEvent weaponFireEvent;
    public GameEvent explosionEvent;
    public GameEvent bazookaPickupEvent;
    public GameEvent wavesDoneEvent;

    public static SoundManager Instance;


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
        bazookaPickupEvent ??= GameEventLoader.Load<GameEvent>("BazookaPickupEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");

        entityHitEvent?.AddListener(OnDamageTaken);
        entityDeathEvent?.AddListener(OnDeath);
        enemyAttackEvent?.AddListener(OnEnemyAttack);
        weaponFireEvent?.AddListener(OnWeaponFire);
        explosionEvent?.AddListener(OnExplosion);
        bazookaPickupEvent?.AddListener(OnPickup);
        wavesDoneEvent?.AddListener(OnGateOpen);
    }

    private IEnumerator Start() {
        while (!GameLoader.FMODReady())
            yield return null;

        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
    }

    void Update()
    {
        MasterBus.setVolume(MasterVolume);
        MusicBus.setVolume(MusicVolume);
        SfxBus.setVolume(SfxVolume);
    }

    public void SetMasterVolumeLevel (float volume) {
        MasterVolume = volume;
    }

    public void SetMusicVolumeLevel (float volume) {
        MusicVolume = volume;
    }

    public void SetSfxVolumeLevel (float volume) {
        SfxVolume = volume;
    }

    private void OnPickup()
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(pickupSound);
        instance.start();
    }

    private void OnEnemyAttack(AttackPayload hit)
    {
        PlayEntitySound(entityAttackSound, Enum.GetName(typeof(EntityKind), hit.attacker.kind));
    }

    private void OnDamageTaken(EntityPayload hit)
    {
        PlayEntitySound(entityHitSound, Enum.GetName(typeof(EntityKind), hit.entity.kind));
    }

    private void OnDeath(EntityPayload hit)
    {
        PlayEntitySound(entityDeathSound, Enum.GetName(typeof(EntityKind), hit.entity.kind));
    }

    private void OnGateOpen()
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(gateOpenSound);
        instance.start();
        instance = FMODUnity.RuntimeManager.CreateInstance(pickupSound);
        instance.start();
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

    private void OnDestroy()
    {
        entityHitEvent?.RemoveListener(OnDamageTaken);
        entityDeathEvent?.RemoveListener(OnDeath);
        enemyAttackEvent?.RemoveListener(OnEnemyAttack);
        weaponFireEvent?.RemoveListener(OnWeaponFire);
        explosionEvent?.RemoveListener(OnExplosion);
    }
}
