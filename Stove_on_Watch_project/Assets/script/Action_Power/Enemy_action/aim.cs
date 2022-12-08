using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aim : abst_enemy_action
{
    public aim(abst_enemy a) : base(a) { }

    public override void use() {
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), 7);
    }
}
