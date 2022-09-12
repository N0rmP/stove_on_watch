using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_action
{

    protected string action_name;
    public string action_name_ { get { return action_name; } }

    protected abst_power power_made_by_this;
    //protected thing owner;    ★enemy action에도 전용 owner를 만들 것

    public abstract void use(); //★버튼을 클릭하자마자 사용불가능하게 만들 구조 강구
    public virtual void update() { }    //update its description or max cost
}