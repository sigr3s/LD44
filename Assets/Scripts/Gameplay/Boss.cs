using UnityEngine;
using UnityEngine.Events;

public class Boss : Enemy {

    public UnityEvent onBossDefeated;

    public bool invencible = false;

    public override bool TakeDamage(int damage){
        if(invencible) return false;

        hp -= damage;
        invencible = true;

        if(hp <= 0){
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        
            foreach(SpriteRenderer renderer in sprites){
                renderer.enabled = false;
            }

            onBossDefeated.Invoke();

            Instantiate(deathPrefab, transform);

            Destroy(gameObject, 1f);
            return true;
        }
        else
        {
            GetComponentInChildren<Animator>().SetTrigger("hit");
        }

        return false;
    }

}