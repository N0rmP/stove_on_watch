using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_event : Abst_event
{
    public temp_event() : base() {
        event_name = "event_test";
    }

    protected override void choice1() {
        GameManager.g.hp_change(GameManager.g.get_Plr(), 3);
    }

    protected override void choice2() { 
        //���÷��̾ �̺�Ʈ�� ��ġ�� ������ ���� ��� ������ ���ΰ�? ����� ���� �ѱ� ��Ʈ�� ������� ��
    }
}
