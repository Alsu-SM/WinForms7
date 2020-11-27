using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms7
{
    public partial class Form1 : Form
    {
        Graphics gr;
        Bitmap bmp;
        Matrix m;
        int[,] temp;
        Pen p = new Pen(Color.Red, 4);
        Pen p1 = new Pen(Color.Black, 2);
        Pen p2 = new Pen(Color.Black, 1);
        Pen p3 = new Pen(Color.Purple, 5); // ручка для первой точки
        Pen p4 = new Pen(Color.Blue, 5); // ручка для второй точки (чтобы понять направление движения)

        Point dot = new Point(10, 10);
        public Form1()
        {
            InitializeComponent();

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(bmp);
     
        }

        public Matrix Init (string fileName)
        {
            string[] split;
            StreamReader reader = new StreamReader(fileName);
            string currentString = reader.ReadLine(); // прочитали первую строку, где лежит количество вершин
            int CityCount = Int32.Parse(currentString);
            double[,] tempM = new double[CityCount, CityCount];
            temp = new int[CityCount, 2];
            for (int i = 0; i < CityCount; i++)
            {
                currentString = reader.ReadLine();
                split = currentString.Split(' '); //разделяем строку по пробелам
                temp[i, 0] = Int32.Parse(split[0]);
                temp[i, 1] = Int32.Parse(split[1]);

            }

            for (int i = 0; i < CityCount; i++)
            {
                for (int j = 0; j < CityCount; j++)
                {
                    tempM[i, j] = getDistance(temp[i, 0], temp[i, 1], temp[j, 0], temp[j, 1]);
                    tempM[j, i] = tempM[i, j];
                }
            }

            Matrix M = new Matrix(CityCount, tempM);
            return M;
        }

        public void DrawCity()
        {
            gr.DrawLine(p1, 0, 0, 0, pictureBox1.Height);
            gr.DrawLine(p1, 0, 0, pictureBox1.Width, 0);
            gr.DrawLine(p1, 0, pictureBox1.Height, pictureBox1.Width, pictureBox1.Height);
            gr.DrawLine(p1, pictureBox1.Width, pictureBox1.Height, pictureBox1.Width, 0);

            for (int i = 0; i < m.funcCityCount; i++)
            {
                gr.DrawEllipse(p, dot.X + temp[i,0]*2, dot.Y + temp[i,1]*2, 1, 1);
                
            }

            pictureBox1.Image = bmp;

           

            
        }

        public double getDistance(int x1, int y1, int x2, int y2)
        {
            double dist;
            dist = Math.Round(Math.Sqrt((x2-x1)*(x2-x1) + (y2 - y1)*(y2 - y1)), 1);
            return dist;
        }

        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                gr.Clear(Color.Transparent);
                richTextBox1.Text = sr.ReadToEnd();
                m = Init(openFileDialog1.FileName);
                sr.Close();

                string str;
                str = "\n";

                for (int i = 0; i < m.funcCityCount; i++)
                {
                    str += "\n";
                    for (int j = 0; j < m.funcCityCount; j++)
                        str += m.funcMatrix[i, j].ToString() + " ";
                }

                richTextBox1.Text += str;
                DrawCity();


                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();


            }
        }

        private void button1_Click(object sender, EventArgs e) // решение задачи
        {
            List<int> solution = new List<int>();
            double criterium;
            solution = m.Algorithm();
            criterium = m.Aim(solution);

            richTextBox1.Text += "\n\n" + criterium.ToString() + "\n";
            for (int i = 0; i < m.funcCityCount + 1; i++)
            {
                richTextBox1.Text += solution[i].ToString() + " ";
            }

            dataGridView1.Rows.Add(m.funcCityCount);
            for (int i = 0; i < m.funcCityCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = solution[i];
                dataGridView1.Rows[i].Cells[1].Value = solution[i+1];
                dataGridView1.Rows[i].Cells[2].Value = m.funcMatrix[solution[i],solution[i+1]];

           
                gr.DrawLine(p2, dot.X + temp[solution[i], 0] * 2, dot.Y + temp[solution[i], 1] * 2, dot.X + temp[solution[i+1], 0] * 2, dot.Y + temp[solution[i+1], 1] * 2);
                if (i == 0)
                {
                    gr.DrawEllipse(p3, dot.X + temp[solution[i], 0] * 2, dot.Y + temp[solution[i], 1] * 2, 1,1);

                }

                if (i == 1)
                {
                    gr.DrawEllipse(p4, dot.X + temp[solution[i], 0] * 2, dot.Y + temp[solution[i], 1] * 2, 1, 1);

                }
            }

            dataGridView1.Rows[m.funcCityCount].Cells[0].Value = "Критерий";
            dataGridView1.Rows[m.funcCityCount].Cells[2].Value = criterium;
            pictureBox1.Image = bmp;


        }

        private void выйтиИзПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
