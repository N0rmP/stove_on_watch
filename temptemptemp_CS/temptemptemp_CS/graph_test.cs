using System;
using System.Collections.Generic;
using System.Text;

namespace temptemptemp_CS
{
    class graph_test
    {
        //Node[,] U1;  //UnConnected
        Node[,] U2;  //Connected
        random_test ran;
        int edge_num;

        public graph_test() {
            //U1 = new Node[11, 11];
            U2 = new Node[11, 11];
            for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    //U1[i, j] = null;
                    U2[i, j] = null;
                }
            }
            ran = new random_test();

            edge_num = 0;
        }

        static void Main() {
            graph_test gra = new graph_test();

            Node temp1 = new Node(); Node temp2 = new Node(); Node temp3 = new Node(); Node temp4 = new Node();
            List<int[]> unconnected = new List<int[]>(); List<int[]> temp_route = new List<int[]>(); 
            int ran_num = 0; int route_counter;

            //Preparation
            /*for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    if (i < 4 | i > 6 | j < 4 | j > 6)
                    {
                        gra.U1[i, j] = new Node(i, j);
                    }
                }
            }*/
            gra.generator_connect(0, 0, 0, 1); gra.generator_connect(0, 0, 1, 0);
            gra.generator_connect(0, 10, 0, 9); gra.generator_connect(0, 10, 1, 10);
            gra.generator_connect(10, 0, 10, 1); gra.generator_connect(10, 0, 9, 0);
            gra.generator_connect(10, 10, 10, 9); gra.generator_connect(10, 10, 9, 10);
            //abvoe_outline start node connect, below_middle elite node connect
            gra.ran.seed();
            ran_num = gra.ran.xoshiro_range(4);
            if (ran_num != 0) { gra.U2[4, 4] = new Node(4, 4); gra.generator_connect(4, 4, 3, 4); gra.generator_connect(4, 4, 4, 3); }
            else { gra.generator_connect(3, 3, 3, 4); gra.generator_connect(3, 3, 4, 3); }
            if (ran_num != 1) { gra.U2[4, 6] = new Node(4, 6); gra.generator_connect(4, 6, 3, 6); gra.generator_connect(4, 6, 4, 7); }
            else { gra.generator_connect(3, 7, 3,6); gra.generator_connect(3, 7, 4, 7); }
            if (ran_num != 2) { gra.U2[6, 4] = new Node(6, 4); gra.generator_connect(6, 4, 7, 4); gra.generator_connect(6, 4, 6, 3); }
            else { gra.generator_connect(7, 3, 7, 4); gra.generator_connect(7, 3, 6, 3); }
            if (ran_num != 3) { gra.U2[6, 6] = new Node(4, 6); gra.generator_connect(6, 6, 7, 6); gra.generator_connect(6, 6, 6, 7); }
            else { gra.generator_connect(7, 7, 7, 6); gra.generator_connect(7, 7, 6, 7); }

            //skeletal line generate
            try
            {
                ran_num = gra.ran.xoshiro_range(20);   //  1/4, upper
                //temp1 = (gra.U1[ran_num / 5, 1 + ran_num % 5] == null) ? gra.U2[ran_num / 5, 1 + ran_num % 5] : gra.U1[ran_num / 5, 1 + ran_num % 5];
                if (gra.U2[ran_num / 5, 1 + ran_num % 5] == null)
                { temp1 = gra.U2[ran_num / 5, 1 + ran_num % 5] = new Node(ran_num / 5, 1 + ran_num % 5); }
                else { temp1 = gra.U2[ran_num / 5, 1 + ran_num % 5]; }
                gra.node_to_node(gra.U2[0, 1], temp1);
                gra.node_to_node(temp1, gra.U2[3, 4]);
                ran_num = gra.ran.xoshiro_range(20);
                //temp1 = (gra.U1[ran_num / 5, 5 + ran_num % 5] == null) ? gra.U2[ran_num / 5, 5 + ran_num % 5] : gra.U1[ran_num / 5, 5 + ran_num % 5];
                if (gra.U2[ran_num / 5, 5 + ran_num % 5] == null)
                { temp2 = gra.U2[ran_num / 5, 5 + ran_num % 5] = new Node(ran_num / 5, 5 + ran_num % 5); }
                else { temp2 = gra.U2[ran_num / 5, 5 + ran_num % 5]; }
                gra.node_to_node(gra.U2[0, 9], temp2);
                gra.node_to_node(temp2, gra.U2[3, 6]);
                gra.node_to_node(temp1, temp2);

                ran_num = gra.ran.xoshiro_range(20);   //  2/4, right
                //temp1 = (gra.U1[1 + ran_num / 4, 7 + ran_num % 4] == null) ? gra.U2[1 + ran_num / 4, 7 + ran_num % 4] : gra.U1[1 + ran_num / 4, 7 + ran_num % 4];
                if (gra.U2[1 + ran_num / 4, 7 + ran_num % 4] == null)
                { temp1 = gra.U2[1 + ran_num / 4, 7 + ran_num % 4] = new Node(1 + ran_num / 4, 7 + ran_num % 4); }
                else { temp1 = gra.U2[1 + ran_num / 4, 7 + ran_num % 4]; }
                gra.node_to_node(gra.U2[1, 10], temp1);
                gra.node_to_node(temp1, gra.U2[4, 7]);
                ran_num = gra.ran.xoshiro_range(20);
                //temp1 = (gra.U1[6 + ran_num / 4, 7 + ran_num % 4] == null) ? gra.U2[6 + ran_num / 4, 7 + ran_num % 4] : gra.U1[6 + ran_num / 4, 7 + ran_num % 4];
                if (gra.U2[6 + ran_num / 4, 7 + ran_num % 4] == null)
                { temp2 = gra.U2[6 + ran_num / 4, 7 + ran_num % 4] = new Node(6 + ran_num / 4, 7 + ran_num % 4); }
                else { temp2 = gra.U2[6 + ran_num / 4, 7 + ran_num % 4]; }
                gra.node_to_node(gra.U2[9, 10], temp2);
                gra.node_to_node(temp2, gra.U2[6, 7]);
                gra.node_to_node(temp1, temp2);

                ran_num = gra.ran.xoshiro_range(20);   //  3/4, down
                //temp1 = (gra.U1[7 + ran_num / 5, 6 + ran_num % 5] == null) ? gra.U2[7 + ran_num / 5, 6 + ran_num % 5] : gra.U1[7 + ran_num / 5, 6 + ran_num % 5];
                if (gra.U2[7 + ran_num / 5, 6 + ran_num % 5] == null)
                { temp1 = gra.U2[7 + ran_num / 5, 6 + ran_num % 5] = new Node(7 + ran_num / 5, 6 + ran_num % 5); }
                else { temp1 = gra.U2[7 + ran_num / 5, 6 + ran_num % 5]; }
                gra.node_to_node(gra.U2[10, 9], temp1);
                gra.node_to_node(temp1, gra.U2[7, 6]);
                ran_num = gra.ran.xoshiro_range(20);
                //temp1 = (gra.U1[7 + ran_num / 5, 1 + ran_num % 5] == null) ? gra.U2[7 + ran_num / 5, 1 + ran_num % 5] : gra.U1[7 + ran_num / 5, 1 + ran_num % 5];
                if (gra.U2[7 + ran_num / 5, 1 + ran_num % 5] == null)
                { temp2 = gra.U2[7 + ran_num / 5, 1 + ran_num % 5] = new Node(7 + ran_num / 5, 1 + ran_num % 5); }
                else { temp2 = gra.U2[7 + ran_num / 5, 1 + ran_num % 5]; }
                gra.node_to_node(gra.U2[10, 1], temp2);
                gra.node_to_node(temp2, gra.U2[7, 4]);
                gra.node_to_node(temp1, temp2);

                ran_num = gra.ran.xoshiro_range(20);   //  4/4, left
                //temp1 = (gra.U1[5 + ran_num / 4, ran_num % 4] == null) ? gra.U2[5 + ran_num / 4, ran_num % 4] : gra.U1[5 + ran_num / 4, ran_num % 4];
                if (gra.U2[5 + ran_num / 4, ran_num % 4] == null)
                { temp1 = gra.U2[5 + ran_num / 4, ran_num % 4] = new Node(5 + ran_num / 4, ran_num % 4); }
                else { temp1 = gra.U2[5 + ran_num / 4, ran_num % 4]; }
                gra.node_to_node(gra.U2[9, 0], temp1);
                gra.node_to_node(temp1, gra.U2[6, 3]);
                ran_num = gra.ran.xoshiro_range(20);
                //temp1 = (gra.U1[1 + ran_num / 4, ran_num % 4] == null) ? gra.U2[1 + ran_num / 4, ran_num % 4] : gra.U1[1 + ran_num / 4, ran_num % 4];
                if (gra.U2[1 + ran_num / 4, ran_num % 4] == null)
                { temp2 = gra.U2[1 + ran_num / 4, ran_num % 4] = new Node(1 + ran_num / 4, ran_num % 4); }
                else { temp2 = gra.U2[1 + ran_num / 4, ran_num % 4]; }
                gra.node_to_node(gra.U2[1, 0], temp2);
                gra.node_to_node(temp2, gra.U2[4, 3]);
                gra.node_to_node(temp1, temp2);
            }
            catch (Exception e) {
                Console.WriteLine("error in skeletal line generate "+ran_num+", check the random num and shape of graph");
                Console.WriteLine(e);
            }

            //other nodes connect
            for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    if (!(i>=4 & i<=6 & j>=4 & j<=6) & gra.U2[i, j] == null) {
                        unconnected.Add(new int[] { i, j });
                    }
                }
            }
            int[] directions = new int[] { 0, 0, 1, -1};
            bool meet_the_U2;
            while (unconnected.Count > 0) {
                ran_num = gra.ran.xoshiro_range(unconnected.Count);
                temp_route.Clear();
                temp_route.Add(unconnected[ran_num]);
                unconnected.RemoveAt(ran_num);
                route_counter = 0;
                meet_the_U2 = false;
                while (true) {
                    for (int i = 0; i < 4; i++) {

                        if (!(temp_route[route_counter][0] + directions[i] >= 4 & temp_route[route_counter][0] + directions[i] <= 6 & temp_route[route_counter][1] + directions[3-i] >= 4 & temp_route[route_counter][1] + directions[3-i] <= 6) &
                        !temp_route.Contains(new int[] { temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i] }) &
                        temp_route[route_counter][0] + directions[i] >= 0 & temp_route[route_counter][0] + directions[i] <= 10 & temp_route[route_counter][1] + directions[3-i] >= 0 & temp_route[route_counter][1] + directions[3-i] <= 10) {
                            if (gra.U2[temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i]] == null) {
                                ran_num = gra.ran.xoshiro_range(temp_route.Count - route_counter - 1);
                                temp_route.Insert(route_counter + ran_num + 1, new int[] { temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i] });
                                gra.generator_connect(temp_route[route_counter][0], temp_route[route_counter][1], temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i]);
                                gra.U2[temp_route[route_counter][0], temp_route[route_counter][1]].visited = false;
                                gra.U2[temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i]].visited = false;
                                unconnected.Remove(new int[] { temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i] });
                            } else if (gra.U2[temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i]].visited) {
                                gra.generator_connect(temp_route[route_counter][0], temp_route[route_counter][1], temp_route[route_counter][0] + directions[i], temp_route[route_counter][1] + directions[3-i]);
                                meet_the_U2 = true;
                                break;
                            }
                        }
                    }
                    if (meet_the_U2) {
                        foreach (int[] i in temp_route) { gra.U2[i[0], i[1]].visited = true; }
                        if (gra.ran.xoshiro_range(2) == 0) { for (int i = 0; i < 4; i++) { directions[i] *= -1; } }
                        else { ran_num = directions[0]; directions[0] = directions[2]; directions[2] = ran_num; ran_num = directions[1]; directions[1] = directions[3]; directions[3] = ran_num; }
                        break; 
                    }
                    route_counter++;
                }
            }


            gra.graph_render();

        }

        public void generator_connect(int x1, int y1, int x2, int y2) {
            /*if (U1[x1, y1] != null & U2[x1, y1] == null) { U2[x1, y1] = U1[x1, y1]; U1[x1, y1] = null; }
            if (U1[x2, y2] != null & U2[x2, y2] == null) { U2[x2, y2] = U1[x2, y2]; U1[x2, y2] = null; }*/
            if (U2[x1, y1] == null) { U2[x1, y1] = new Node(x1, y1); }
            if (U2[x2, y2] == null) { U2[x2, y2] = new Node(x2, y2); }

            int temp_dir = x1 - x2 != 0 ? (1 - x1 + x2) : (2 + y1 - y2);
            if (this.U2[x1, y1].link[temp_dir] != this.U2[x2, y2]) {
                this.U2[x1, y1].connect(this.U2[x2, y2], temp_dir);
                this.U2[x1, y1].visited = true;
                this.U2[x2, y2].visited = true;
            }

            this.edge_num++;
        }

        public void node_to_node(Node start, Node end)
        {
            if (start == end) { return; }

            Node cur = start;
            int x_change = (start.coor[0] - end.coor[0] > 0) ? -1 : 1;
            int y_change = (start.coor[1] - end.coor[1] > 0) ? -1 : 1;
            int x_gap = Math.Abs(start.coor[0] - end.coor[0]);
            int y_gap = Math.Abs(start.coor[1] - end.coor[1]);
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
                    if (this.ran.xoshiro_range(2) == 0) { this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0] + x_change, cur.coor[1]); cur = cur.link[1 + x_change]; x_gap--; }
                    else { this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0], cur.coor[1] + y_change); cur = cur.link[2 - y_change]; y_gap--; }
                }
            }
        }

        private void graph_render() {
            string[] graph = new string[21];

            for (int i = 0; i < 21; i += 1) {
                for (int j = 0; j < 11; j++) {
                    if (i % 2 == 0)
                    {
                        if (U2[i / 2, j] != null)
                        {
                            graph[i] += "○";
                            if (U2[i / 2, j].link[1] != null) { graph[i] += "-"; } else if (j < 10) { graph[i] += " "; }
                        }
                        else { graph[i] += "■ "; }
                    }
                    else {

                        if (U2[i / 2, j] != null)
                        {
                            if (U2[i / 2, j].link[2] != null)
                            {
                                graph[i] += "|  ";
                            }
                            else if (i < 20) { graph[i] += "   "; }
                        }
                        else { graph[i] += "   "; }
                    }
                }
            }

            for (int i = 0; i < 21; i++) { Console.WriteLine(graph[i]); }
        }
    }
}
