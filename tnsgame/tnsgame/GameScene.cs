using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Foundation;
using UIKit;

namespace TNSGame
{
    public class GameScene : CCScene
    {
        private CCLayer mainLayer;
        private CCSprite course;
        private CCSprite flag;
        private CCLabel strokes,sunkit;
        private CCEventListenerTouchAllAtOnce touchListener;
        private CCSprite ballSprite;
        private float ballXVelocity;
        private float ballYVelocity;
        private float StartX;
        private float StartY;
        private int strokeCount;
        private bool isOver;

        public GameScene(CCWindow mainWindow)
            : base(mainWindow)
        {
            
            mainLayer = new CCLayer();
            AddChild(mainLayer);

            course = new CCSprite("golfcourse");
            course.Position = mainLayer.VisibleBoundsWorldspace.Center;
            var scaleWidth = mainLayer.VisibleBoundsWorldspace.MaxX / course.ContentSize.Width;
            var scaleHeight = mainLayer.VisibleBoundsWorldspace.MaxY / course.ContentSize.Height;
            course.ScaleX = scaleWidth;                   // Scales by multiplying the current size.
            course.ScaleY = scaleHeight;
            mainLayer.AddChild(course);

            flag = new CCSprite("flag"); // positioned at 232, 95 
            flag.Position = new CCPoint( mainLayer.VisibleBoundsWorldspace.MinX+(232*scaleWidth),
                                         mainLayer.VisibleBoundsWorldspace.MaxY-(115*scaleHeight));
            flag.AnchorPoint = CCPoint.AnchorMiddleBottom;
            flag.VertexZ = 1;
            mainLayer.AddChild(flag);

            strokes = new CCLabel("Strokes: 0", "arial", 30);
            strokes.PositionX = mainLayer.VisibleBoundsWorldspace.MinX + 20;
            strokes.PositionY = mainLayer.VisibleBoundsWorldspace.MaxY - 35;
            strokes.AnchorPoint = CCPoint.AnchorUpperLeft;
            strokes.Color = CCColor3B.Black;
            mainLayer.AddChild(strokes);

            ballSprite = new CCSprite("golfball");
            ballSprite.Position=mainLayer.VisibleBoundsWorldspace.Center;
            ballSprite.PositionY = 120;
            mainLayer.AddChild(ballSprite);

            isOver = false;
            Schedule(RunGameLogic);
            Schedule(ApplyDrag,0.1f);

            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = HandleTouchesEnded;
            touchListener.OnTouchesBegan = HandlesTouchesBegan;
            AddEventListener(touchListener, this);

        }

        private void HandlesTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            StartX = touches[0].LocationOnScreen.X;
            StartY = touches[0].LocationOnScreen.Y;
        }

        void HandleTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            // get x dist and y dist from last touch - first touch
            strokeCount++;
            strokes.Text = String.Format("Strokes: {0}",strokeCount);
            var lastloc = touches[0].LocationOnScreen;
            var xdist = lastloc.X - StartX;
            var ydist = lastloc.Y - StartY;
            ballXVelocity = xdist/5;
            ballYVelocity = -ydist/5;

            // Maximums
            if (ballYVelocity < -50f)
            {
                ballYVelocity = -50f;
            }
            else
            {
                if (ballYVelocity > 50f)
                {
                    ballYVelocity = 50f;
                }
            }
        }

        private void RunGameLogic(float frameTimeInSeconds)
        {
            if (isOver)
            {
                return;
            }
            ballSprite.PositionX += ballXVelocity * frameTimeInSeconds;
            ballSprite.PositionY += ballYVelocity * frameTimeInSeconds;
            var inHole = (ballSprite.BoundingBoxTransformedToParent.IntersectsRect(
                flag.BoundingBoxTransformedToParent)
             && Math.Abs(ballXVelocity) < 1 && Math.Abs(ballYVelocity) < 1);
            if (!inHole) return;
            // Display Message
            sunkit = new CCLabel(String.Format("Sunk it in {0}!",strokeCount), "arial", 50);
            sunkit.Position = mainLayer.VisibleBoundsWorldspace.Center;
            sunkit.AnchorPoint = CCPoint.AnchorMiddle;
            sunkit.Color = CCColor3B.Red;
            mainLayer.AddChild(sunkit);

            ballYVelocity = 0f;
            ballXVelocity = 0f;
            isOver = true;
        }

        // slow the ball up
        private void ApplyDrag(float frameTimeInSeconds){            
            const float drag = 0.98f;
            if (isOver)
            {
                return;
            }
            if (Math.Abs(ballYVelocity) < 0.001)
            {
                ballYVelocity = 0f;
            }
            else
            {
                ballYVelocity *= drag;
            }
            if (Math.Abs(ballXVelocity) < 0.001)
            {
                ballXVelocity = 0f;
            }
            else
            {
                ballXVelocity *= drag;
            }
        }
    }



}