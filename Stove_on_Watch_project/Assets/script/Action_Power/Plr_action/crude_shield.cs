using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crude_shield : abst_Plr_action
{
    public crude_shield() : base() {
        max_cost = initial_max_cost = 22;
        cur_cooltime = 0;
        is_savable = false;
        tags.Add(concepts.complex);
    }
    protected override void effect() {
        GameManager.g.block(owner, max_cost);
    }
}
