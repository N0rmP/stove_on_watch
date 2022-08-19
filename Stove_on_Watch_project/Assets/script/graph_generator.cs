using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graph_generator
{
    node[,] U2;  //means Connected graph
    int edge_num;
    //int[,] graph_distancer;

    private void init()
    {
        edge_num = 0;
        U2 = null; U2 = new node[11, 11];
        for (int i = 0; i < 11; i++) { for (int j = 0; j < 11; j++) { U2[i, j] = null; } }
        //graph_distancer = new int[5, 121];
        //for (int i = 0; i < 4; i++) { for (int j = 0; j < 121; j++) { graph_distancer[i, j] = 122; } }
    }

    public node[,] graph_generate(bool is_first_stage) {
        this.temp_BFS();
        return this.U2;
        //★스테이지 단계에 따른 맵 생성 구현
        //★U2 메모리 해제 방법 강구
    }

    #region generation
    private void generator_connect(int x1, int y1, int x2, int y2)   //connect 2 nodes in distance 1
    {
        if (U2[x1, y1] == null) { U2[x1, y1] = new node(x1, y1); }
        if (U2[x2, y2] == null) { U2[x2, y2] = new node(x2, y2); }

        int temp_dir = (x1 - x2 != 0) ? (1 - x1 + x2) : (2 + y1 - y2);  //if x1, x2 is same then connect with y
        if (this.U2[x1, y1].link[temp_dir] != this.U2[x2, y2])
        {
            this.U2[x1, y1].connect(this.U2[x2, y2], temp_dir);
            this.U2[x1, y1].visited = true;
            this.U2[x2, y2].visited = true;
            this.edge_num++;
        }
    }

    private void node_to_node(node start, node end)
    {
        if (start == end) { return; }

        node cur = start;
        int x_change = (start.coor[0] - end.coor[0] > 0) ? -1 : 1;
        int y_change = (start.coor[1] - end.coor[1] > 0) ? -1 : 1;
        int x_gap = Mathf.Abs(start.coor[0] - end.coor[0]);
        int y_gap = Mathf.Abs(start.coor[1] - end.coor[1]);
        for (int i = x_gap + y_gap; i > 0; i--)
        {
            if (x_gap != 0 & y_gap == 0)
            {
                this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0] + x_change, cur.coor[1]); cur = cur.link[1 + x_change]; x_gap--;
            }
            else if (x_gap == 0 & y_gap != 0)
            {
                this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0], cur.coor[1] + y_change); cur = cur.link[2 - y_change]; y_gap--;
            }
            else
            {
                if (GameManager.g.ran.xoshiro_range(2) == 0) { this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0] + x_change, cur.coor[1]); cur = cur.link[1 + x_change]; x_gap--; }
                else { this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0], cur.coor[1] + y_change); cur = cur.link[2 - y_change]; y_gap--; }
            }
        }
    }

    private void temp_BFS(/*bool use_random*/)
    {
        this.init();

        List<node> scanning = new List<node>();
        List<int> yet_un = new List<int>();
        node cur = null;
        int temp_ran = -1; int temp = 0;

        while (true)
        {
            temp_ran = GameManager.g.ran.xoshiro_range(121);
            if (temp_ran / 11 < 4 | temp_ran / 11 > 6 | temp_ran % 11 < 4 | temp_ran % 11 > 6) { break; }
        }
        U2[temp_ran / 11, temp_ran % 11] = new node(temp_ran / 11, temp_ran % 11);

        int[] directions = new int[4] { 0, 0, 1, -1 };
        scanning.Add(U2[temp_ran / 11, temp_ran % 11]);
        U2[temp_ran / 11, temp_ran % 11].visited = true;
        while (scanning.Count > temp)
        {
            cur = scanning[temp];
            for (int i = 0; i < 4; i++)
            {
                if ((cur.coor[0] + directions[i] < 4 | cur.coor[0] + directions[i] > 6 | cur.coor[1] + directions[3 - i] < 4 | cur.coor[1] + directions[3 - i] > 6) &
                (cur.coor[0] + directions[i] >= 0 & cur.coor[0] + directions[i] <= 10 & cur.coor[1] + directions[3 - i] >= 0 & cur.coor[1] + directions[3 - i] <= 10))
                {
                    if (U2[cur.coor[0] + directions[i], cur.coor[1] + directions[3 - i]] == null)
                    {
                        //U2[cur.coor[0] + directions[i], cur.coor[1] + directions[3 - i]] = new node(cur.coor[0] + directions[i], cur.coor[1] + directions[3 - i]);
                        this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0] + directions[i], cur.coor[1] + directions[3 - i]);
                        temp_ran = GameManager.g.ran.xoshiro_range(temp + 1, scanning.Count);
                        scanning.Insert(temp_ran, U2[cur.coor[0] + directions[i], cur.coor[1] + directions[3 - i]]);
                    }
                }
            }
            temp++;
        }
        temp_ran = GameManager.g.ran.xoshiro_range(4);
        if (temp_ran != 0) { U2[4, 4] = new node(4, 4); generator_connect(4, 4, 3, 4); generator_connect(4, 4, 4, 3); }
        else { generator_connect(3, 3, 3, 4); generator_connect(3, 3, 4, 3); }
        if (temp_ran != 1) { U2[4, 6] = new node(4, 6); generator_connect(4, 6, 3, 6); generator_connect(4, 6, 4, 7); }
        else { generator_connect(3, 7, 3, 6); generator_connect(3, 7, 4, 7); }
        if (temp_ran != 2) { U2[6, 4] = new node(6, 4); generator_connect(6, 4, 7, 4); generator_connect(6, 4, 6, 3); }
        else { generator_connect(7, 3, 7, 4); generator_connect(7, 3, 6, 3); }
        if (temp_ran != 3) { U2[6, 6] = new node(6, 6); generator_connect(6, 6, 7, 6); generator_connect(6, 6, 6, 7); }
        else { generator_connect(7, 7, 7, 6); generator_connect(7, 7, 6, 7); }

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
                if (U2[temp_ran / 11, temp_ran % 11].link[i] == null &
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
