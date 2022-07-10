using System;
using System.Collections.Generic;
using System.Text;

namespace temptemptemp_CS
{
    class graph_test
    {
        Node[,] U1;  //UnConnected
        Node[,] U2;  //Connected
        public graph_test() {
            U1 = new Node[11, 11];
            U2 = new Node[11, 11];
        }

        static void Main() {
            graph_test gra = new graph_test();

            Node temp1 = new Node(); Node temp2 = new Node(); Node temp3 = new Node(); Node temp4 = new Node();
            List<Node> T = new List<Node>();
            int temp_int = 0;

            //Preparation
            for (int i = 0; i < 11; i++) {
                for (int j = 0; j < 11; j++) {
                    if (i < 4 & i > 6 & j < 4 & j > 6)
                    {
                        gra.U1[i, j] = new Node(i, j);
                    }
                    else {
                        gra.U1[i, j] = null;
                    }
                }
            }
            gra.generator_connect(0, 0, 0, 1); gra.generator_connect(0, 0, 1, 0);
            gra.generator_connect(0, 10, 0, 9); gra.generator_connect(0, 10, 1, 10);
            gra.generator_connect(10, 0, 10, 1); gra.generator_connect(10, 0, 9, 0);
            gra.generator_connect(10, 10, 10, 9); gra.generator_connect(10, 10, 9, 10);
            //abvoe_outline start node connect, below_middle elite node connect
            //temp_int = random number among 0~3
            if (temp_int != 0) { gra.U2[4, 4] = new Node(4, 4); gra.generator_connect(4, 4, 3, 4); gra.generator_connect(4, 4, 4, 3); }
            if (temp_int != 1) { gra.U2[4, 6] = new Node(4, 6); gra.generator_connect(4, 6, 3, 6); gra.generator_connect(4, 4, 4, 5); }
            if (temp_int != 2) { gra.U2[6, 4] = new Node(6, 4); gra.generator_connect(6, 4, 5, 4); gra.generator_connect(4, 4, 6, 3); }
            if (temp_int != 3) { gra.U2[6, 6] = new Node(4, 6); gra.generator_connect(6, 6, 5, 6); gra.generator_connect(4, 4, 6, 5); }

        }

        public void generator_connect(int x1, int y1, int x2, int y2) {
            U2[x1, y1].connect(U2[x2, y2], x1 - x2 != 0 ? (3 + x1 - x2) : (1 - y1 + y2));
            if (U1[x1, y1] != null & U2[x1, y1] == null) { U2[x1, y1] = U1[x1, y1]; U1[x1, y1] = null; }
            if (U1[x2, y2] != null & U2[x2, y2] == null) { U2[x2, y2] = U1[x2, y2]; U1[x2, y2] = null; }
        }

        private void graph_render() {
            string[] graph = new string[21];

            for (int i = 0; i < 21; i+=2) {
                for (int j = 0; j < 11; j++) {
                    if (U2[i, j] != null) { graph[i]+="○"; } else { graph[i]+="●"; }
                    if (U2[i, j].link[2] != null) { graph[i + 1] += "|   "; } else if (i < 20) { graph[i + 1] += "    "; }
                    if (U2[i, j].link[1] != null) { graph[i] += "-"; } else if (j < 10) { graph[i] += " "; }
                }
            }
            for (int i = 0; i < 21; i++) { Console.WriteLine(graph[i]); }
        }
    }
}
