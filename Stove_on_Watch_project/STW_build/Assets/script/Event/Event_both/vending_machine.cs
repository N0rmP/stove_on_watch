using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vending_machine : abst_event { 

    protected override void choice1() {
        if (GameManager.g.get_Plr().get_shards() >= 30) {
            GameManager.g.get_Plr().set_shards(true, -30);
            GameManager.g.hp_change(GameManager.g.get_Plr(), 5);
            is_event_end = true;
        }
    }

    protected override void choice2() {
        is_event_end = true;
    }
}
