using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Boids
{
    class Boid
    {
        protected Sprite sprite;
        protected static Texture texture;
        protected float speed;

        public Vector2 Velocity { get; protected set; }

        protected static float alignmentRadius;
        protected static float cohesionRadius;
        protected static float separationRadius;

        protected static float alignmentWeight;
        protected static float cohesionWeight;
        protected static float separationWeight;

        protected static float steerMultiplier;

        protected static float sightHalfAngle = MathHelper.DegreesToRadians(150);

        public Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }

        public Vector2 Forward
        {   //get the angle using the sprite rotation and transform it in a new vector2
            get { return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation)); } // view direction
            //set the angle using the atan2 method with the current vector2
            set { sprite.Rotation = (float)Math.Atan2(value.Y, value.X); }
        }

        public Boid(Vector2 position)
        {
            if (texture == null)
            {
                texture = new Texture("Assets/boid.png");
            }

            sprite = new Sprite(texture.Width, texture.Height);
            sprite.position = position;
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            //velocity = Vector2.Zero;
            speed = 150f;

            float directionX = RandomGenerator.GetRandomFloat() * (RandomGenerator.GetRandom(0, 2) == 0 ? 1 : -1);
            float directionY = RandomGenerator.GetRandomFloat() * (RandomGenerator.GetRandom(0, 2) == 0 ? 1 : -1);

            Velocity = new Vector2(directionX, directionY).Normalized() * speed;
            Forward = Velocity;

            alignmentRadius = 300f;
            cohesionRadius = 200f;
            separationRadius = 70f;

            alignmentWeight = 1.0f;
            cohesionWeight = 1.0f;
            separationWeight = 8.0f;

            steerMultiplier = 4.0f;
        }

        //3 raggi, 3 pesi, velocità, steermultip

        //sighthalfangle = mathhelper.degreestoradians(150)
        //get alignment prende la media dei forward dei vicini * alignment weight
        //cohesion posizione media
        //separation 
        //somma dei vettori sull'asse x e poi sull'asse y
        //
        //se il risultato della somma è != vector2.zero

        //normalizzo result

        //velocity = (lerp vettore di partenza, vettore finale * velocità, deltatime * sterzata)
        //velocity.normalized * speed

        //if velocity > 0
        //sprite rotation = atan2(velocity y, velocity x)

        public virtual void PacmanEffect()
        {
            if (sprite.position.X > Program.Window.Width + sprite.pivot.X && Velocity.X > 0)
            {
                sprite.position.X = 0 - sprite.pivot.X;
            }
            else if (sprite.position.X < 0 - sprite.pivot.X && Velocity.X < 0)
            {
                sprite.position.X = Program.Window.Width + sprite.pivot.X;
            }

            if (sprite.position.Y > Program.Window.Height + sprite.pivot.Y && Velocity.Y > 0)
            {
                sprite.position.Y = 0 - sprite.pivot.Y;
            }
            else if (sprite.position.Y < 0 - sprite.pivot.Y && Velocity.Y < 0)
            {
                sprite.position.Y = Program.Window.Height + sprite.pivot.Y;
            }
        }

        protected virtual bool CheckVisible(Vector2 position, float radius, float halfAngle, out Vector2 distance)
        {
            distance = position - Position;
            if (distance.Length < radius)
            {
                float angle = (float)Math.Acos(Vector2.Dot(Forward, distance.Normalized()));

                if (angle <= halfAngle)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void Update()
        {
            sprite.position += Velocity * Program.DeltaTime;

            PacmanEffect();

            Vector2 result = Vector2.Zero;
            Vector2 alignment = GetAlignment() * alignmentWeight;
            Vector2 cohesion = GetCohesion() * cohesionWeight;
            Vector2 separation = GetSeparation() * separationWeight;

            result += alignment + cohesion + separation;

            if (result != Vector2.Zero)
            {
                Velocity = Vector2.Lerp(Velocity, result.Normalized() * speed, steerMultiplier * Program.DeltaTime);
                Velocity = Velocity.Normalized() * speed;
            }

            Forward = Velocity;

        }

        protected virtual Vector2 GetAlignment()
        {
            Vector2 alignment = Vector2.Zero;
            Vector2 distance;
            int neighbors = 0;
            foreach (Boid b in Program.Boids)
            {
                if (b == this)
                {
                    continue;
                }

                if (CheckVisible(b.Position, alignmentRadius, sightHalfAngle, out distance))
                {
                    neighbors++;
                    alignment += b.Forward;
                }
            }
            if (neighbors != 0)
            {
                return (alignment / neighbors).Normalized();
            }
            return alignment;
        }

        protected virtual Vector2 GetCohesion()
        {
            Vector2 cohesion = Vector2.Zero;
            Vector2 distance;
            int neighbors = 0;

            foreach (Boid b in Program.Boids)
            {
                if (b == this)
                {
                    continue;
                }

                if (CheckVisible(b.Position, cohesionRadius, sightHalfAngle, out distance))
                {
                    neighbors++;
                    cohesion += b.Position;
                }
            }

            if (neighbors != 0)
            {
                cohesion /= neighbors;
                cohesion -= Position;
                cohesion.Normalize();
            }

            return cohesion;
        }

        protected virtual Vector2 GetSeparation()
        {
            Vector2 separation = Vector2.Zero;
            Vector2 distance;
            int neighbors = 0;

            foreach (Boid b in Program.Boids)
            {
                if (b == this)
                {
                    continue;
                }
                if (CheckVisible(b.Position, separationRadius, sightHalfAngle, out distance))
                {
                    neighbors++;
                    separation += distance;
                }
            }
            if (neighbors != 0)
            {
                separation = -(separation / neighbors).Normalized();
            }
            return separation;
        }

        public virtual void Draw()
        {
            sprite.DrawTexture(texture);
        }

    }


}
