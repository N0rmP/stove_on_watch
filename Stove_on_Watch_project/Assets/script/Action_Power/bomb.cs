using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : abst_Plr_action
{
    private int count;
    public override string action_description_ { 
        get { 
            return action_description + count + " times"; 
        } 
    }

    public bomb() : base() {
        max_cost = initial_max_cost = 0;
        cur_cooltime = 0;
        is_savable = false;
        count = 4;
    }
    protected override void effect() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), 40);
        count--;
        if(count < 1)
            owner.actions_.Remove(this);
    }
}
