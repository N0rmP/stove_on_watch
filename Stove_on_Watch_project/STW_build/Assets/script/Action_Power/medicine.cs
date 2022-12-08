using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medicine : abst_Plr_action
{
    public medicine() : base() {
        max_cost = initial_max_cost = 0;
        cur_cooltime = 0;
        is_savable = false;
    }
    protected override void effect() {
        GameManager.g.hp_change(owner, 20);
        owner.actions_.Remove(this);
    }
}
