using System;

namespace MapExtractor
{
    class Node
    {
        public int X, Y;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
