using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intercept : abst_enemy_action
{
    public intercept(abst_enemy a) : base(a) {
        power_made_by_this = new intercept_power(a);
    }

    public override void use() {
        owner.power_add(power_made_by_this);
        power_made_by_this.count_ = 1;
    }
}
