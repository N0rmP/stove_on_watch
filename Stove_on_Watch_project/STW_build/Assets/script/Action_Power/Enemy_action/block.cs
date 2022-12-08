using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : abst_enemy_action
{
    public block(abst_enemy a) : base(a) { }

    public override void use() {
        GameManager.g.block(owner, GameManager.g.ROLL());
    }
}
