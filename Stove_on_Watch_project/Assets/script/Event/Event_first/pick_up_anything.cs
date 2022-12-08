using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_up_anything : abst_event
{
    protected override void choice1() {
        GameManager.g.get_Plr().passives_.Add(new pick_up_anything_power(GameManager.g.get_Plr()));
        cur_pos.event_here_ = null;
        is_event_end = true;
    }
}
