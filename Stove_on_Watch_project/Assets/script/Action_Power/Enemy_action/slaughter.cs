using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slaughter : abst_enemy_action
{
    public slaughter(abst_enemy a) : base(a) { }

    public override void use() {
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
        GameManager.g.attack(this.owner, GameManager.g.get_Plr(), GameManager.g.ROLL());
    }
}
