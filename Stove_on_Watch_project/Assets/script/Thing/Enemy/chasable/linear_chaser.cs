using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linear_chaser : abst_chasable
{
    linear_chaser(abst_enemy e) : base(e) { }

    public override void search() {
        node temp_plr_loc = GameManager.g.get_Plr().get_location();
        node temp_node;
        for (int i = 0; i < 4; i++) {
            temp_node = owner.get_location().get_link()[i];
            while (temp_node != null) {
                if (temp_node == temp_plr_loc) {
                    state_update(true);
                    return;
                }
                temp_node = temp_node.get_link()[i];
            }
        }
        state_update(false);
    }

    public void route_BFS() {
        int direction;
        node temp_plr_loc = GameManager.g.get_Plr().get_location();
        if (temp_plr_loc.get_coor()[0] == owner.get_location().get_coor()[0]) {
            if (temp_plr_loc.get_coor()[1] > owner.get_location().get_coor()[1])
                direction = 2;
            else
                direction = 0;
        } else {
            if (temp_plr_loc.get_coor()[0] > owner.get_location().get_coor()[0])
                direction = 1;
            else
                direction = 3;
        }
        node cur_node = owner.get_location();
        while (cur_node != temp_plr_loc) {
            route.Push(direction);
            cur_node = cur_node.get_link()[direction];
        }
    }
}
