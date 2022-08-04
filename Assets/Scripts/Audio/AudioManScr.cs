using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManScr : MonoBehaviour
{
    public GameObject player;
    public GameObject sniper;
    public GameObject ranger;
    public GameObject charger;

    public GameObject weapon1;
    public GameObject shotgun;
    public GameObject bazooka;
    public GameObject grenade;

    public AK.Wwise.Event hit;
    public AK.Wwise.Event sniper_hit;
    public AK.Wwise.Event ranger_hit;
    public AK.Wwise.Event charger_hit;

    public AK.Wwise.Event FootstepsSFX;

    public AK.Wwise.Event ShotSingle;
    public AK.Wwise.Event Shotgun;
    public AK.Wwise.Event Baz;
    public AK.Wwise.Event DoorOpen;



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        player.GetComponent<Entity>().onHit.RemoveListener(PlayerHit);
        sniper.GetComponent<Entity>().onHit.RemoveListener(SniperHit);
        ranger.GetComponent<Entity>().onHit.RemoveListener(RangerHit);
        charger.GetComponent<Entity>().onHit.RemoveListener(ChargerHit);

    }

    public void PlayerHit(int health)
    {
        hit.Post(player);
    }
    public void SniperHit(int health)
    {
        hit.Post(player);
    }
    public void RangerHit(int health)
    {
        hit.Post(player);
    }
    public void ChargerHit(int health)
    {
        hit.Post(player);
    }

    private void OnDisable()
    {
        player.GetComponent<Entity>().onHit.AddListener(PlayerHit);
        sniper.GetComponent<Entity>().onHit.AddListener(SniperHit);
        ranger.GetComponent<Entity>().onHit.AddListener(RangerHit);
        charger.GetComponent<Entity>().onHit.AddListener(ChargerHit);
    }

    void Footsteps()
    {
        FootstepsSFX.Post(player);
    }

    void Shot()
    {
        if (weapon1.activeSelf && Input.GetButton("Fire1")) ShotSingle.Post(weapon1);
        if (shotgun.activeSelf && Input.GetMouseButtonDown(0)) Shotgun.Post(shotgun);
        if (bazooka.activeSelf && Input.GetMouseButtonDown(0)) Baz.Post(bazooka);
    }

    void Update()
    {
        Shot();
    }

    void Door()
    {
        DoorOpen.Post(gameObject);
    }
}
