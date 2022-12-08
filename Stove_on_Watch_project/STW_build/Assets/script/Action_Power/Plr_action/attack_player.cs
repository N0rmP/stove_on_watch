using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_player : abst_Plr_action
{
    public attack_player() : base() {
        initial_max_cost = max_cost = 15;
        cur_cooltime = 0;
        is_savable = false;
    }
    protected override void effect() {
        GameManager.g.attack(this.owner, GameManager.g.get_selected_enemy(), 15);
    }
}
