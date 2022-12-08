using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preach : abst_enemy_action
{
    public preach(abst_enemy ae) : base(ae) { }

    public override void use() {
        if (GameManager.g.ran.xoshiro_range(2) == 0)
            GameManager.g.attack(owner, GameManager.g.get_Plr(), GameManager.g.ROLL(-1, -1));
        else
            GameManager.g.attack(owner, GameManager.g.get_Plr(), GameManager.g.ROLL(-1, 1));
    }
}
