using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_action
{
    protected abst_power power_made_by_this;
    //protected thing owner;    ��enemy action���� ���� owner�� ���� ��

    //public void click() { if (GameManager.g.get_is_Plr_turn()) { this.use(); } } �ڹ�ư�� click �Լ��� abst_action�� GameManager�� order_list�� ����ִ´�
    public abstract void use();
    public virtual void update() { }    //update its description or max cost
}