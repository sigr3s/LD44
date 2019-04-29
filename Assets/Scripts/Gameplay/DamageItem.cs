using UnityEngine;

public class DamageItem : MonoBehaviour {

    public int damage = 2;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}