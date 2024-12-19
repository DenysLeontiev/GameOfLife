using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;

        private GameEngine gameEngine;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            nudDensity.Enabled = false;
            nudResolution.Enabled = false;

            resolution = (int)nudResolution.Value;

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            int maxDensityValue = (int)nudDensity.Maximum;
            int invertedDensityValue = (int)nudDensity.Minimum + maxDensityValue - (int)nudDensity.Value;
            gameEngine = new GameEngine(height / resolution, width / resolution, invertedDensityValue);

            pictureBox1.Image = new Bitmap(width, height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            Text = $"Generation {gameEngine.CurrentGeneration}";
            timer1.Start(); // start game
        }

        private void StopGame()
        {
            if (timer1.Enabled == false)
                return;

            timer1.Stop();
            nudDensity.Enabled = true;
            nudResolution.Enabled = true;
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Black);

            bool[,] field = gameEngine.GetCurrentGeneration();


            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x,y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }


            pictureBox1.Refresh();

            Text = $"Generation: {gameEngine.CurrentGeneration}";

            gameEngine.GenerateNextGeneration();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled == false)
                return;

            int xCoord = e.Location.X / resolution;
            int yCoord = e.Location.Y / resolution;

            if (e.Button == MouseButtons.Left) // add life at coord
            {
                gameEngine.AddCell(xCoord, yCoord);
            }

            if (e.Button == MouseButtons.Right) // destroy life at coord
            {
                gameEngine.RemoveCell(xCoord, yCoord);
            }
        }
    }
}
