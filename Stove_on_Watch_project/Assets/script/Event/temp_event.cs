using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_event : abst_event
{
    public temp_event() : base() {
        event_name = "event_test";
    }

    protected override void choice1() {     //hp +3
        GameManager.g.hp_change(GameManager.g.get_Plr(), 3);
    }

    protected override void choice2() {     //leave
        this.is_event_end = true;
    }
}
