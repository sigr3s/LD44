using System;
using UnityEngine;

public class Player : MonoBehaviour {

    //Manager should inject self here
    public IGameController gameController;


    [Header("Settings")]
    public float initialHP = 100f;
    public float attackRadius = 2f;
    public float attackCD = 0.5f;
    public float damage = 1f;
    public float hpTradePerSecond = 2f;
    public float hpTradeModifier = 2f;



    [Header("Current status")]
    public float hp = 100f;
    public float actualAttackCD = 0;
    public Checkpoint lastCheckPoint = null;
    public int retievedSouls = 0;
    public IInteractable interactableItem;

    private void Start() {
        hp = initialHP;
    }

    private void Update() {
        if(actualAttackCD > 0){
            actualAttackCD -= Time.deltaTime;
        }
    }

    public void Interact()
    {
        if(interactableItem != null){
            interactableItem.Interact(this);
        }
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        lastCheckPoint = checkpoint;
    }

    public void Respawn()
    {
        hp = initialHP;
        transform.position = lastCheckPoint.transform.position;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactableItem = interactable;
    }

    public void Hability()
    {
        gameController.TradeHpForTime(hpTradePerSecond * Time.deltaTime, hpTradeModifier, this);
    }

    public void Attack()
    {
        actualAttackCD = attackCD;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject){
                Enemy enemy = colliders[i].GetComponent<Enemy>();

                if(enemy != null){
                    if(enemy.TakeDamage(damage)){
                        retievedSouls ++;
                        hp += enemy.hp;
                    }
                }
            }
        }
    }

    public void TakeDamage(float ammount)
    {
        hp -= ammount;
    }
}