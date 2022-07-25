using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_Plr_action : MonoBehaviour
{
    protected int max_cost;
    protected int max_cooltime;
    protected int cur_cooltime;
    protected bool is_savable;  //if cost described by ¡Â then it's true
    protected enum concepts { simple, complex, haste, calm, plan }
    protected concepts[] tag;

    public abst_Plr_action() {  }

    public virtual void on_use() { }    //effect when used the action itself
    public virtual void effect1() { }   //effect activated in some time (ex : when Plr's turn end)
    public virtual void effect2() { }   //effect assisting effect1 (ex : remove it when its effect is over)
    public virtual void update() { }    //update its description or max cost

    public virtual void cooldown() {
        this.cur_cooltime--;   
    }
}
