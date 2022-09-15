using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrjOrdenar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graphics g;
        Brush black = new SolidBrush(Color.Black);
        Brush white = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
        Random rand = new Random();
        int[] A = new int[500];
        int aux = 0, delay = 1, speed = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            for (int i = 0; i < 500; i++)
                A[i] = i + 1;
            A = A.OrderBy(x => rand.Next()).ToArray();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 500; i++)
            {
                g.FillRectangle(white, i * 2, 100, 2, 500 - A[i]);
                g.FillRectangle(black, i * 2, 600 - A[i], 2, A[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            delay = 1;
            speed = 0;
            if (comboBox1.Text == "Bubble")
                Bubble();
            else if (comboBox1.Text == "Selection")
                Selection();
            else if (comboBox1.Text == "Insertion")
                Insertion();
            else if (comboBox1.Text == "Quick")
                Quick(0, 500);
            else if (comboBox1.Text == "Merge")
                Merge(0, 500);
            else if (comboBox1.Text == "Gravity")
                Gravity();
            else if (comboBox1.Text == "Comb")
                Comb();
            else if (comboBox1.Text == "Radix")
                Radix();
            else if (comboBox1.Text == "Odd-even")
                Odd_even();
            else if (comboBox1.Text == "Stooge")
                Stooge(0, 500);
            else if (comboBox1.Text == "Cycle")
                Cycle();
            else if (comboBox1.Text == "Shell")
                Shell();
            else if (comboBox1.Text == "Heap")
                Heap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            delay = 0;
            A = A.OrderBy(x => rand.Next()).ToArray();
        }

        private void trocar(int i, int j)
        {
            aux = A[i];
            A[i] = A[j];
            A[j] = aux;
        }

        private void inserir(int A, int B)
        {
            if (A < B)
                for (int i = A; i < B; i++)
                    trocar(i, i + 1);
            else if (A > B)
                for (int i = A; i > B; i--)
                    trocar(i, i - 1);
        }

        private async void Bubble()
        {
            for (int i = 1; i < 500; i++)
            {
                for (int j = 0; j < 500 - i; j++)
                    if (A[j] > A[j + 1])
                        trocar(j, j + 1);
                await Task.Delay(delay);
            }
        }

        private async void Selection()
        {
            int place = 0;
            for (int i = 0; i < 500; i++)
            {
                int value = 600;
                for (int j = i; j < 500; j++)
                    if (A[j] < value)
                    {
                        place = j;
                        value = A[j];
                    }
                trocar(i, place);
                await Task.Delay(delay);
            }
        }

        private async void Insertion()
        {
            for (int i = 1; i < 500; i++)
            {
                bool look = true;
                for (int j = i; j > 0 & look; j--)
                    if (A[j] < A[j - 1])
                        trocar(j, j - 1);
                    else
                        look = false;
                await Task.Delay(delay);
            }
        }

        private async Task Quick(int min, int max)
        {
            int i = min, j = max - 1;
            int pivot = A[rand.Next(min, max)];
            while (i < j)
            {
                if (A[i] > A[j])
                {
                    trocar(i, j);
                    await Task.Delay(delay);
                }
                if (A[i] < pivot)
                    i++;
                if (A[j] > pivot)
                    j--;
            }
            if (i - min > 1)
                await Quick(min, i);
            if (max - i > 1)
                await Quick(i, max);
        }

        private async Task Merge(int min, int max)
        {
            int mid = (max + min) / 2;
            if (mid - min > 1)
                await Merge(min, mid);
            if (max - mid > 1)
                await Merge(mid, max);
            int i = min, j = mid;
            while (i < j & j < max)
            {
                if (A[i] > A[j])
                {
                    inserir(j, i);
                    j++;
                    await Task.Delay(delay);
                }
                i++;
            }
        }

        private async void Gravity()
        {
            for (int i = 0; i < 500; i++)
            {
                for (int j = 499; j > 0; j--)
                    if (A[j] < A[j - 1])
                        trocar(j, j - 1);
                await Task.Delay(delay);
            }
        }

        private async void Comb()
        {
            int gap = 250;
            bool sort = false;
            while (!sort)
            {
                sort = true;
                for (int j = 0; j < 500 - gap; j++)
                    if (A[j] > A[j + gap])
                    {
                        trocar(j, j + gap);
                        sort = false;
                        await Task.Delay(delay);
                    }
                if (gap > 1)
                    gap = gap * 10 / 13;
            }
        }

        private async void Radix()
        {
            int i, j;
            for (int k = 8; k >= 0; k--)
            {
                i = 0;
                j = 0;
                while (j < 500)
                {
                    string s = Convert.ToString(A[j], 2);
                    while (s.Length < 9)
                        s = s.Insert(0, "0");
                    if (s[k] == '0')
                    {
                        inserir(j, i);
                        i++;
                        await Task.Delay(delay);
                    }
                    j++;
                }
            }
        }

        private async void Odd_even()
        {
            bool sort = false;
            while (!sort)
            {
                sort = true;
                for (int i = 0; i < 500; i += 2)
                    if (A[i] > A[i + 1])
                    {
                        trocar(i, i + 1);
                        sort = false;
                    }
                for (int i = 1; i < 499; i += 2)
                    if (A[i] > A[i + 1])
                    {
                        trocar(i, i + 1);
                        sort = false;
                    }
                await Task.Delay(delay);
            }
        }

        private async Task Stooge(int min, int max)
        {
            if (A[min] > A[max - 1])
            {
                trocar(min, max - 1);
                if (speed % 25 == 0)
                    delay = 1;
                else
                    delay = 0;
                speed++;
                await Task.Delay(delay);
            }
            if (max - min > 2)
            {
                int t = (max - min) / 3; 
                await Stooge(min, max - t);
                await Stooge(min + t, max);
                await Stooge(min, max - t);
            }
        }

        private async void Cycle()
        {
            int i = 0;
            while (i < 500)
            {
                int count = 0;
                for (int j = 0; j < 500; j++)
                    if (A[j] < A[i])
                        count++;
                if (i != count)
                {
                    trocar(i, count);
                    await Task.Delay(delay);
                }
                else
                    i++;
            }
        }

        private async void Shell()
        {
            int gap = 512;
            while (gap > 1)
            {
                gap /= 2;
                for (int k = 0; k < gap; k++)
                {
                    for (int i = gap + k; i < 500; i += gap)
                    {
                        bool look = true;
                        for (int j = i; j > gap & look; j -= gap)
                            if (A[j] < A[j - gap])
                                trocar(j, j - gap);
                            else
                                look = false;
                        if (speed % 10 == 0)
                            delay = 1;
                        else
                            delay = 0;
                        speed++;
                        await Task.Delay(delay);
                    }
                }
            }
        }

        private async void Heap()
        {
            for (int i = 250; i >= 0; i--)
            {
                Heapify(i, 499);
                await Task.Delay(delay);
            }
            for (int i = 499; i > 0; i--)
            {
                Heapify(0, i);
                trocar(0, i);
                await Task.Delay(delay);
            }
        }

        private void Heapify(int i, int max)
        {
            int l = 2 * i + 1;
            int r = 2 * i + 2;
            int down = i;
            if (l <= max)
                if (r <= max)
                {
                    if (A[l] > A[r] & A[i] < A[l])
                        down = l;
                    if (A[r] > A[l] & A[i] < A[r])
                        down = r;
                }
                else if (A[i] < A[l])
                    down = l;
            if (i != down)
            {
                trocar(i, down);
                Heapify(down, max);
            }
        }
    }
}