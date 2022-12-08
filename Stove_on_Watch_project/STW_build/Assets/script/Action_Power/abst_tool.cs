using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_tool : abst_action
{
    public override void use() {
        tool_effect();
        GameManager.g.get_Plr().tool_inventory.Remove(this);
    }

    protected abstract void tool_effect();
}
