using Collect_game.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collect_game
{
    public partial class Form1 : Form
    {
        List<ball> balls;
        Image paddleimage, ballimage;
        paddle paddle;
        System.Windows.Forms.Timer tmr;
        long counter;
        int xs = 0;
        int basketballs, footballs, volleyballs;
        Random r;
        public Form1()
        { 
            InitializeComponent();
            paddle = new paddle();
            r = new Random();
            balls = new List<ball>();
            footballs = 0;
            basketballs = 0;
            volleyballs = 0;
            /*
             * Types:
             * 0: Bomb
             * 1: Football
             * 2: Basketball
             * 3: Volleyball
             * 4: Smilingball
             */
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            highScore.Text = (string)Properties.Settings.Default.Highscore;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            tmr = new System.Windows.Forms.Timer();
            counter = 0;
            tmr.Interval = 25;
            tmr.Tick += new EventHandler(tmr_Tick);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            OnKeyDown(new KeyEventArgs(keyData));
            return base.ProcessDialogKey(keyData);
        }


        private void Form1_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                paddle.MoveLeft();
            }
            else if (e.KeyCode == Keys.Right)
            {
                paddle.MoveRight();
            }
        }
        void tmr_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count();i++ )
            {
                balls[i].Update();
                if (balls[i].touched==true)
                {
                    int bt = balls[i].GetType();
                    balls.Remove(balls[i]);
                    switch (bt)
                    {
                        case 0:
                            xs += 1;
                            if(xs>=1){
                                x1.Image = Resources.x;
                            }
                            if (xs >= 2)
                            {
                                x2.Image = Resources.x;
                            }
                            if (xs >= 3)
                            {
                                x3.Image = Resources.x;
                                GameOver();
                            }
                            break;
                        case 1:
                            footballs += 1;
                            footballCounter.Text = footballs.ToString();
                            break;
                        case 2:
                            basketballs += 1;
                            basketballCounter.Text = basketballs.ToString();
                            break;
                        case 3:
                            volleyballs += 1;
                            volleyballCounter.Text = volleyballs.ToString();
                            break;
                        case 4:
                            if(xs>0){xs -= 1;}
                            if(xs<1){
                                x1.Image = null;
                            }
                            if (xs < 2)
                            {
                                x2.Image = null;
                            }
                            if (xs < 3)
                            {
                                x3.Image = null;
                            }
                            break;
                    }
                }
                if (balls[i].GetY() > 412)
                {
                    int t = balls[i].GetType();
                    balls.Remove(balls[i]);
                    if (t != 0 && t != 4) {
                        xs += 1;
                        if (xs >= 1)
                        {
                            x1.Image = Resources.x;
                        }
                        if (xs >= 2)
                        {
                            x2.Image = Resources.x;
                        }
                        if (xs >= 3)
                        {
                            x3.Image = Resources.x;
                            GameOver();
                        }
                    }
                }
            }
            counter +=25;
            if(counter % 1050 == 0 && balls.Count()< 3){
                counter = 0;
                balls.Add(new ball(r.Next(245), paddle, r.Next(0, 5)));
            }
            this.Refresh();
        }

        public void GameOver() {
            tmr.Stop();
            MessageBox.Show("Game over! your score is " + (basketballs + volleyballs + footballs));
            int highscore = Convert.ToInt32(Properties.Settings.Default.Highscore);
            if ((basketballs + volleyballs + footballs) > highscore) {
                Properties.Settings.Default.Highscore = (basketballs + volleyballs + footballs).ToString();
                highScore.Text = (string)Properties.Settings.Default.Highscore;
                Properties.Settings.Default.Save();
            }
            basketballs = 0;
            volleyballs = 0;
            footballs = 0;
            xs = 0;
            x1.Image = null;
            x2.Image = null;
            x3.Image = null;
            basketballCounter.Text= "0";
            footballCounter.Text = "0";
            volleyballCounter.Text = "0";
            tmr.Start();
        }
            
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            
            paddleimage = Resources.paddle;
            ballimage = Resources.football;
            try
            {
                e.Graphics.DrawImage(paddleimage, paddle.GetX(), 400, 48, 12);
                foreach (var b in balls)
                {
                    e.Graphics.DrawImage(GetImage(b.GetType()), b.GetX(), b.GetY(), 48, b.GetHeight());
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        private Image GetImage(int type)
        {
            Image img;
            switch (type)
            {
                case 0:
                    img = Resources.bomb;
                    break;
                case 1:
                    img = Resources.football;
                    break;
                case 2:
                    img = Resources.basketball;
                    break;
                case 3:
                    img = Resources.volleyball;
                    break;
                case 4:
                    img = Resources.smilingball;
                    break;
                default:
                    img = Resources.football;
                    break;
            }
            return img;
        }

        private void resetHighscore_Click(object sender, EventArgs e)
        {
            try {

                Properties.Settings.Default.Highscore = "0";
                highScore.Text = (string)Properties.Settings.Default.Highscore;
                Properties.Settings.Default.Save();
            }
            catch(Exception ex) { }
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            if (tmr.Enabled == false)
            {
                tmr.Start();
            }
            button_play.Enabled = false;
        }

        private void button_instructions_Click(object sender, EventArgs e)
        {
            tmr.Stop();
            button_play.Enabled = true;
            Form2 f = new Form2();
            f.Show();
        }

        private void button_about_Click(object sender, EventArgs e)
        {
            tmr.Stop();
            button_play.Enabled = true;
            Form3 f = new Form3();
            f.Show();
        }
    }
}
