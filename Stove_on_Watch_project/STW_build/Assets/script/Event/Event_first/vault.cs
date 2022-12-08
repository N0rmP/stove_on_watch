using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vault : abst_event { 
    protected override void choice1() {
        GameManager.g.rew.reward_add_shards(30);
        GraphicManager.g.reward_init();
        GameManager.g.place_enemy();
        cur_pos.event_here_ = null;
        this.is_event_end = true;
    }

    protected override void choice2() {
        this.is_event_end = true;
    }
}
