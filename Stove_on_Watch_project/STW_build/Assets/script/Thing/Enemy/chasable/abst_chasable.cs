using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class abst_chasable
{
    protected abst_enemy owner;
    protected Stack<int> route;
    public enum state {idle, patrol, chase};
    protected state cur_state;

    public abst_chasable(abst_enemy e) {
        owner = e;
        route = new Stack<int>();
    }

    public abstract void search();
    public void route_BFS() {
        route.Clear();
        List<node> checking = new List<node>();
        checking.Add(owner.get_location());
        int check_index = 0;
        node[] temp_link;
        while (check_index < checking.Count) {
            if (checking[check_index] == GameManager.g.get_Plr().get_location())
                break;
            temp_link = checking[check_index].get_link();
            for (int i = 0; i < 4; i++) {
                if (temp_link[i] == null || temp_link[i].route_homedirection != -1)
                    continue;
                temp_link[i].route_homedirection = (i > 1) ? (i - 2) : (i + 2);
                checking.Add(temp_link[i]);
            }
            check_index++;
        }
        if (check_index == checking.Count) {
            Debug.Log("route_BFS is malfunctioning");
            return;
        }
        node temp_node = checking[check_index];
        while (temp_node != owner.get_location()) {
            route.Push((temp_node.route_homedirection > 1) ? (temp_node.route_homedirection - 2) : (temp_node.route_homedirection + 2));
            temp_node = temp_node.get_link()[temp_node.route_homedirection];
        }
        foreach (node n in checking)
            n.route_homedirection = -1;
    }
    public int get_next_move() {
        search();
        if (cur_state == state.idle || route.Count < 1)
            return -1;
        else
            return route.Pop();
    }
    public void state_update(bool condition) {
        if (condition) {
            cur_state = state.chase;
            route_BFS();
        } else if (cur_state == state.chase)
            cur_state = state.patrol;
        else if (route.Count == 0)
            cur_state = state.idle;
    }
    public state get_cur_state() {
        return cur_state;
    }
}
