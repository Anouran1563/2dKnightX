using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2dFightX
{
    public partial class Form1 : Form
    {
        private Rectangle pictureBox1OriginalRectangle;
        private Rectangle pictureBox2OriginalRectangle;
        private Rectangle pictureBox3OriginalRectangle;
        private Rectangle pictureBox4OriginalRectangle;
        private Rectangle pictureBox5OriginalRectangle;
        private Rectangle pictureBox8OriginalRectangle;
        private Rectangle pbSwordOriginalRectangle;
        private Rectangle pbPlayerOriginalRectangle;
        private Rectangle pbBabuOriginalRectangle;
        private Rectangle pictureBox6OriginalRectangle;
        private Rectangle pictureBox9OriginalRectangle;
        private Rectangle pictureBox11OriginalRectangle;
        private Rectangle pictureBox12OriginalRectangle;
        private Rectangle picturebox13OriginalRectangle;
        private Rectangle picturebox43OriginalRectangle;
        private Rectangle picturebox44OriginalRectangle;
        private Rectangle picturebox7OriginalRectangle;
        private Rectangle hp3OriginalRectangle;
        private Rectangle hp2OriginalRectangle;
        private Rectangle hp1OriginalRectangle;
        private Size originalformsize;

        int PlayerX = 2;
        int PlayerY = 357;
        int PlayerHp = 3;
        int playerJump;
        int jumpHeight = 50;
        int originalPlayerY;

        int BabuX = 450;
        int BabuHp;

        bool left, right;
        bool jump;
        bool directionPressed;
        bool playingAction;
        bool hasSword;
        bool jumping = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void PhealthIndex() //Controls for the Health
        {
            if (PlayerHp == 2)
            {
                Hp1.Visible = false;
                pbPlayer.Image = Properties.Resources.hurt;
            }
            if (PlayerHp == 1)
            {
                Hp2.Visible = false;
                pbPlayer.Image = Properties.Resources.hurt;
            }
            if (PlayerHp == 0)
            {
                Hp3.Visible = false;
                EndForm();
            }
        }
        private void BhealthIndex() //Controls for the Health
        {
            if (BabuHp == 1)
            {
                pbBabu.Image = Properties.Resources.Babu_hurt;
            }
            if (BabuHp == 2)
            {
                pbBabu.Image = Properties.Resources.Babu_Dead;
            }
            if (BabuHp == 3)
            {
                EndForm();
            }
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && !directionPressed)
            {

                right = false;
                left = true;
                MovePlayerAnimation("left");
                directionPressed = true;
            }
            else if (e.KeyCode == Keys.D && !directionPressed)
            {
                left = false;
                right = true;
                MovePlayerAnimation("right");
                directionPressed = true;
            }

            if (e.KeyCode == Keys.W)
            {
                jump = true;
                MovePlayerAnimation("jump");
            }
        }

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                left = false;
                right = false;
                pbPlayer.Image = Properties.Resources.idle;
                directionPressed = false;
            }

            if (e.KeyCode == Keys.W)
            {
                jump = false;
                pbPlayer.Image = Properties.Resources.idle;
            }

            if (e.KeyCode == Keys.E && !playingAction && hasSword == true)
            {
                SetPlayerAction("hit");
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            ImageAnimator.UpdateFrames();
            MovePlayerBackground();
            PhealthIndex();
            BhealthIndex();
            pbPlayer.Top += playerJump;
            originalPlayerY = pbPlayer.Top;
            pbBabu.Left = BabuX;

            if (left == true && pbPlayer.Left > 0)
            {
                PlayerX--;
                pbPlayer.Left = PlayerX;
            }

            if (right == true && pbPlayer.Left + (pbPlayer.Width + 60) < this.ClientSize.Width)
            {
                PlayerX++;
                pbPlayer.Left = PlayerX;
            }

            if (jump)
            {
                if (!jumping)
                {
                    playerJump = -10;
                    jumping = true;
                }

                PlayerY += playerJump;

                if (PlayerY <= originalPlayerY - jumpHeight)
                {
                    jumping = false;
                }
            }
            else
            {
                playerJump = +20;
                PlayerY += playerJump;

                if (PlayerY >= originalPlayerY)
                {
                    PlayerY = originalPlayerY;
                    jumping = false;
                }
            }

            BabuX++;
            if (pbBabu.Bounds.IntersectsWith(dotRight.Bounds))
            {
                pbBabu.Image = Properties.Resources.babu_left;
                BabuX--;
            }
            else if (pbBabu.Bounds.IntersectsWith(dotLeft.Bounds))
            {
                pbBabu.Image = Properties.Resources.babu_Right;
                BabuX++;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform")
                {
                    if (pbPlayer.Bounds.IntersectsWith(x.Bounds) && jump == false)
                    {
                        pbPlayer.Top = x.Top - pbPlayer.Height;
                        playerJump = 0;
                    }
                    x.BringToFront();

                    {
                        if (x is PictureBox && (string)x.Tag == "fire")
                        {
                            if (pbPlayer.Bounds.IntersectsWith(x.Bounds))
                            {
                                PlayerHp--;
                                Console.WriteLine("After Collision - PlayerHp" + PlayerHp);
                                if (PlayerHp == 0)
                                {
                                    MessageBox.Show("Mission failed" + Environment.NewLine + "Click ok to try again");
                                    pbPlayer.Image = Properties.Resources.dead;
                                    return;
                                }
                            }
                        }
                        if (x is PictureBox && x == pbBabu && hasSword)
                        {
                            if (pbPlayer.Bounds.IntersectsWith(x.Bounds))
                            {
                                PlayerHp--;
                                Console.WriteLine("after collision - playerhp" + PlayerHp);
                                if (PlayerHp == 0)
                                {
                                    MessageBox.Show("mission failed" + Environment.NewLine + "click ok to try again");
                                    pbPlayer.Image = Properties.Resources.dead;
                                }
                            }
                        }

                    }
                }
            }
            if (pbPlayer.Bounds.IntersectsWith(pbSword.Bounds))
            {
                hasSword = true;
                pbSword.Visible = false;
            }

            if (BabuHp == 3)
            {
                pbBabu.Image = Properties.Resources.Babu_Dead;
                GameTimer.Stop();
                EndForm();
            }
        }
        private void MovePlayerBackground()
        {
            if (left)
            {
                if (PlayerX > 0)
                {
                    PlayerX -= 6;
                }

                //if (BgPosition < 0 && PlayerX < 200)
                //{
                //    BgPosition += 5;
                //}
            }

            if (right)
            {
                if (PlayerX + pbPlayer.Width < this.ClientSize.Width)
                {
                    PlayerX += 6;
                }
            }
        }


        private void MovePlayerAnimation(string direction)
        {
            if (direction == "left")
            {
                left = true;
                pbPlayer.Image = Properties.Resources.goLeft;
            }

            if (direction == "right")
            {
                right = true;
                pbPlayer.Image = Properties.Resources.goRight;
            }

            if (direction == "jump")
            {
                jump = true;
                pbPlayer.Image = Properties.Resources.jump;
            }

            directionPressed = true;
            playingAction = false;
        }



        private void SetPlayerAction(string animation)
        {
            pbPlayer.Image = Properties.Resources.hit;

            if (pbPlayer.Bounds.IntersectsWith(pbBabu.Bounds))
            {
                BabuHp += 1;
            }

            playingAction = true;
        }

        private void EndForm()
        {
            GameTimer.Stop();
            if (PlayerHp == 0)
            {
                pbPlayer.Image = Properties.Resources.dead;
                MessageBox.Show("You fought well but your time has come");
            }
            else if (BabuHp == 3)
            {
                pbBabu.Visible = false;
                MessageBox.Show("You saved us all. We all are proud of you");
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            originalformsize = this.Size;
            pictureBox1OriginalRectangle = new Rectangle(pictureBox1.Location.X, pictureBox1.Location.Y, pictureBox1.Width, pictureBox1.Height);
            pictureBox2OriginalRectangle = new Rectangle(pictureBox2.Location.X, pictureBox2.Location.Y, pictureBox2.Width, pictureBox2.Height); ;
            pictureBox3OriginalRectangle = new Rectangle(pictureBox3.Location.X, pictureBox3.Location.Y, pictureBox3.Width, pictureBox3.Height);
            pictureBox4OriginalRectangle = new Rectangle(pictureBox4.Location.X, pictureBox4.Location.Y, pictureBox4.Width, pictureBox4.Height); ;
            pictureBox5OriginalRectangle = new Rectangle(pictureBox5.Location.X, pictureBox5.Location.Y, pictureBox5.Width, pictureBox5.Height);
            pictureBox8OriginalRectangle = new Rectangle(pictureBox8.Location.X, pictureBox8.Location.Y, pictureBox8.Width, pictureBox8.Height);
            pbSwordOriginalRectangle = new Rectangle(pbSword.Location.X, pbSword.Location.Y, pbSword.Width, pbSword.Height);
            pbPlayerOriginalRectangle = new Rectangle(pbPlayer.Location.X, pbPlayer.Location.Y, pbPlayer.Width, pbPlayer.Height);
            pbBabuOriginalRectangle = new Rectangle(pbBabu.Location.X, pbBabu.Location.Y, pbBabu.Width, pbBabu.Height);
            pictureBox6OriginalRectangle = new Rectangle(pictureBox6.Location.X, pictureBox6.Location.Y, pictureBox6.Width, pictureBox6.Height);
            pictureBox9OriginalRectangle = new Rectangle(pictureBox9.Location.X, pictureBox9.Location.Y, pictureBox9.Width, pictureBox9.Height);
            pictureBox11OriginalRectangle = new Rectangle(pictureBox11.Location.X, pictureBox11.Location.Y, pictureBox11.Width, pictureBox11.Height);
            pictureBox12OriginalRectangle = new Rectangle(dotLeft.Location.X, dotLeft.Location.Y, dotLeft.Width, dotLeft.Height);
            picturebox13OriginalRectangle = new Rectangle(pictureBox12.Location.X, pictureBox12.Location.Y, pictureBox12.Width, pictureBox12.Height);
            picturebox44OriginalRectangle = new Rectangle(pictureBox44.Location.X, pictureBox44.Location.Y, pictureBox44.Width, pictureBox44.Height);
            picturebox7OriginalRectangle = new Rectangle(pictureBox7.Location.X, pictureBox7.Location.Y, pictureBox43.Width, pictureBox43.Height);
            hp3OriginalRectangle = new Rectangle(Hp3.Location.X, Hp3.Location.Y, Hp3.Width, Hp3.Height);
            hp2OriginalRectangle = new Rectangle(Hp2.Location.X, Hp2.Location.Y, Hp2.Width, Hp2.Height);
            hp1OriginalRectangle = new Rectangle(Hp1.Location.X, Hp1.Location.Y, Hp1.Width, Hp1.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeCtrl(pictureBox1OriginalRectangle, pictureBox1);
            ResizeCtrl(pictureBox2OriginalRectangle, pictureBox2);
            ResizeCtrl(pictureBox3OriginalRectangle, pictureBox3);
            ResizeCtrl(pictureBox4OriginalRectangle, pictureBox4);
            ResizeCtrl(pictureBox5OriginalRectangle, pictureBox5);
            ResizeCtrl(pictureBox8OriginalRectangle, pictureBox8);
            ResizeCtrl(pbSwordOriginalRectangle, pbSword);
            ResizeCtrl(pbPlayerOriginalRectangle, pbPlayer);
            ResizeCtrl(pbBabuOriginalRectangle, pbBabu);
            ResizeCtrl(pictureBox6OriginalRectangle, pictureBox6);
            ResizeCtrl(pictureBox9OriginalRectangle, pictureBox9);
            ResizeCtrl(pictureBox11OriginalRectangle, pictureBox11);
            ResizeCtrl(pictureBox12OriginalRectangle, dotLeft);
            ResizeCtrl(picturebox13OriginalRectangle, pictureBox12);
            ResizeCtrl(picturebox43OriginalRectangle, pictureBox43);
            ResizeCtrl(picturebox44OriginalRectangle, pictureBox44);
            ResizeCtrl(picturebox7OriginalRectangle, pictureBox7);
            ResizeCtrl(hp3OriginalRectangle, Hp3);
            ResizeCtrl(hp2OriginalRectangle, Hp2);
            ResizeCtrl(hp1OriginalRectangle, Hp1);
        }

        private void ResizeCtrl(Rectangle OriginalControlRect, Control control)
        {
            float xAxis = (float)(this.Width) / (float)(originalformsize.Width);
            float yAxis = (float)(this.Height) / (float)(originalformsize.Height);

            int newXPosition = (int)(OriginalControlRect.X * xAxis);
            int newYPosition = (int)(OriginalControlRect.Y * yAxis);

            int newWidth = (int)(OriginalControlRect.Width * xAxis);
            int newHight = (int)(OriginalControlRect.Height * yAxis);

            control.Location = new Point(newXPosition, newYPosition);
            control.Size = new Size(newWidth, newHight);
        }
    }
}
