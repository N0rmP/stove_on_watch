using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing
{
    protected int max_hp;
    protected int cur_hp;
    //���÷��̾� �� ���� ��������Ʈ

    public List<abst_power> powers;  //��private���� ����� ��� ����

    public void init()
    {
        this.cur_hp = this.max_hp;
        if (this.powers == null) { this.powers = new List<abst_power>(); } else { this.powers.Clear(); }
    }

    #region get_set
    public int get_max_hp() { return this.max_hp; }
    public void set_max_hp(int i) { this.max_hp = i; }
    public int get_cur_hp() { return this.cur_hp; }
    public void set_cur_hp(bool is_plus, int i) {
        if (is_plus) { this.cur_hp += i; }
        else { this.cur_hp = i; }
    }
    #endregion get_set
}
