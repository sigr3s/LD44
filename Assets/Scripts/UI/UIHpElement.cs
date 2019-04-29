using UnityEngine;

public class UIHpElement : MonoBehaviour {
    bool empty = false;
    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void SetState(bool empty){
        if(empty != this.empty){
            if(empty){
                anim.SetTrigger("loselife");
            }
            else
            {
                anim.SetTrigger("getlife");
            }

            this.empty = empty;
        }
    }
}