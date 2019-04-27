using System;
using UnityEngine;

[System.Serializable]
public struct PlayerData{
    [Header("Current status")]
    public float hp;
    public float attackCD;

    [Header("Respawn info")]
    public int lastCheckPoint;
    public int checkPointArea;

    [Header("Story")]
    public int retievedSouls;
}

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
    public IInteractable interactableItem;

    [Header("Player Data")]
    public PlayerData playerData;

    private void Update() {
        if(playerData.attackCD > 0){
            playerData.attackCD -= Time.deltaTime;
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
        playerData.lastCheckPoint = checkpoint.id;
        playerData.checkPointArea = checkpoint.mapArea.area;
    }

    public void Respawn()
    {
        playerData.hp = initialHP;
        playerData.attackCD = 0f;
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
        playerData.attackCD = attackCD;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject){
                Enemy enemy = colliders[i].GetComponent<Enemy>();

                if(enemy != null){
                    if(enemy.TakeDamage(damage)){
                        playerData.retievedSouls ++;
                        playerData.hp += enemy.hp;
                    }
                }
            }
        }
    }

    public void TakeDamage(float ammount)
    {
        playerData.hp -= ammount;
    }
}