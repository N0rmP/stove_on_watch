using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_enemy_action : abst_action
{
    protected abst_enemy owner;

    public abst_enemy_action(abst_enemy a) {
        this.owner = a;
    }
}
