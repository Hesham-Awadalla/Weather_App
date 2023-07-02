using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Weather_App
{
    class Clock
    {
        DispatcherTimer timer = new DispatcherTimer();

        int r, Xm, Ym, Xstd, Ystd, Xmin, Ymin, Xsek, Ysek, sek, min, std;

        DateTime localTime;

        Line StundenZeiger = new Line();
        Line MinutenZeiger = new Line();
        Line SekundenZeiger = new Line();

        string canvasKey;

        public Clock(int rad, string canvasKey)
        {
            r = Xm = Ym = rad;

            this.canvasKey = canvasKey;

            ((Canvas)App.Current.Resources[canvasKey]).Children.Add(StundenZeiger);
            ((Canvas)App.Current.Resources[canvasKey]).Children.Add(MinutenZeiger);
            ((Canvas)App.Current.Resources[canvasKey]).Children.Add(SekundenZeiger);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) => paint();
            timer.Start();
        }

        public void paint()
        {
            Dictionary<string, int> localTimesClocks = new Dictionary<string, int>()
            {
                {"canvasMainClock", MainWindow.stdMainLocalTime},
                {"canvasClock1", MainWindow.stdLocation1LocalTime},
                {"canvasClock2", MainWindow.stdLocation2LocalTime},
                {"canvasClock3", MainWindow.stdLocation3LocalTime}
            };

            localTime = DateTime.Now;

            sek = localTime.Second;
            min = localTime.Minute;

            //LocalTime from the API!  24-Format!!
            if (localTimesClocks[canvasKey] > 12)
            {
                std = localTimesClocks[canvasKey] - 12;
            }
            else
            {
                std = localTimesClocks[canvasKey];
            }

            Xstd = Xm + (int)(0.6 * r * Math.Sin((std * Math.PI / 6) + (min * Math.PI / 360)));
            Ystd = Ym - (int)(0.6 * r * Math.Cos((std * Math.PI / 6) + (min * Math.PI / 360)));

            Xmin = Xm + (int)(0.8 * r * Math.Sin(min * Math.PI / 30));
            Ymin = Ym - (int)(0.8 * r * Math.Cos(min * Math.PI / 30));

            Xsek = Xm + (int)(0.9 * r * Math.Sin(sek * Math.PI / 30));
            Ysek = Ym - (int)(0.9 * r * Math.Cos(sek * Math.PI / 30));

            //StundenZeiger
            StundenZeiger.Visibility = System.Windows.Visibility.Visible;
            StundenZeiger.StrokeThickness = 2;
            StundenZeiger.Stroke = System.Windows.Media.Brushes.WhiteSmoke;
            StundenZeiger.X1 = Xm;
            StundenZeiger.Y1 = Ym;
            StundenZeiger.X2 = Xstd;
            StundenZeiger.Y2 = Ystd;

            //MinutenZeiger
            MinutenZeiger.Visibility = System.Windows.Visibility.Visible;
            MinutenZeiger.StrokeThickness = 1;
            MinutenZeiger.Stroke = System.Windows.Media.Brushes.WhiteSmoke;
            MinutenZeiger.X1 = Xm;
            MinutenZeiger.Y1 = Ym;
            MinutenZeiger.X2 = Xmin;
            MinutenZeiger.Y2 = Ymin;

            //SekundenZeiger
            SekundenZeiger.Visibility = System.Windows.Visibility.Visible;
            SekundenZeiger.StrokeThickness = 0.5;
            SekundenZeiger.Stroke = System.Windows.Media.Brushes.Red;
            SekundenZeiger.X1 = Xm;
            SekundenZeiger.Y1 = Ym;
            SekundenZeiger.X2 = Xsek;
            SekundenZeiger.Y2 = Ysek;
        }
    }
}
