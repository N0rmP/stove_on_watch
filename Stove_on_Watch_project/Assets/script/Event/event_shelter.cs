using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_shelter : abst_event {
    // Start is called before the first frame update

    public event_shelter() : base(){
        event_name = "shelter";
    }

    protected override void choice1() {     //confirm
        this.is_event_end = true;
    }
}
