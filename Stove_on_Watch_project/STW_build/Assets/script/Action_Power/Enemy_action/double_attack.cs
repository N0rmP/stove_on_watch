using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class double_attack : abst_enemy_action {
    public double_attack(abst_enemy a) : base(a) { }

    public override void use() {
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
    }
}
