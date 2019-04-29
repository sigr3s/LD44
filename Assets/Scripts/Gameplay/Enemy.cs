using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int hp = 2;
    public GameObject deathPrefab;

    public int playerRecoverOnKill = 1;


    public virtual bool TakeDamage(int damage)
    {
        GetComponentInParent<MapObject>().RegisterChanges(new BasicMapObjectData{ shouldSpawn = false});        

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        
        foreach(SpriteRenderer renderer in sprites){
            renderer.enabled = false;
        }

        GetComponent<Collider2D>().enabled = false;
        Instantiate(deathPrefab, transform);

        Destroy(gameObject, 1f);
        return true;
    }
}