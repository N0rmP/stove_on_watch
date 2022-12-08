using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turbulence : abst_event
{
    protected override void choice1() {
        int temp_ran;
        do {
            temp_ran = GameManager.g.ran.xoshiro_range(121);
        } while (!GameManager.g.gra.is_placable(temp_ran / 11, temp_ran % 11));
        GameManager.g.get_Plr().move_to(
                    GameManager.g.get_map()[temp_ran / 11, temp_ran % 11]
                    );
        is_event_end = true;
    }

    protected override void choice2() {
        is_event_end = true;
    }
}
