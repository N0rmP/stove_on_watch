using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class embrace : abst_enemy_action
{
    public embrace(abst_enemy a) : base(a) { }

    public override void use() {
        int temp = GameManager.g.ROLL(-1, 1);
        GameManager.g.attack(owner, GameManager.g.get_Plr(), temp);
    }
}
