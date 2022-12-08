using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class thing {
    protected int max_hp;
    protected int cur_hp;
    protected int block;

    protected GameObject combat_board;
    protected List<abst_power> powers;
    protected List<abst_power> passives;
    protected node location;
    public GameObject combat_board_ {
        get { return combat_board; }
        set { combat_board = value; }
    }

    public List<abst_power> passives_ { get { return passives; } }


    public thing() { init(); }

    public void init() {
        if (powers == null) { powers = new List<abst_power>(); } else { powers.Clear(); }
        if (passives == null) { passives = new List<abst_power>(); } else { passives.Clear(); }
        personal_init();
        set_cur_hp(false, max_hp);
        block = 0;
    }

    protected abstract void personal_init();

    public void arrange_powers() {
        int index = 0;
        foreach (abst_power ap in powers) {
            ap.update_power();
            if (ap.get_visible())
                ap.get_block().GetComponent<RectTransform>().localPosition = new Vector2(0, -50 - 100 * index);
        }
    }

    public void move_to(node n) {
        if (location != null) {
            location.hand_thing(this, n);
        } else {
            n.add_thing(this);
        }
        location = n;
        n.POV_process(false);
    }

    #region get_set
    public int get_max_hp() { return max_hp; }
    public virtual void set_max_hp(int i) { max_hp = i; }
    public int get_cur_hp() { return cur_hp; }
    public virtual void set_cur_hp(bool is_plus, int i) {
        if (is_plus) {
            cur_hp += i;
            if (cur_hp > max_hp) {
                cur_hp = max_hp;
            }
        } else {
            cur_hp = i;
        }

        if (combat_board != null) { 
            if(is_plus)
                GraphicManager.g.countings_add(combat_board.transform.GetChild(0).GetChild(0).gameObject, cur_hp);
            else
                GraphicManager.g.set_text(combat_board.transform.GetChild(0).GetChild(0).gameObject, cur_hp.ToString());
        } 
    }
    public int get_block() { return block; }
    public void set_block(bool is_plus, int i) {
        if (is_plus) {
            block += i;
        } else {
            block = i;
        }

        if (block <= 0) {
            block = 0;
        }

        if (combat_board != null) {
            if (is_plus)
                GraphicManager.g.countings_add(combat_board.transform.GetChild(4).GetChild(0).gameObject, block);
            else
                GraphicManager.g.set_text(combat_board.transform.GetChild(4).GetChild(0).gameObject, block.ToString());
        }
    }
    public void power_add(abst_power ap) {
        if (powers.Contains(ap))
            return;

        powers.Add(ap);
        if (ap.get_visible())
            GraphicManager.g.prepare_power_block(ap);
        if (get_cur_hp() > 0)
            arrange_powers();
    }
    public void power_remove(abst_power ap) {
        if (ap.get_visible())
            ap.return_block();
        powers.Remove(ap);
        if (get_cur_hp() > 0)
            arrange_powers();
    }
    public List<abst_power> get_powers() { return powers; }
    public node get_location() { return location; }
    public void set_location(node n) { location = n; /*★개체 이동에 따른 처리*/ }
    #endregion get_set
}