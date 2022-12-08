using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despair_mother : abst_enemy_action
{
    public despair_mother(abst_enemy ae) : base(ae) { }

    public override void use() {
        GameManager.g.get_Plr().set_cur_hope(true, 
            GameManager.g.ROLL(-1, -1)
            );
    }
}
