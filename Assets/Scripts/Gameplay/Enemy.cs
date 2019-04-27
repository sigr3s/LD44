using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float hp = 30f;
    public GameObject deathPrefab;

    public bool TakeDamage(float damage)
    {
        Instantiate(deathPrefab, transform);
        GetComponentInParent<MapObject>().RegisterChanges(new BasicMapObjectData{ shouldSpawn = false});        

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1f);
        return true;
    }
}