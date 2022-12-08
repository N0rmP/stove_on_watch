using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shelter : abst_event {
    // Start is called before the first frame update

    /*
    public shelter() : base(){
        event_name = "shelter";
    }*/

    protected override void choice1() {     //confirm
        this.is_event_end = true;
    }
}
