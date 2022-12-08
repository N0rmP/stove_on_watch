using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collection : abst_event
{
    public collection() {
        force = true;
    }

    protected override void choice1() {
        player Plr = GameManager.g.get_Plr();
        if (Plr.get_shards() >= 20) {
            Plr.set_shards(true, -20);
            is_event_end = true;
        }
    }

    protected override void choice2() {
        abst_enemy temp_ae =  LibraryManager.li.return_enemy(cur_pos.get_coor()[0], cur_pos.get_coor()[1]);
        GameManager.g.get_wandering_enemies().Add(temp_ae);
        temp_ae.engage();
        GameManager.g.general_combat_start();
        cur_pos.event_here_ = null;
        is_event_end = true;
    }
}
