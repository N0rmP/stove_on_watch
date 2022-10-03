using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class thing {
    protected int max_hp;
    protected int cur_hp;
    //★플레이어 및 몬스터 스프라이트

    public List<abst_power> powers;
    protected node location;

    public thing() { init(); }

    public void init()
    {
        if (powers == null) { powers = new List<abst_power>(); } else { powers.Clear(); }
        personal_init();
        cur_hp = max_hp;
    }

    protected abstract void personal_init();

    public void move_to(node n) {
        if (location != null) {
            location.hand_thing(this, n);
        } else {
            n.add_thing(this);
        }
        location = n;
    }

    #region get_set
    public int get_max_hp() { return max_hp; }
    public void set_max_hp(int i) { max_hp = i; }
    public int get_cur_hp() { return cur_hp; }
    public void set_cur_hp(bool is_plus, int i) {
        if (is_plus) { 
            cur_hp += i;
            if (cur_hp > max_hp) {
                cur_hp = max_hp;
            }
        } else { 
            cur_hp = i;
        }
    }
    public List<abst_power> get_powers() { return powers; }
    public node get_location() { return location; }
    public void set_location(node n) { location = n; /*★개체 이동에 따른 처리*/ }
    #endregion get_set
}
