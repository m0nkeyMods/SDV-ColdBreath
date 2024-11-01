using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using static StardewValley.Minigames.BoatJourney;
using xTile;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;

public class Breath
{
    public float X;
    public float Y;
    // Maybe should be 0-1
    public float opacity;
}

namespace ColdBreath
{

    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        // Copied a lot of stuff from https://gitlab.com/speeder1/SMAPIHealthbarMod/-/blob/master/SMAPIHealthBarMod/HealthBarMod.cs?ref_type=heads
        int tick = 0;
        private Texture2D Pixel;
        private List<Breath> breaths = new List<Breath>();

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Display.RenderingHud += this.GraphicsEvents_DrawTick;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game is drawing to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GraphicsEvents_DrawTick(object sender, RenderingHudEventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.currentMinigame != null || Game1.activeClickableMenu != null)
                return;
            if (this.Pixel == null)
                this.Pixel = this.GetPixel();

            Vector2 playerLocation = Game1.player.getStandingPosition();

            // Load your image asset
            Texture2D image = this.Helper.ModContent.Load<Texture2D>("assets/coldbreath.png");

            // Get the dimensions of the game window
            int screenWidth = Game1.viewport.Width;
            int screenHeight = Game1.viewport.Height;

            // Calculate the center position of the screen
            int centerX = screenWidth / 2 - 100; // Adjust for half the width of the image
            int centerY = screenHeight / 2 - 100; // Adjust for half the height of the image

            foreach (Breath breath in breaths)
            {
                // Draw the image in the center of the screen (UI space)
                e.SpriteBatch.Draw(
                    image,
                    new Rectangle(centerX, centerY, 50, 50), // Adjust size if needed
                    Color.White
                );

                // Subtract from opacity as time goes on
            }

            if (tick < 1000)
            {
                tick++;
            }
            else
            {
                tick = 0;
                this.Monitor.Log($"Adding breath at {playerLocation.X}, {playerLocation.Y}", LogLevel.Info);
                breaths.Add(new Breath()
                {
                    X = playerLocation.X,
                    Y = playerLocation.Y,
                    opacity = 1
                });
            }
        }

        /// <summary>Get a blank pixel.</summary>
        private Texture2D GetPixel()
        {
            Texture2D pixel = new(Game1.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            return pixel;
        }
    }
}