using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stationary : abst_chasable
{
    public stationary(abst_enemy e) : base(e) { cur_state = state.idle; }

    public override void search() { }

}
