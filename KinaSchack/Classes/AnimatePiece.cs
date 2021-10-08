using KinaSchack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace KinaSchack.Classes
{
    /// <summary>
    /// Class <c>AnimatePiece</c> models a piece moving with variable velocity through the screen
    /// </summary>
    public class AnimatePiece
    {
        public Rect StartPosition;
        public Rect DrawPosition;
        public Rect EndPosition;
        public BoardStatus Player;
        public bool Done;
        public Vector2 Velocity;
        private readonly float _initialDistance;

        public AnimatePiece(Rect startPos, Rect endPos, BoardStatus player)
        {
            StartPosition = startPos;
            EndPosition = endPos;
            Player = player;
            Done = false;
            DrawPosition = startPos;
            Velocity = GetVelocity();
            _initialDistance = GetDistance();
        }
        /// <summary>
        /// This method updates the draw location. It speeds up in the beginning and slows down towards the end
        /// </summary>
        public void Update()
        {
            if (GetDistance() < 1)
            {
                Velocity = Vector2.Zero;
                Done = true;
            }
            DrawPosition.X += Velocity.X;
            DrawPosition.Y += Velocity.Y;
            if (GetDistance() > _initialDistance / 2)
            {
                Velocity.X *= (float)1.1;
                Velocity.Y *= (float)1.1;
            }
            else
            {
   
                Velocity.X = Math.Sign(Velocity.X) * (float)Math.Max(Math.Abs(Velocity.X * 0.9), 0.5);
                Velocity.Y = Math.Sign(Velocity.Y) * (float)Math.Max(Math.Abs(Velocity.Y * 0.9), 0.5);
            }
            //Getting close towards the end marks it done and we can stop drawing the animation

        }
        private Vector2 GetVelocity()
        {
            return new Vector2((float)(EndPosition.X - StartPosition.X) / 100, (float)(EndPosition.Y - StartPosition.Y) / 100);
        }
        private float GetDistance()
        {
            return Vector2.Distance(new Vector2((float)DrawPosition.X, (float)DrawPosition.Y), new Vector2((float)EndPosition.X, (float)EndPosition.Y));
        }
    }
}
