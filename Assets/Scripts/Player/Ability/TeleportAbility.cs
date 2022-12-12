using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Teleport")]
public class TeleportAbility : Ability
{
    public float radius = 3;

    public override void Activate(GameObject parent)
    {
        PlayAbilitySound();

        Time.timeScale = 0.05f;

        var objectsInRange = Physics2D.OverlapCircleAll(parent.transform.position, radius);

        if (objectsInRange.Length == 0)
            return;
    }

    public void KeepActive(GameObject parent)
    {
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        parent.transform.position = new Vector3(mousePos.x, mousePos.y, parent.transform.position.z);
    }

    public override void Deactivate()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        soundInstance.setParameterByNameWithLabel("AbilityKind", name + "Exit");
        soundInstance.start();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
