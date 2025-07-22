using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake=new List<Circle>();
        private Circle food=new Circle();

        private List<Circle> walls = new List<Circle>();

        int maxWidth, maxHeight, score;
        Random rand = new Random();
        bool goLeft, goRight, goUp, goDown;

        public Form1()
        {
            InitializeComponent();
            new Settings();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left && Settings.Directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.Directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.Directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.Directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (goRight)
            {
                Settings.Directions = "right";
            }
            if (goLeft)
            {
                Settings.Directions = "left";
            }
            if (goUp)
            {
                Settings.Directions = "up";
            }
            if (goDown)
            {
                Settings.Directions = "down";
            }

            for (int i = Snake.Count-1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.Directions)
                    {
                        case "left":
                            Snake[i].x--;
                            break;
                        case "right":
                            Snake[i].x++;
                            break;
                        case "up":
                            Snake[i].y--;
                            break;
                        case "down":
                            Snake[i].y++;
                            break;
                    }
                    if(Snake[i].x < 0)
                    {
                        Snake[i].x = maxWidth;
                    }
                    if (Snake[i].x > maxWidth)
                    {
                        Snake[i].x = 0;
                    }
                    if (Snake[i].y < 0)
                    {
                        Snake[i].y = maxHeight;
                    }
                    if (Snake[i].y > maxHeight)
                    {
                        Snake[i].y = 0;
                    }
                    if (Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        EatFood();
                    }
                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            GameOver();
                        }
                    }
                    foreach (var wall in walls)
                    {
                        if (Snake[0].x == wall.x && Snake[0].y == wall.y)
                        {
                            GameOver();
                        }
                    }
                }
                else
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }
            map.Invalidate();
        }

        private void UpdatePictureBox(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColour;
            for(int i = 0;i< Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour=Brushes.Black;
                }
                else
                {
                    snakeColour =Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].x * Settings.Width, Snake[i].y * Settings.Height, Settings.Width, Settings.Height));
            }

            canvas.FillEllipse(Brushes.Red, new Rectangle(food.x * Settings.Width, food.y * Settings.Height, Settings.Width, Settings.Height));
            foreach (var wall in walls)
            {
                canvas.FillRectangle(Brushes.Gray, new Rectangle(wall.x * Settings.Width, wall.y * Settings.Height, Settings.Width, Settings.Height));
            }
        }

        private void RestartGame()
        {
            maxWidth = map.Width / Settings.Width - 1;
            maxHeight=map.Height / Settings.Height - 1;

            Snake.Clear();
            walls.Clear();
            for (int x = 3; x <= 11; x++)
            {
                walls.Add(new Circle { x = x, y = 3 });
            }
            for (int x = 3; x <= 11; x++)
            {
                walls.Add(new Circle { x = x, y = 4 });
            }
            for (int x = 22; x <= 30; x++)
            {
                walls.Add(new Circle { x = x, y = 3 });
            }
            for (int x = 22; x <= 30; x++)
            {
                walls.Add(new Circle { x = x, y = 4 });
            }
            
            for (int x = 3; x <= 11; x++)
            {
                walls.Add(new Circle { x = x, y = 14 });
            }
            for (int x = 3; x <= 11; x++)
            {
                walls.Add(new Circle { x = x, y = 15 });
            }
            for (int x = 22; x <= 30; x++)
            {
                walls.Add(new Circle { x = x, y = 14 });
            }
            for (int x = 22; x <= 30; x++)
            {
                walls.Add(new Circle { x = x, y = 15 });
            }
            startButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;
            gameTimer.Interval = 100;
            Settings.Directions = "left";

            Circle head=new Circle {x=10, y=5 };
            Snake.Add(head);

            for (int i = 0; i < 2; i++)
            {
                Circle body=new Circle();
                Snake.Add(body);
            }
            
            bool validFoodPosition = false;
            while (!validFoodPosition)
            {
                food = new Circle { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };
                bool overlapsSnake = Snake.Any(s => s.x == food.x && s.y == food.y);
                bool overlapsWall = walls.Any(w => w.x == food.x && w.y == food.y);
                if (!overlapsSnake && !overlapsWall)
                {
                    validFoodPosition = true;
                }
            }
  
            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;
            txtScore.Text="Score: "+score;
            if(score >= 10)
            {
                gameTimer.Interval = 90;
            }
            if (score >= 20)
            {
                gameTimer.Interval = 80;
            }
            if (score >= 30)
            {
                gameTimer.Interval = 70;
            }
            if (score >= 40)
            {
                gameTimer.Interval = 60;
            }
            if (score >= 50)
            {
                gameTimer.Interval = 50;
            }

            Circle body=new Circle { x = Snake[Snake.Count-1].x, y=Snake[Snake.Count-1].y};
            Snake.Add(body);
            bool validPosition = false;
            while (!validPosition)
            {
                food = new Circle { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };
                bool overlapsSnake = Snake.Any(s => s.x == food.x && s.y == food.y);
                bool overlapsWall = walls.Any(w => w.x == food.x && w.y == food.y);
                if (!overlapsSnake && !overlapsWall)
                {
                    validPosition = true;
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled=true;
        }
    }
}
