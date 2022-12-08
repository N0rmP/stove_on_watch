using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mercenary : abst_event
{
    player Plr = GameManager.g.get_Plr();

    protected override void choice1() {
        if (Plr.get_cur_hope() >= 100) {
            Plr.set_cur_hope(true, -100);
            Plr.passives_.Add(new mercenary_power(Plr));
            cur_pos.event_here_ = null;
            is_event_end = true;
        }
    }

    protected override void choice2() {
        is_event_end = true;
    }
}
