using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wither : abst_event
{
    player Plr = GameManager.g.get_Plr();

    protected override void choice1() {
        if (Plr.get_cur_hope() >= 50) {
            Plr.set_cur_hope(true, -50);
            Plr.set_cur_hp(true, 20);
            cur_pos.event_here_ = null;
            is_event_end = true;
        }
    }

    protected override void choice2() {
        is_event_end = true;
    }
}
