using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class thing {
    protected int max_hp;
    protected int cur_hp;
    protected int block;
    //★플레이어 및 몬스터 스프라이트

    public GameObject combat_board;
    public List<abst_power> powers;
    protected node location;

    public thing() { init(); }

    public void init() {
        if (powers == null) { powers = new List<abst_power>(); } else { powers.Clear(); }
        personal_init();
        cur_hp = max_hp;
        block = 0;
    }

    protected abstract void personal_init();

    public void arrange_powers() {
        int index = 0;
        foreach (abst_power ap in powers) {
            ap.get_block().GetComponent<RectTransform>().localPosition = new Vector2(0, -50 - 100 * index);
            ap.update_power();
        }
    }

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
    }
    public int get_block() { return block; }
    public void set_block(bool is_plus, int i) {
        if (is_plus) {
            block += i;
        } else {
            block = i;
        }

        if (block < 0) {
            block = 0;
            //★방어도 이펙트 숨기기
        }
    }
    public List<abst_power> get_powers() { return powers; }
    public node get_location() { return location; }
    public void set_location(node n) { location = n; /*★개체 이동에 따른 처리*/ }
    #endregion get_set
}