using System;
using System.Collections.Generic;
using System.Text;

namespace temptemptemp_CS
{
    class graph_test
    {
        Node[,] U1;  //UnConnected
        Node[,] U2;  //Connected
        random_test ran;
        int edge_num;

        public graph_test() {
            U1 = new Node[11, 11];
            U2 = new Node[11, 11];
            for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    U1[i, j] = null;
                    U2[i, j] = null;
                }
            }
            ran = new random_test();

            edge_num = 0;
        }

        static void Main() {
            graph_test gra = new graph_test();

            Node temp1 = new Node(); Node temp2 = new Node(); Node temp3 = new Node(); Node temp4 = new Node();
            List<Node> T = new List<Node>();
            int temp_int = 0;

            //Preparation
            for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    if (i < 4 | i > 6 | j < 4 | j > 6)
                    {
                        gra.U1[i, j] = new Node(i, j);
                    }
                }
            }
            gra.generator_connect(0, 0, 0, 1); gra.generator_connect(0, 0, 1, 0);
            gra.generator_connect(0, 10, 0, 9); gra.generator_connect(0, 10, 1, 10);
            gra.generator_connect(10, 0, 10, 1); gra.generator_connect(10, 0, 9, 0);
            gra.generator_connect(10, 10, 10, 9); gra.generator_connect(10, 10, 9, 10);
            //abvoe_outline start node connect, below_middle elite node connect
            gra.ran.seed();
            temp_int = gra.ran.xoshiro_range(4);
            if (temp_int != 0) { gra.U2[4, 4] = new Node(4, 4); gra.generator_connect(4, 4, 3, 4); gra.generator_connect(4, 4, 4, 3); }
            else { gra.generator_connect(3, 3, 3, 4); gra.generator_connect(3, 3, 4, 3); }
            if (temp_int != 1) { gra.U2[4, 6] = new Node(4, 6); gra.generator_connect(4, 6, 3, 6); gra.generator_connect(4, 6, 4, 7); }
            else { gra.generator_connect(3, 7, 3,6); gra.generator_connect(3, 7, 4, 7); }
            if (temp_int != 2) { gra.U2[6, 4] = new Node(6, 4); gra.generator_connect(6, 4, 7, 4); gra.generator_connect(6, 4, 6, 3); }
            else { gra.generator_connect(7, 3, 7, 4); gra.generator_connect(7, 3, 6, 3); }
            if (temp_int != 3) { gra.U2[6, 6] = new Node(4, 6); gra.generator_connect(6, 6, 7, 6); gra.generator_connect(6, 6, 6, 7); }
            else { gra.generator_connect(7, 7, 7, 6); gra.generator_connect(7, 7, 6, 7); }

            //skeletal line generate
            temp_int = gra.ran.xoshiro_range(21);   //  1/8, betwwen 12 oclock~1 oclock, clockwise
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[temp_int / 5, 5 + temp_int % 5] == null) ? gra.U2[temp_int / 5, 5 + temp_int % 5] : gra.U1[temp_int / 5, 5 + temp_int % 5];
            gra.node_to_node(gra.U2[0, 9], temp1);
            gra.node_to_node(temp1, gra.U2[3, 6]);
            temp_int = gra.ran.xoshiro_range(21);   //  2/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[1 + temp_int / 4, 7 + temp_int % 4] == null) ? gra.U2[1 + temp_int / 4, 7 + temp_int % 4] : gra.U1[1 + temp_int / 4, 7 + temp_int % 4];
            gra.node_to_node(gra.U2[1, 10], temp1);
            gra.node_to_node(temp1, gra.U2[4, 7]);
            temp_int = gra.ran.xoshiro_range(21);   //  3/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[6 + temp_int / 4, 7 + temp_int % 4] == null) ? gra.U2[ 6+ temp_int / 4, 7 + temp_int % 4] : gra.U1[6 + temp_int / 4, 7 + temp_int % 4];
            gra.node_to_node(gra.U2[9, 10], temp1);
            gra.node_to_node(temp1, gra.U2[6, 7]);
            temp_int = gra.ran.xoshiro_range(21);   //  4/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[7 + temp_int / 5, 6 + temp_int % 5] == null) ? gra.U2[7 + temp_int / 5, 6 + temp_int % 5] : gra.U1[7 + temp_int / 5, 6 + temp_int % 5];
            gra.node_to_node(gra.U2[10, 9], temp1);
            gra.node_to_node(temp1, gra.U2[7, 6]);
            temp_int = gra.ran.xoshiro_range(21);   //  5/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[6 + temp_int / 5, 1 + temp_int % 5] == null) ? gra.U2[6 + temp_int / 5, 1 + temp_int % 5] : gra.U1[6 + temp_int / 5, 1 + temp_int % 5];
            gra.node_to_node(gra.U2[10, 1], temp1);
            gra.node_to_node(temp1, gra.U2[7, 4]);
            temp_int = gra.ran.xoshiro_range(21);   //  6/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[5 + temp_int / 4, temp_int % 4] == null) ? gra.U2[5 + temp_int / 4, temp_int % 4] : gra.U1[5 + temp_int / 4, temp_int % 4];
            gra.node_to_node(gra.U2[9, 0], temp1);
            gra.node_to_node(temp1, gra.U2[6, 3]);
            temp_int = gra.ran.xoshiro_range(21);   //  7/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[1 + temp_int / 4, temp_int % 4] == null) ? gra.U2[1 + temp_int / 4, temp_int % 1] : gra.U1[1 + temp_int / 4, temp_int % 4];
            gra.node_to_node(gra.U2[1, 0], temp1);
            gra.node_to_node(temp1, gra.U2[4, 3]);
            temp_int = gra.ran.xoshiro_range(21);   //  8/8
            Console.WriteLine(temp_int);
            temp1 = (gra.U1[temp_int / 5, 1 + temp_int % 5] == null) ? gra.U2[temp_int / 5, 1 + temp_int % 5] : gra.U1[temp_int / 5, 1 + temp_int % 5];
            gra.node_to_node(gra.U2[0, 1], temp1);
            gra.node_to_node(temp1, gra.U2[3, 4]);

            //other nodes connect



            gra.graph_render();

        }

        public void generator_connect(int x1, int y1, int x2, int y2) {
            if (U1[x1, y1] != null & U2[x1, y1] == null) { U2[x1, y1] = U1[x1, y1]; U1[x1, y1] = null; }
            if (U1[x2, y2] != null & U2[x2, y2] == null) { U2[x2, y2] = U1[x2, y2]; U1[x2, y2] = null; }

            int temp_dir = x1 - x2 != 0 ? (1 - x1 + x2) : (2 + y1 - y2);
            if (this.U2[x1, y1].link[temp_dir] != this.U2[x2, y2]) {
                this.U2[x1, y1].connect(this.U2[x2, y2], temp_dir);
            }

            this.edge_num++;
        }

        public void node_to_node(Node start, Node end) {
            if (start == end) { return; }

            Node cur = start;
            int x_change = (start.coor[0] - end.coor[0] > 0) ? -1 : 1;
            int y_change = (start.coor[1] - end.coor[1] > 0) ? -1 : 1;
            int x_gap = Math.Abs(start.coor[0] - end.coor[0]);
            int y_gap = Math.Abs(start.coor[1] - end.coor[1]);
            for (int i = x_gap+y_gap; i > 0; i--) {
                if (x_gap != 0 & y_gap == 0) {
                    this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0] + x_change, cur.coor[1]); cur = cur.link[1 + x_change]; x_gap--;
                } else if (x_gap == 0 & y_gap != 0) {
                    this.generator_connect(cur.coor[0], cur.coor[1], cur.coor[0], cur.coor[1] + y_change); cur = cur.link[2 - y_change]; y_gap--;
                }
                else {
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
