using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collapsed_house : abst_event
{
    protected override void choice1() {
        player Plr = GameManager.g.get_Plr();
        if (Plr.get_cur_hope() >= 50) {
            Plr.set_cur_hope(true, -50);
            GameManager.g.rew.reward_add_action();
            GameManager.g.rew.reward_add_shards(40);
            cur_pos.event_here_ = null;
            is_event_end = true;
        }        
    }

    protected override void choice2() {
        is_event_end = true;
    }
}
