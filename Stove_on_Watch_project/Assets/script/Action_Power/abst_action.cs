using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_action
{

    protected string action_name;
    public string action_name_ { get { return action_name; } }

    protected abst_power power_made_by_this;
    //protected thing owner;    ��enemy action���� ���� owner�� ���� ��

    public abstract void use(); //�ڹ�ư�� Ŭ�����ڸ��� ���Ұ����ϰ� ���� ���� ����
    public virtual void update() { }    //update its description or max cost
}