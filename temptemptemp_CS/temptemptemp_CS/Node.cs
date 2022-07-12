using System;
using System.Collections.Generic;
using System.Text;

namespace temptemptemp_CS
{
    class Node
    {
        public int[] coor = new int[2];
        public Node[] link = new Node[4];  //0 up, 1 right, 2 down, 3 left
        public bool visited;

        public Node(int x, int y) : this()
        {
            this.coor[0] = x;
            this.coor[1] = y;
        }
        public Node() {
            for (int i = 0; i < 4; i++) { this.link[i] = null;   /*-1 means it has no link*/}
            this.visited = false;
        }

        public void connect(Node n, int dir) {
            if (n == null) { Console.WriteLine("from ("+this.coor[0]+","+this.coor[1]+") direction "+dir+" null node connecting error"); return; }
            this.link[dir] = n;
            n.link[dir < 2 ? dir + 2 : (dir + 2) % 4] = this;
        }

        public void existence_test() {
            Console.WriteLine("cordinate of this is : "+this.coor[0]+", "+this.coor[1]);
        }
    }
}
