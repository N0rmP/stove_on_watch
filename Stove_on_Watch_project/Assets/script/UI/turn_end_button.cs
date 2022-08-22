using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_end_button : MonoBehaviour
{
    private abst_action ender;
    public turn_end_button() { this.ender = new turn_end(); }

    public void click() { GameManager.g.get_order_list().Enqueue(this.ender);}
}