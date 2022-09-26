using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_Plr_action : abst_action
{

    protected int initial_max_cost;
    protected int max_cost;
    protected int cur_cooltime;
    private int temp_cost;  //save the amount of cost usage between button click and actual use(), ★
    protected bool is_savable;  //if max cost is described by ≤ then it's true, ★아마 카드 정보 표시할 때만 사용될 듯
    public enum concepts { simple, complex, haste, calm, plan }
    protected HashSet<concepts> tags;
    protected player owner;

    public abst_Plr_action() {
        owner = GameManager.g.get_Plr();
        tags = new HashSet<concepts>();
    }

    public override void use() {
        if (this.cur_cooltime <= 0)
        {
            effect();
            if (tags.Contains(concepts.simple)) { cur_cooltime = 1; }
            else if (tags.Contains(concepts.complex)) { cur_cooltime = 3; }
            else { cur_cooltime = 2; }
            GameManager.g.last_used = this;
        }
        else {
            Debug.Log("its in cooltime");
        }
    }

    protected abstract void effect();

    protected int cost_choice() {
        //★슬라이더를 사용해 this.max_cost 이하의 희망 사용량 지정
        return 0;
    }
    #region get_set
    public int get_cur_cooltime() { return this.cur_cooltime; }
    public void set_cur_cooltime(bool is_plus, int i) {
        if (is_plus) { this.cur_cooltime = (this.cur_cooltime + i < 0) ? 0 : this.cur_cooltime + i; }
        else { this.cur_cooltime = (i < 0) ? 0 : i; }
        //★사용 가능 여부와 불가능 여부 표현할 방법 찾기
    }
    #endregion get_set
}
