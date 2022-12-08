using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despair : abst_enemy_action
{
    //if decrease_param == -1, this is used by mother and the value should be decided by ROLL
    private int decrease_value;

    public despair(abst_enemy ae, int decrease_param = 20) : base(ae) {
        decrease_value = decrease_param;
        action_description += decrease_param.ToString();
    }

    public override void use() {
        GameManager.g.get_Plr().set_cur_hope(true, -decrease_value);
    }
}
