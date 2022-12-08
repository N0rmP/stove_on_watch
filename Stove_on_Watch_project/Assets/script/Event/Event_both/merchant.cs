using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class merchant : abst_event
{
    player Plr = GameManager.g.get_Plr();

    protected override void choice1() {
        if (Plr.get_shards() >= 30) {
            Plr.set_shards(true, -30);
            GameManager.g.rew.reward_add_action();
            GraphicManager.g.reward_init();
            is_event_end = true;
        }
    }

    protected override void choice2() {
        if (Plr.actions_.Count >= 1) {
            Plr.actions_.RemoveAt(
                GameManager.g.ran.xoshiro_range(Plr.actions_.Count)
                );
            Plr.set_shards(true, 80);
            is_event_end = true;
        }
    }

    protected override void choice3() {
        is_event_end = true;
    }
}
