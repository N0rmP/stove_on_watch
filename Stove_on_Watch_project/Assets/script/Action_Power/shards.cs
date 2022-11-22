using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shards : abst_Plr_action
{
    int amount;
    public int amount_ {
        get { return amount; }
        set { amount = value; }
    }

    public override string action_description_ {
        get {
            Debug.Log(action_description + amount.ToString());
            return (action_description + amount.ToString());
        }
    }

    public shards() : base() {
        amount = 0;
    }

    public override void use() {
        GameManager.g.get_Plr().shards += amount;
        amount = 0;
    }

    protected override void effect() { }
}
