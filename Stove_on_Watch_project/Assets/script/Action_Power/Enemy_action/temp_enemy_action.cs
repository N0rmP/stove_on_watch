using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_enemy_action : abst_enemy_action
{
    public temp_enemy_action(abst_enemy a) : base(a) { }

    public override void use()
    {
        int temp = GameManager.g.ROLL();
        GameManager.g.attack(this.owner , GameManager.g.get_Plr() , temp);
        Debug.Log("temp_enemt_action used, value was " + temp);
    }
}
