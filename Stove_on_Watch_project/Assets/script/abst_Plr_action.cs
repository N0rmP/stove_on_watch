using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_Plr_action : abst_action
{
    protected int max_cost;
    protected int max_cooltime;
    protected int cur_cooltime;
    private int temp_cost;  //save the amount of cost usage between button click and actual use()
    protected bool is_savable;  //if max cost is described by ≤ then it's true
    public enum concepts { simple, complex, haste, calm, plan }
    protected concepts[] tags;

    public abst_Plr_action() { 
        
    }

    #region get_set
    public void set_cur_cooltime(bool is_plus, int i) {
        if (is_plus) { this.cur_cooltime = (this.cur_cooltime + i < 0) ? 0 : this.cur_cooltime + i; }
        else { this.cur_cooltime = (i < 0) ? 0 : i; }
        //★사용 가능 여부와 불가능 여부 표현할 방법 찾기
    }
    #endregion get_set
}
