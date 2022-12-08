using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graph_generator
{
    node[,] U2;  //means Connected graph
    int edge_num;
    private bool is_first_stage;
    public int elite_pos;
    //int[,] graph_distancer;

    private void init()
    {
        edge_num = 0;
        //U2 = null; U2 = new node[11, 11];
        U2 = GameManager.g.get_map();
        //for (int i = 0; i < 11; i++) { for (int j = 0; j < 11; j++) { U2[i, j] = null; } }
        //graph_distancer = new int[5, 121];
        //for (int i = 0; i < 4; i++) { for (int j = 0; j < 121; j++) { graph_distancer[i, j] = 122; } }
    }

    /*
    public node[,] graph_generate(bool is_first_stage) {
        t//his.temp_BFS();
        //★스테이지 단계에 따른 맵 생성 구현
        //★U2 메모리 해제 방법 강구
    }
    */

    #region generation
    private void generator_connect(int x1, int y1, int x2, int y2)   //connect 2 nodes in distance 1
    {
        //if (U2[x1, y1] == null) { U2[x1, y1] = new node(x1, y1); }
        //if (U2[x2, y2] == null) { U2[x2, y2] = new node(x2, y2); }

        int temp_dir = (x1 - x2 != 0) ? (1 - x1 + x2) : (2 + y1 - y2);  //if x1, x2 is same then connect with y
        if (this.U2[x1, y1].get_link()[temp_dir] != this.U2[x2, y2]) {
            this.U2[x1, y1].connect(this.U2[x2, y2], temp_dir);
            this.U2[x1, y1].set_visited(true);
            this.U2[x2, y2].set_visited(true);
            this.edge_num++;
        }
    }

    public bool is_placable(int x, int y) {
        return (!is_first_stage | x < 4 | x > 6 | y < 4 | y > 6) & (x >= 0 & x <= 10 & y >= 0 & y <= 10);
    }

    private void node_to_node(node start, node end)
    {
        if (start == end) { return; }

        node cur = start;
        int[] start_coor = start.get_coor();
        int[] end_coor = end.get_coor();
        int[] cur_coor;
        int x_change = (start_coor[0] - end_coor[0] > 0) ? -1 : 1;
        int y_change = (start_coor[1] - end_coor[1] > 0) ? -1 : 1;
        int x_gap = Mathf.Abs(start_coor[0] - end_coor[0]);
        int y_gap = Mathf.Abs(start_coor[1] - end_coor[1]);
        for (int i = x_gap + y_gap; i > 0; i--)
        {
            cur_coor = cur.get_coor();
            if (x_gap != 0 & y_gap == 0)
            {
                this.generator_connect(cur_coor[0], cur_coor[1], cur_coor[0] + x_change, cur_coor[1]); cur = cur.get_link()[1 + x_change]; x_gap--;
            }
            else if (x_gap == 0 & y_gap != 0)
            {
                this.generator_connect(cur_coor[0], cur_coor[1], cur_coor[0], cur_coor[1] + y_change); cur = cur.get_link()[2 - y_change]; y_gap--;
            }
            else
            {
                if (GameManager.g.ran.xoshiro_range(2) == 0) { this.generator_connect(cur_coor[0], cur_coor[1], cur_coor[0] + x_change, cur_coor[1]); cur = cur.get_link()[1 + x_change]; x_gap--; }
                else { this.generator_connect(cur_coor[0], cur_coor[1], cur_coor[0], cur_coor[1] + y_change); cur = cur.get_link()[2 - y_change]; y_gap--; }
            }
        }
    }

    public void temp_BFS(/*bool use_random*/)
    {
        this.init();
        is_first_stage = GameManager.g.get_is_first_stage();

        List<node> scanning = new List<node>(); //unlike usual BFS this BFS enqueue unvisited node into random index of the collection, so it uses list insted of queue
        List<int> yet_un = new List<int>();
        int[] cur_coor;
        int temp_ran = -1; int temp = 0;

        //BFS starting point
        while (true) {
            temp_ran = GameManager.g.ran.xoshiro_range(121);
            if (temp_ran / 11 < 4 | temp_ran / 11 > 6 | temp_ran % 11 < 4 | temp_ran % 11 > 6) { break; }
        }
        scanning.Add(U2[temp_ran / 11, temp_ran % 11]);


        int[] directions = new int[4] { 0, 0, 1, -1 };
        U2[temp_ran / 11, temp_ran % 11].set_visited(true);
        while (scanning.Count > temp)
        {
            cur_coor = scanning[temp].get_coor();
            for (int i = 0; i < 4; i++) {
                if (is_placable(cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i])) {
                    if (!U2[cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i]].get_visited()) {
                        //U2[cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i]] = new node(cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i]);
                        this.generator_connect(cur_coor[0], cur_coor[1], cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i]);
                        temp_ran = GameManager.g.ran.xoshiro_range(temp + 1, scanning.Count);
                        scanning.Insert(temp_ran, U2[cur_coor[0] + directions[i], cur_coor[1] + directions[3 - i]]);
                    }
                }
            }
            temp++;
        }

        if (is_first_stage) {
            //first stage, elite placement
            elite_pos = GameManager.g.ran.xoshiro_range(4);
            if (elite_pos != 0) { generator_connect(4, 4, 3, 4); generator_connect(4, 4, 4, 3); } else { generator_connect(3, 3, 3, 4); generator_connect(3, 3, 4, 3); }
            if (elite_pos != 1) { generator_connect(4, 6, 3, 6); generator_connect(4, 6, 4, 7); } else { generator_connect(3, 7, 3, 6); generator_connect(3, 7, 4, 7); }
            if (elite_pos != 2) { generator_connect(6, 4, 7, 4); generator_connect(6, 4, 6, 3); } else { generator_connect(7, 3, 7, 4); generator_connect(7, 3, 6, 3); }
            if (elite_pos != 3) { generator_connect(6, 6, 7, 6); generator_connect(6, 6, 6, 7); } else { generator_connect(7, 7, 7, 6); generator_connect(7, 7, 6, 7); }
        } else {
            generator_connect(5, 5, 4, 5);
            generator_connect(5, 5, 5, 4);
            generator_connect(5, 5, 6, 5);
            generator_connect(5, 5, 5, 6);
        }

        //start location
        generator_connect(0, 0, 0, 1); generator_connect(0, 0, 1, 0);
        generator_connect(0, 10, 0, 9); generator_connect(0, 10, 1, 10);
        generator_connect(10, 0, 10, 1); generator_connect(10, 0, 9, 0);
        generator_connect(10, 10, 10, 9); generator_connect(10, 10, 9, 10);

        //additional edge
        while (edge_num < 143)
        {
            temp_ran = GameManager.g.ran.xoshiro_range(121);
            if ((temp_ran / 11 >= 4 & temp_ran / 11 <= 6 & temp_ran % 11 >= 4 & temp_ran % 11 <= 6) | U2[temp_ran / 11, temp_ran % 11] == null) { continue; }
            yet_un.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (U2[temp_ran / 11, temp_ran % 11].get_link()[i] == null &
                temp_ran / 11 + directions[i] >= 0 & temp_ran / 11 + directions[i] <= 10 & temp_ran % 11 + directions[3 - i] >= 0 & temp_ran % 11 + directions[3 - i] <= 10 &
                !(temp_ran / 11 + directions[i] >= 4 & temp_ran / 11 + directions[i] <= 6 & temp_ran % 11 + directions[3 - i] >= 4 & temp_ran % 11 + directions[3 - i] <= 6))
                { yet_un.Add(i); }
            }
            if (yet_un.Count == 0) { continue; }
            temp = GameManager.g.ran.xoshiro_range(yet_un.Count);
            generator_connect(temp_ran / 11, temp_ran % 11, temp_ran / 11 + directions[yet_un[temp]], temp_ran % 11 + directions[3 - yet_un[temp]]);
        }
    }
    #endregion generation
}
