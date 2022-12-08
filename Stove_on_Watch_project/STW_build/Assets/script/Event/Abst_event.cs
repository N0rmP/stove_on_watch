using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_event
{
    //public string event_name { get; set; }
    protected bool is_event_end;
    protected int choice;
    public bool choice_complete;
    protected node cur_pos;
    protected bool force = false;
    public bool is_event_end_ {
        get { return is_event_end; }
        set { is_event_end = value; }
    }
    public int choice_ {
        get { return choice; }
        set { if ((value < 0) | (value > 2)) { choice = 2; } else { choice = value; } }
    }
    public node cur_pos_ {
        get { return cur_pos; }
        set { cur_pos = value; }
    }
    public bool force_ {
        get { return force; }
    }
    

    public abst_event() {
        choice = -1;
    }

    public IEnumerator happen() {
        //★옵저버 패턴 개새끼야
        is_event_end = false;
        GraphicManager.g.event_recover();
        while (!is_event_end) {
            choice_complete = false;
            GraphicManager.g.event_output(GetType().ToString());
            yield return new WaitUntil(() => choice_complete);
            Debug.Log(choice);
            switch (choice) {
                case (0):
                    choice1(); break;
                case (1):
                    choice2(); break;
                case (2):
                    choice3(); break;
                default:
                    break;
            }
        }
        GraphicManager.g.event_remove();
    }

    protected abstract void choice1();
    protected virtual void choice2() { }
    protected virtual void choice3() { }

}
