using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHp : MonoBehaviour {
    public Image hpBar;
    public TextMeshProUGUI hpText;

    
    [Header("Manager")]
    public GameController controller;

    void Update()
    {
        hpText.text =  Mathf.CeilToInt(controller.player.playerData.hp).ToString();

        hpBar.fillAmount = controller.player.playerData.hp / 300f;
    }
}