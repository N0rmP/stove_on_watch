using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_end : abst_action
{
    public override void use()
    {
        GameManager.g.turn_end();
    }
}
