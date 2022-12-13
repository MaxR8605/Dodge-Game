using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dodge_Game
{
    public partial class DodgeGame : Form
    {
        Rectangle character = new Rectangle(0, 0, 30, 30);

        List<Rectangle> obstacles = new List<Rectangle>();
        List<int> type = new List<int>();
        List<int> speed = new List<int>();
        List<int> size = new List<int>();

        bool upDown = false;
        bool downDown = false;
        bool leftDown = false;
        bool rightDown = false;
        bool moveable = false;

        int score = 0;

        SolidBrush orange = new SolidBrush(Color.Orange);
        Pen yellow = new Pen(Color.Yellow, 10);
        SolidBrush red = new SolidBrush(Color.Red);
        SolidBrush characterColour = new SolidBrush(Color.Blue);

        Random random = new Random();
        int randValue = 0;

        public DodgeGame()
        {
            InitializeComponent();
            GotoStart();
        }

        private void DodgeGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (moveable == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        upDown = true;
                        break;
                    case Keys.Down:
                        downDown = true;
                        break;
                    case Keys.Left:
                        leftDown = true;
                        break;
                    case Keys.Right:
                        rightDown = true;
                        break;
                }
            }
        }

        private void DodgeGame_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // move player

            if (upDown == true && character.Y > 0)
            {
                character.Y -= 5;
            }
            if (downDown == true && character.Y < this.Height - character.Height)
            {
                character.Y += 5;
            }
            if (leftDown == true && character.X > 0)
            {
                character.X -= 5;
            }
            if (rightDown == true)
            {
                character.X += 5;
                
                if (character.X > this.Width)
                {
                    character.X = -character.Width;
                    score++;
                    scoreLabel.Text = $"Score:\n  {score}";
                }
            }

            // move ball objects

            for (int i = 0; i < obstacles.Count(); i++)
            {
                int y = obstacles[i].Y + speed[i];
                if (type[i] == 3)
                {
                    obstacles[i] = new Rectangle(obstacles[i].X + random.Next(-3, 4), y, size[i], size[i]);
                }
                else
                {
                    obstacles[i] = new Rectangle(obstacles[i].X, y, size[i], size[i]);
                }
            }

            // generate a random value

            randValue = random.Next(1, 101);

            // generate an obstacle if it is time
            if (randValue < 26)
            {
                randValue = random.Next(10, 71);
                size.Add(randValue);
                obstacles.Add(new Rectangle(random.Next(60, this.Width - 60 - randValue), 0, randValue, randValue));
                speed.Add(random.Next(3, 6));

                randValue = random.Next(1, 101);
                if (randValue < 67)
                {
                    type.Add(random.Next(1, 3));
                }
                else
                {
                    type.Add(random.Next(3, 7));
                }
            }

            // remove obstacle if it goes off the screen

            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].Y > this.Height)
                {
                    obstacles.RemoveAt(i);
                    size.RemoveAt(i);
                    speed.RemoveAt(i);
                    type.RemoveAt(i);

                    moveable = true;
                }

                if (character.IntersectsWith(obstacles[i]))
                {
                    GotoStart();

                    obstacles.RemoveAt(i);
                    size.RemoveAt(i);
                    speed.RemoveAt(i);
                    type.RemoveAt(i);
                }
            }

            Refresh();
        }

        private void DodgeGame_Paint(object sender, PaintEventArgs e)
        {
            //draw character 
            e.Graphics.FillRectangle(characterColour, character);

            //draw obstacles 
            for (int i = 0; i < obstacles.Count(); i++)
            {
                switch (type[i])
                {
                    case 1:
                        e.Graphics.FillRectangle(orange, obstacles[i].X, obstacles[i].Y, size[i], size[i]);
                        break;
                    case 2:
                        e.Graphics.FillEllipse(orange, obstacles[i].X, obstacles[i].Y, size[i], size[i]);
                        break;
                    case 3:
                        e.Graphics.FillRectangle(red, obstacles[i].X, obstacles[i].Y, size[i], size[i]);
                        break;
                    case 4:
                        e.Graphics.FillPie(red, obstacles[i].X, obstacles[i].Y, size[i], size[i], speed[i] * 20, speed[i] * size[i]);
                        break;
                    case 5:
                        e.Graphics.DrawRectangle(yellow, obstacles[i].X, obstacles[i].Y, size[i], size[i]);
                        break;
                    case 6:
                        e.Graphics.DrawEllipse(yellow, obstacles[i].X, obstacles[i].Y, size[i], size[i]);
                        break;
                }
            }
        }
        public void GotoStart()
        {
            character.X = 30;
            character.Y = this.Height / 2 - character.Height / 2;
        }
    }
}
