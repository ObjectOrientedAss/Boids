using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Boids
{
    class Program
    {
        public static Window Window;
        public static List<Boid> Boids;
        public static float DeltaTime { get { return Window.deltaTime; } }

        private const float spawnCd = 0.2f;
        private static float currentCd;


        static void Main(string[] args)
        {
            Window = new Window(1280, 720, "Boids");

            Boids = new List<Boid>();

            while (Window.IsOpened)
            {
                //INPUT
                Input();
                if (Window.GetKey(KeyCode.Esc))
                {
                    return;
                }

                //UPDATE
                foreach (Boid boid in Boids)
                {
                    boid.Update();
                }

                //DRAW
                foreach (Boid boid in Boids)
                {
                    boid.Draw();
                }

                Window.Update();
            }

        }

        public static void Input()
        {
            if (currentCd <= 0)
            {
                if (Window.mouseLeft)
                {
                    Boids.Add(new Boid(new Vector2(Window.mouseX, Window.mouseY)));
                    currentCd = spawnCd;
                }
            }
            else
            {
                currentCd -= DeltaTime;
            }
        }
    }
}
