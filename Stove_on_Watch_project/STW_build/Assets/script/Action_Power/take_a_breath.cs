using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_a_breath : abst_Plr_action
{
    public override void use() {
        GameManager.g.hp_change(GameManager.g.get_Plr(), 15);
    }

    protected override void effect() { }
}
