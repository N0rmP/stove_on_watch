using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mother : abst_root_enemy
{
    public mother(int x, int y) : base(x, y) {
        actions_per_turn = 2;
        initial_action_list = new List<abst_enemy_action> { new embrace(this), new embrace(this), new embrace(this), new despair_mother(this)};
        passives.Add(new fear(this));
        passives.Add(new mercy(this));
        chase_module = new stationary(this);
        list_reset();
    }
}
