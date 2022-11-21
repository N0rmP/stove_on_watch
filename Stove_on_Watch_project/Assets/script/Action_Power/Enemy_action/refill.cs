using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refill : abst_enemy_action { 
    
    public refill(abst_enemy ae) : base(ae){ }

    public override void use() {
        int temp = 0;
        for (int i = 0; i < 3; i++) {
            temp = GameManager.g.ROLL(-1, -1);
            GameManager.g.block(owner, temp);
        }
    }
}
