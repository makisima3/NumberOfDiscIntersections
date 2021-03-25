using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int drawFieldWidth;
        private int drawFieldHeight;
        private int maxRadius;
        private int centimeter = 10;
        private int[] a;

        public MainWindow()
        {
            InitializeComponent();

            drawFieldHeight = (int)DrawField.Height;
            drawFieldWidth = (int)DrawField.Width;
            maxRadius = int.MaxValue;

            DrowCoordinateAxis();
            //DrawCircle(5, 0);
            //a = FillArray(5);
            a = new int[6] { 1, 5, 2, 1, 4, 0 };
            DrawCircleByArray(a);

            Count.Content = $"The number of pairs of intersecting discs: {solution(a)}";
        }

        private int solution(int[] A)
        {
            int N = A.Length;
            int[] sum = new int[N];

            for (int i = 0; i < N; i++)
            {

                int right;

                if (N - 1 >= A[i] + i)
                {
                    right = i + A[i];
                }
                else
                {
                    right = N - 1;
                }

                sum[right]++;
            }

            for (int i = 1; i < N; i++)
            {
                sum[i] += sum[i - 1];
            }

            int result = N * (N - 1) / 2;

            for (int j = 0; j < N; j++)
            {

                int left;

                if (j - A[j] < 0)
                {
                    left = 0;
                }
                else
                {
                    left = j - A[j];
                }

                if (left > 0)
                {
                    result -= sum[left - 1];//.
                }
            }

            if (result > 10000000)
            {
                return -1;
            }

            return result;
        }

        private int[] FillArray(int count)
        {
            int[] arr = new int[count];
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                arr[i] = rnd.Next(1, maxRadius);
            }

            return arr;
        }

        private void DrawCircleByArray(int[] A)
        {
            for (int i = 0; i < a.Length; i++)
            {
                DrawCircle(a[i], i);
            }
        }

        private void DrawCircle(int r, int x)
        {
            r *= centimeter * 2;
            if (r > 0)
            {
                x = x * centimeter + drawFieldWidth / 2;

                Ellipse ellipse = new Ellipse();

                ellipse.Width = r;
                ellipse.Height = r;
                ellipse.Stroke = Brushes.Black;

                Canvas.SetLeft(ellipse, x - (r / 2));
                Canvas.SetTop(ellipse, (drawFieldHeight / 2) - (r / 2));

                DrawField.Children.Add(ellipse);
            }
        }

        private void DrowCoordinateAxis()
        {
            Line lineX = new Line();
            Line lineY = new Line();

            lineX.Stroke = Brushes.Black;
            lineY.Stroke = Brushes.Black;

            lineX.X1 = 0;
            lineX.X2 = drawFieldWidth;
            lineX.Y1 = drawFieldHeight / 2;
            lineX.Y2 = drawFieldHeight / 2;

            lineY.X1 = drawFieldWidth / 2;
            lineY.X2 = drawFieldWidth / 2;
            lineY.Y1 = 0;
            lineY.Y2 = drawFieldHeight;

            DrawField.Children.Add(lineX);
            DrawField.Children.Add(lineY);

            DrowAxisCentimeter();
        }

        private void DrowAxisCentimeter()
        {
            Point zeroPoint = new Point(drawFieldWidth / 2, drawFieldHeight / 2);

            for (int x = 0; x < drawFieldWidth; x += centimeter)
            {
                Line centimeter = new Line();

                centimeter.Stroke = Brushes.Red;

                centimeter.X1 = x;
                centimeter.X2 = x;
                centimeter.Y1 = drawFieldHeight / 2 - 5;
                centimeter.Y2 = drawFieldHeight / 2 + 5;

                DrawField.Children.Add(centimeter);
            }

            for (int y = 0; y < drawFieldWidth; y += centimeter)
            {
                Line centimeter = new Line();

                centimeter.Stroke = Brushes.Red;

                centimeter.X1 = drawFieldWidth / 2 - 5;
                centimeter.X2 = drawFieldWidth / 2 + 5;
                centimeter.Y1 = y;
                centimeter.Y2 = y;

                DrawField.Children.Add(centimeter);
            }
        }

        private void ClearDrawField()
        {
            List<Ellipse> remove = new List<Ellipse>();

            foreach (var childe in DrawField.Children)
            {
                if (childe is Ellipse)
                {
                    remove.Add(childe as Ellipse);
                }
            }

            foreach (var childe in remove)
            {
                DrawField.Children.Remove(childe);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int count = Convert.ToInt32(input.Text);
                maxRadius = Convert.ToInt32(maxR.Text);

                ClearDrawField();

                a = FillArray(count);

                if (count < 1000)
                    DrawCircleByArray(a);

                Count.Content = $"The number of pairs of intersecting discs: {solution(a)}";
            }
            catch (Exception ex)
            {
                Count.Content = "Enter valid data.";
            }
        }
    }
}
