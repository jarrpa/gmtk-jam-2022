using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Teleport")]
public class TeleportAbility : Ability
{
    public float radius = 3;

    private List<GameObject> _enemies;
    public override void Activate(GameObject parent)
    {
        _enemies ??= new List<GameObject>();
        
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
        var objectsInRange = Physics2D.OverlapCircleAll(parent.transform.position, radius);

         if (objectsInRange.Length == 0)
             return;
         
         foreach (var obj in objectsInRange)
         {
             if (obj.gameObject.CompareTag("Enemy"))
             {
                 var enemyObject = obj.gameObject;
                 enemyObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(parent.transform.position.x, parent.transform.position.y,  0f));
                 enemyObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(obj.transform.position.x, obj.transform.position.y,  0f));
                 
                 _enemies.Add(enemyObject);
             }
         }
    }

    public void KeepActive(GameObject parent)
    {
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if (hit.collider == null)
            return;
        
        foreach (var enemy in _enemies.Where(enemy => ReferenceEquals(hit.collider.gameObject, enemy)))
        {
            parent.transform.position = hit.collider.transform.position;
            break;
        }
    }

    public override void Deactivate()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy == null) continue;
            enemy.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            enemy.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
        }

        _enemies.Clear();
        
        Time.timeScale = 1f; 
        Time.fixedDeltaTime = 0.02f;
    }
}
