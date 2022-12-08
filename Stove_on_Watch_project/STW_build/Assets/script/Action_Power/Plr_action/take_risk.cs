using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_risk : abst_Plr_action
{
    public take_risk() : base() {
        max_cost = initial_max_cost = 15;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.simple);
    }
    protected override void effect() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), max_cost);
        GameManager.g.hp_change(owner, -2);
    }
}
