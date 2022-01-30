using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collect_game
{
    class ball
    {
        paddle paddle;
        int x, y;
        int type;
        public bool touched = false;
        public ball(int x,paddle paddle,int type)
        {
            this.x = x;
            this.y = -48;
            this.paddle = paddle;
            this.type = type;
        }

        public void Update()
        {
            if ((y <= 347 && this.type != 0) || (y <= 323 && this.type == 0))
            {
                y += 5;
            }
            else
            {
                if (this.x >= (paddle.GetX() - 48) && this.x <= (paddle.GetX() + 48) && this.y < 353) { y += 0; touched = true; }
                else if(y <= 412){ y += 5; }
            }

        }
        public int GetHeight()
        {
            if (type == 0)
            {
                return 72;
            }
            else
            {
                return 48;
            }
        }
        public int GetX() { return x; }
        public int GetY() { return y; }
        public int GetType() { return type; }
    }
}
