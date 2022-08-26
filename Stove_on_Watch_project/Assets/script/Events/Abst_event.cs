using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abst_event
{
    protected string event_name { get; set; }
    public int choice {
        get { return choice; }
        set { if (choice < 0 | choice > 2) { choice = 2; } } 
    }

    public Abst_event() {
        choice = 2;
    }

    public IEnumerator happen() {
        int temp = choice;
        GraphicManager.g.temp_event_recover();
        GraphicManager.g.event_placement(this.event_name);
        yield return new WaitWhile(() => choice==temp);
        switch (choice) {
            case (0):
                choice1(); break;
            case (1):
                choice2(); break;
            case (2):
                choice3(); break;
        }
    }

    protected abstract void choice1();
    protected virtual void choice2() { }
    protected virtual void choice3() { }

}
