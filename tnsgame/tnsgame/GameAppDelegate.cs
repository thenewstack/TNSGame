using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Foundation;
using UIKit;

namespace TNSGame
{
    class GameAppDelegate : CCApplicationDelegate
    {

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            var bounds = mainWindow.WindowSizeInPixels;
            CCScene.SetDefaultDesignResolution(bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";

            var gameScene = new GameScene(mainWindow);
            mainWindow.RunWithScene(gameScene);         
        }
    }
}