using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class near_chaser : abst_chasable
{
    public near_chaser(abst_enemy e) : base (e) { }

    public override void search() {
        int gap = 0;
        gap += Mathf.Abs(owner.get_location().get_coor()[0] - GameManager.g.get_Plr().get_location().get_coor()[0])
             + Mathf.Abs(owner.get_location().get_coor()[1] - GameManager.g.get_Plr().get_location().get_coor()[1]);
        state_update(gap <= 2);
    }
}
