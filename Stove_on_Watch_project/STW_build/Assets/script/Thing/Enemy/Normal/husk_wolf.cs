using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class husk_wolf : abst_enemy
{
    public husk_wolf(int x, int y) : base(x, y) {
        actions_per_turn = 1;
        initial_action_list = new List<abst_enemy_action> { new attack(this), new double_attack(this), new slaughter(this) };
        passives.Add(new husk(this));
        chase_module = new linear_chaser(this);
        list_reset();
    }
}
