using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quick_slash : abst_Plr_action { 
    public quick_slash() : base() {
        max_cost = initial_max_cost = 7;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.simple);
    }
    protected override void effect() {
        GameManager.g.attack(owner, GameManager.g.get_selected_enemy(), max_cost);
    }
}
