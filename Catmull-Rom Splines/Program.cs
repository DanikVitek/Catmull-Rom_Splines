using System;
using System.Linq;
using ConsoleGameEngine;

namespace Catmull_Rom_Splines
{
    class Program : ConsoleGame
    {
        private static int 
            windowWidth = 160, 
            windowHeight = 80, 
            fontWidth = 10, 
            fontHeight= 10;
        
        static void Main(string[] args)
        {
            new Program().Construct(windowWidth, windowHeight, fontWidth, fontHeight, FramerateMode.MaxFps) ;
        }

        private Spline path;

        private int selectedPoint;

        private Point cursor;

        private long cooldownTimeKey;

        private static long CurrentTimeMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public override void Create() {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();

            TargetFramerate = 15;

            cursor = new Point(windowWidth / 2, windowHeight / 2);
            cooldownTimeKey = CurrentTimeMillis();

            path.Points = new[]
            {
                new Point(10, 41),
                new Point(40, 41),
                new Point(70, 41),
                new Point(100, 41)
            };
        }

        public override void Update()
        {
            if (Engine.GetKeyDown(ConsoleKey.UpArrow))
                cursor.Y -= 2;
            else if (Engine.GetKeyDown(ConsoleKey.DownArrow))
                cursor.Y += 2;
            if (Engine.GetKeyDown(ConsoleKey.LeftArrow))
                cursor.X -= 2;
            else if (Engine.GetKeyDown(ConsoleKey.RightArrow))
                cursor.X += 2;

            // Select point
            if (Engine.GetKey(ConsoleKey.F))
            {
                for (int i = 0; i < path.Points.Length; i++)
                {
                    if (path.Points[i].X - 1 <= cursor.X && cursor.X <= path.Points[i].X + 1 &&
                        path.Points[i].Y - 1 <= cursor.Y && cursor.Y <= path.Points[i].Y + 1)
                    {
                        selectedPoint = i;
                        break;
                    }
                }
            }
            
            // Add new point
            if (Engine.GetKey(ConsoleKey.E) && CurrentTimeMillis() >= cooldownTimeKey)
            {
                path.Points = path.Points.Append(new Point(cursor.X, cursor.Y)).ToArray();
                cooldownTimeKey = CurrentTimeMillis() + 250L;
            }
            
            if (Engine.GetKeyDown(ConsoleKey.W))
                path.Points[selectedPoint].Y -= 2;
            else if (Engine.GetKeyDown(ConsoleKey.S))
                path.Points[selectedPoint].Y += 2;
            if (Engine.GetKeyDown(ConsoleKey.A))
                path.Points[selectedPoint].X -= 2;
            else if (Engine.GetKeyDown(ConsoleKey.D))
                path.Points[selectedPoint].X += 2;

            
        }

        public override void Render() {
            Engine.ClearBuffer();
            
            // Drawing spline:

            // draw connections
            for (float t = 0f; t <= path.Points.Length - 3f; t += 0.005f)
                Engine.SetPixel(path.GetSplinePoint(t), (int) Color.White);

            // Draw nodes + indexes
            for (int i = 0; i < path.Points.Length; i++)
            {
                Engine.Fill(new Point(path.Points[i].X-1, path.Points[i].Y-1), new Point(path.Points[i].X+2, path.Points[i].Y+2), i == selectedPoint ? (int) Color.Pink : (int) Color.Red);
                Engine.WriteText(path.Points[i], i.ToString(), (int) Color.Yellow);
            }
            
            // Draw cursor
            Engine.SetPixel(cursor, (int) Color.LightBlue);
            
            Engine.DisplayBuffer();
        }
    }
}