using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : thing
{
    private const int initial_max_hp = 20;
    private int max_hope;
    private int cur_hope;

    private List<abst_Plr_action> inventory;
    public List<abst_Plr_action> actions;   //¡Ú

    public player() {
        this.init();
    }

    public void init()
    {
        this.max_hp = initial_max_hp;
        if (inventory == null) { inventory = new List<abst_Plr_action>(); } else { inventory.Clear(); }
        if (actions == null) { actions = new List<abst_Plr_action>(); } else { actions.Clear(); }
        base.init();
    }

    #region get_set
    public int get_cur_hope() { return this.cur_hope; }
    public void set_cur_hope(bool is_plus, int i) { if (is_plus) { this.cur_hope += i; } else { this.cur_hope = i; } }
    #endregion get_set
}
