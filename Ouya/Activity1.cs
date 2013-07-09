using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Ouya.Console.Api;

namespace FontBuddySample.Ouya
{
	[Activity (Label = "FontBuddySample.Ouya", 
	           MainLauncher = true,
	           Icon = "@drawable/icon",
	           Theme = "@style/Theme.Splash",
               AlwaysRetainTaskState=true,
	           LaunchMode=LaunchMode.SingleInstance,
	           ScreenOrientation = ScreenOrientation.SensorLandscape,
	           ConfigurationChanges = ConfigChanges.Orientation | 
	                                  ConfigChanges.KeyboardHidden | 
	                                  ConfigChanges.Keyboard)]
	[IntentFilter(new[] { Intent.ActionMain }
            , Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		public const string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

		public static OuyaFacade PurchaseFacade = null;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create our OpenGL view, and display it
			Game1.Activity = this;
			var g = new Game1();
			SetContentView(g.Window);
			g.Run();

			byte[] applicationKey = null;
			using (var stream = Resources.OpenRawResource(Resource.Raw.key))
			{
				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					applicationKey = ms.ToArray();
				}
			}

			PurchaseFacade = OuyaFacade.Instance;
			PurchaseFacade.Init(this, DEVELOPER_ID, applicationKey);
		}
	}
}


