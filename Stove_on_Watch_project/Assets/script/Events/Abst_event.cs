using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_event
{
    protected string event_name { get; set; }
    protected bool is_event_end;
    protected int choice;
    public int choice_ {
        get { return choice; }
        set { if (choice < 0 | choice > 2) { choice = 2; } else { choice = value; } }
    }
    public bool choice_complete;

    public abst_event() {
        choice = 2;
    }

    public IEnumerator happen() {
        is_event_end = false;
        while (!is_event_end) {
            choice_complete = false;
            GraphicManager.g.event_output(this.event_name);
            yield return new WaitUntil(() => choice_complete);
            switch (choice) {
                case (0):
                    choice1(); break;
                case (1):
                    choice2(); break;
                case (2):
                    choice3(); break;
            }
        }
        GraphicManager.g.temp_event_remove();
    }

    protected abstract void choice1();
    protected virtual void choice2() { }
    protected virtual void choice3() { }

}
