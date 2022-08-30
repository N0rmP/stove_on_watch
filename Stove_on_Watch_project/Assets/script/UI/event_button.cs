using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class event_button : MonoBehaviour
{
    public int event_button_order;
    public void on_click() {
        GameManager.g.cur_event.choice_ = event_button_order;
        GameManager.g.cur_event.choice_complete = true;
    }
}
