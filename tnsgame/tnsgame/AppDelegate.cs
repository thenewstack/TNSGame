using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Foundation;
using UIKit;

namespace TNSGame
{

    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var application = new CCApplication();
            application.ApplicationDelegate = new GameAppDelegate();
            application.StartGame();
            return true;
        }
    }
}