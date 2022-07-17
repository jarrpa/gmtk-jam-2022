using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManScr : MonoBehaviour
{
    public GameObject player;
    public AK.Wwise.Event hit;
    private void OnEnable()
    {
        player.GetComponent<Entity>().OnDeath += PlaySound;
    }

    public void PlaySound()
    {
        hit.Post(player);
    }

    private void OnDisable()
    {
        player.GetComponent<Entity>().OnDeath -= PlaySound;
    }

    private void Start()
    {
        StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(5f);
        player.GetComponent<Entity>().TakeDamage(100);
    }
}
