using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIHp : MonoBehaviour {
    public TextMeshProUGUI hpText;

    public List<Image> extraBars;

    
    [Header("Manager")]
    public GameController controller;
    public List<UIHpElement> uiHpElements;

    void Update()
    {
        int playerhp = GameController.Instance.player.playerData.hp;

        for(int i = 0; i < uiHpElements.Count; i ++){
            uiHpElements[i].SetState(i+1 > playerhp);
        }
        
    }
}