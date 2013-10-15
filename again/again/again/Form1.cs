using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace again
{
    public partial class Form1 : Form
    {
        public int M;
        public int N;
        public int I;
        public int J;
        public Route route;
        public BackRoute backRoute;
        public bool myEnd = false;
        public List<Route> rightRoutes=new List<Route>();
        public List<Route> backRoutes=new List<Route>();
        public Form1()
        {
            InitializeComponent();
        }

        public void initOriginalData()
        {
            myEnd = false;
            M = Convert.ToInt32(textBox1.Text);
            N = Convert.ToInt32(textBox2.Text);
            I = Convert.ToInt32(textBox3.Text);
            J = Convert.ToInt32(textBox4.Text);
            route = new Route(M, N);
            initMyTable(route.myRoute);
            backRoute = new BackRoute(M, N);
            comboBox2.Items.Clear();
            rightRoutes = new List<Route>();
            backRoutes = new List<Route>();

        }

        //初始化route.myRoute，可以走的方向赋0，不能走的方向赋2
        public void initMyTable(int[, ,] myTable)
        {
            int M = myTable.GetLength(0);
            int N = myTable.GetLength(1);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {//z值代表8个方向
                    myTable[i, j, 0] = (i + 1 < M && j - 2 >= 0) ? 0 : 2;
                    myTable[i, j, 1] = (i + 2 < M && j - 1 >= 0) ? 0 : 2;
                    myTable[i, j, 2] = (i + 2 < M && j + 1 < N) ? 0 : 2;
                    myTable[i, j, 3] = (i + 1 < M && j + 2 < N) ? 0 : 2;
                    myTable[i, j, 4] = (i - 1 >= 0 && j + 2 < N) ? 0 : 2;
                    myTable[i, j, 5] = (i - 2 >= 0 && j + 1 < N) ? 0 : 2;
                    myTable[i, j, 6] = (i - 2 >= 0 && j - 1 >= 0) ? 0 : 2;
                    myTable[i, j, 7] = (i - 1 >= 0 && j - 2 >= 0) ? 0 : 2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initOriginalData();
            int i=I,j=J;    //起点
            while (true)
            {
                
                int z = 0;
                forward(ref i, ref j, ref z);
                back(ref i, ref j, ref z);
                if (myEnd)
                {
                    textBox5.Text = rightRoutes.Count.ToString();
                    for(int itm=0;itm<rightRoutes.Count;itm++)
                    {
                        comboBox2.Items.Add((object)itm);
                    }

                    MessageBox.Show(rightRoutes.Count.ToString());
                    return;
                }

            }

        }


        public void forward(ref int i, ref int j, ref int z)
        {
            for (; z < 8; z++)
            {
                if (z <= 7 && route.myRoute[i, j, z] == 0)
                {
                    //bool isGot = nextIJ(ref i, ref j, z, route);
                    //if(

                    if (returnExistIndexInRoutes(route, backRoutes, z))
                    {//如果怎样怎样就跳出  && isInRoutes(route, backRoutes) 
                        //              condition1 = true; 
                        continue;
                    }
                    else
                    {
                        if (nextIJ(ref i, ref j, z, route))
                        {
                            Route tempRoute = new Route(M, N);
                            copyRoute(route, tempRoute);
                            if (isInRoutes(tempRoute, rightRoutes))
                                tempRoute.first = true;
                            else
                                tempRoute.first = false;
                            rightRoutes.Add(tempRoute);
                            PreviousIJ(ref i, ref j, route);


                        };
                        break;
                    }

                }
            }
        }
        

        public void back(ref int i,ref int j,ref int z)
        {
            if (z == 8)
            {
                if (route.inOut.Count == 0)
                {
                    myEnd = true;
                    return;
                }

                PreviousIJ(ref i, ref j, route);
                
                    
                
            }
        }
        //把之前修改过的route状态更改回来
        public bool PreviousIJ(ref int i, ref int j, Route route)
        {

            int z = route.inOut.Pop();
            route.myRoute[i, j, z] = 0;
            int flag = (z + 4) % 8;
            switch (flag)
            {
                case 0:
                    i--; j += 2;
                    break;
                case 1:
                    i -= 2; j++;
                    break;
                case 2:
                    i -= 2; j--;
                    break;
                case 3:
                    i--; j -= 2;
                    break;
                case 4:
                    i++; j -= 2;
                    break;
                case 5:
                    i += 2; j--;
                    break;
                case 6:
                    i += 2; j++;
                    break;
                case 7:
                    i++; j += 2;
                    break;
            }
            route.myRoute[i, j, flag] = 0;

            Route tempRoute = new Route(M, N);
            copyRoute(route, tempRoute);
            tempRoute.backDirection = flag;
            backRoutes.Add(tempRoute);


            //route.backDirection = flag;
            if (i == I && j == J)
                return true;
            else
                return false;


        }
        //public bool condition1 = false;

        public bool nextIJ(ref int i, ref int j, int z, Route route)
        {
            route.myRoute[i, j, z] = 1;
            switch (z)
            {
                case 0:
                    i++; j -= 2;
                    break;
                case 1:
                    i += 2; j--;
                    break;
                case 2:
                    i += 2; j++;
                    break;
                case 3:
                    i++; j += 2;
                    break;
                case 4:
                    i--; j += 2;
                    break;
                case 5:
                    i -= 2; j++;
                    break;
                case 6:
                    i -= 2; j--;
                    break;
                case 7:
                    i--; j -= 2;
                    break;
            }
            route.myRoute[i, j, (z + 4) % 8] = 1;
            route.inOut.Push((z + 4) % 8);
            if (i == I && j == J)
                return true;
            else
                return false;
        }
       
        public void copyRoute(Route source,Route destination)
        {
            for (int i = 0; i < source.myRoute.GetLength(0); i++)
            {
                for (int j = 0; j < source.myRoute.GetLength(1); j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        destination.myRoute[i, j, k] = source.myRoute[i, j, k];
                    }
                }
            }
            //副本
            Stack<int> pop = new Stack<int>();
            int sourceCount = source.inOut.Count;
            for (int k = 0; k < sourceCount; k++)
            {
                pop.Push(source.inOut.Pop());
            }
            for (int k = 0; k < sourceCount; k++)
            {
                int t = pop.Pop();
                destination.inOut.Push(t);
                source.inOut.Push(t);
            }
        }
        
       
        public bool isSame(Route r1, Route r2)
        {
            for (int i = 0; i < r1.myRoute.GetLength(0); i++)
            {
                for (int j = 0; j < r1.myRoute.GetLength(1); j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        if (r1.myRoute[i, j, k] != r2.myRoute[i, j, k])
                            return false;
                    }
                }
            }
            return true;
        }
        public bool isInRoutes(Route r, List<Route> routes)
        {
            for(int i=0;i<routes.Count;i++)
            {
                if(isSame(r,routes[i]))
                    return true;
            }
            return false;
        }
        public bool returnExistIndexInRoutes(Route r, List<Route> routes,int z)
        {
            List<int> indexs = new List<int>();
            for (int i = 0; i < routes.Count; i++)
            {
                if (isSame(r, routes[i]))
                    indexs.Add(routes[i].backDirection);
            }
            for (int i = 0; i < indexs.Count; i++)
            {
                if (indexs[i] == z)
                    return true;
            }
            return false;
        }
        public void returnIJ(ref int i,ref int j,int z)
        {
            switch (z)
            {
                case 0:
                    i++; j -= 2;
                    break;
                case 1:
                    i += 2; j--;
                    break;
                case 2:
                    i += 2; j++;
                    break;
                case 3:
                    i++; j += 2;
                    break;
                case 4:
                    i--; j += 2;
                    break;
                case 5:
                    i -= 2; j++;
                    break;
                case 6:
                    i -= 2; j--;
                    break;
                case 7:
                    i--; j -= 2;
                    break;
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int M = Convert.ToInt32(textBox1.Text);
            int N = Convert.ToInt32(textBox2.Text);
            int I = Convert.ToInt32(textBox3.Text);
            int J = Convert.ToInt32(textBox4.Text);

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            int perWidth = width / (M - 1);
            int perHeight = height / (N - 1);
            Pen pen = new Pen(Color.Blue, 1);
            for (int i = 1; i < M; i++)//画列
            {
                e.Graphics.DrawLine(pen, new Point(i * perWidth, 0), new Point(i * perWidth, height));
            }
            for (int i = 1; i < N; i++)//画行
            {
                e.Graphics.DrawLine(pen, new Point(0, i * perHeight), new Point(width, i * perHeight));
            }

            pen = new Pen(Color.Purple, 1);
            if (rightRoutes.Count > 0)
            {
                int _i=rightRoutes[Convert.ToInt32(comboBox2.Text)].myRoute.GetLength(0);
                int _j=rightRoutes[Convert.ToInt32(comboBox2.Text)].myRoute.GetLength(1);
                int _k=8;

                bool first=true;
                for (int i = 0; i < _i; i++)
                {
                    for (int j = 0; j < _j; j++)
                    {
                        for (int k = 0; k < _k; k++)
                        {
                            if (rightRoutes[Convert.ToInt32(comboBox2.Text)].myRoute[i, j, k] == 1)
                            {
                                //if (i == I && j == J && first)
                                //{

                                //    pen = new Pen(Color.Yellow, 1);
                                //    first = false;

                                //}
                                //else
                                //{
                                //    pen = new Pen(Color.Purple, 1);
                                //}
                                int tempI = i, tempJ = j;
                                returnIJ(ref tempI, ref tempJ, k);
                                Point[] pArr = new Point[2];
                                pArr[0] = new Point();
                                pArr[0].X = i * perWidth; pArr[0].Y = j * perHeight;
                                pArr[1] = new Point();
                                pArr[1].X = tempI * perWidth; pArr[1].Y = tempJ * perHeight;
                                e.Graphics.DrawLines(pen, pArr);
                            }
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
