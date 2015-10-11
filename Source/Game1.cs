using FontBuddyLib;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FontBuddySample
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		#region Properties

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		GameClock CurrentTime;

		List<IFontBuddy> buddies = new List<IFontBuddy>();

		BouncyNumbers bounce;

		private NumberBuddy num;

		private const int start = 0;
		private const int end = 4000;

		#endregion //Properties

		#region Methods

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
			Content.RootDirectory = "Content";

#if ANDROID
			graphics.PreferredBackBufferWidth = 853;
			graphics.PreferredBackBufferHeight = 480;
#else
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
#endif

			CurrentTime = new GameClock();

			//buddies.Add(new FontBuddy());
			buddies.Add(new ShadowTextBuddy());
			buddies.Add(new WrongTextBuddy());
			buddies.Add(new ShakyTextBuddy());
			buddies.Add(new OppositeTextBuddy());
			buddies.Add(new RainbowTextBuddy());

			bounce = new BouncyNumbers()
			{
				Rescale = 1f
			};
			bounce.Start(start, end);
			buddies.Add(bounce);

			num = new NumberBuddy();
			buddies.Add(num);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			foreach (IFontBuddy myBuddy in buddies)
			{
				myBuddy.LoadContent(Content, "TestFont");
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			CurrentTime.Update(gameTime);

			//restart the bounby numbers
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				bounce.Start(start, end);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				num.Add(100);
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Z))
			{
				num.Add(-100);
			}

			// TODO: Add your update logic here
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//get the start point
			Rectangle screen = graphics.GraphicsDevice.Viewport.TitleSafeArea;
			Vector2 position = new Vector2(screen.Left + 32, screen.Top + 32);

			string test = "FontBuddy!";

			//draw all those fonts
			foreach (IFontBuddy myBuddy in buddies)
			{
				//draw the left justified text
				myBuddy.Write(test, position, Justify.Left, 1.0f, Color.White, spriteBatch, CurrentTime);

				//draw the centered text
				position.X = screen.Center.X;
				myBuddy.Write(test, position, Justify.Center, 1.0f, Color.White, spriteBatch, CurrentTime);

				//draw the right justified text
				position.X = screen.Right - 32f;
				myBuddy.Write(test, position, Justify.Right, 1.0f, Color.White, spriteBatch, CurrentTime);

				//move to the start point for the next font
				position.X = 32f;
				position.Y += myBuddy.Font.MeasureString(test).Y;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}

