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
    public class AnimatePiece
    {
        public Rect StartPosition;
        public Rect DrawPosition;
        public Rect EndPosition;
        public BoardStatus Player;
        public bool Done;
        public Vector2 Velocity;
        private float _initialDistance;

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

        public void Update()
        {
            DrawPosition.X += Velocity.X;
            DrawPosition.Y += Velocity.Y;
            if (GetDistance() > _initialDistance / 2)
            {
                Velocity.X *= (float)1.1;
                Velocity.Y *= (float)1.1;
            }
            else
            {
                Velocity.X *= (float)0.9;
                Velocity.Y *= (float)0.9;
            }
            if (GetDistance() < 0.1)
            {
                Velocity = Vector2.Zero;
                Done = true;
            }
            
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
