using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweep : abst_enemy_action {

    public sweep(abst_enemy ae) : base(ae) { }

    public override void use() {
        int temp = GameManager.g.ROLL(-1, 1);
        GameManager.g.attack(owner, GameManager.g.get_Plr(), temp);
    }
}
