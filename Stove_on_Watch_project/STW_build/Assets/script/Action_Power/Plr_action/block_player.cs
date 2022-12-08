using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block_player : abst_Plr_action
{
    public block_player() : base() {
        initial_max_cost = max_cost = 15;
        cur_cooltime = 0;
        is_savable = false;
    }
    protected override void effect() {
        GameManager.g.block(this.owner, max_cost);
    }
}
