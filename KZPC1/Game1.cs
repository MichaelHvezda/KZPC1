using FFMediaToolkit;
using HeyRed.ImageSharp.AVCodecFormats;
using HeyRed.ImageSharp.AVCodecFormats.Avi;
using HeyRed.ImageSharp.AVCodecFormats.Mov;
using KZPC1.Setting;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MonoGame.Extended.VideoPlayback;
using Emgu.CV;
using Emgu.CV.Structure;
//using Video = MonoGame.Extended.Framework.Media.Video;
//using VideoPlayer = MonoGame.Extended.Framework.Media.VideoPlayer;

namespace KZPC1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState state;
        KeyboardState previousState;
        Microsoft.Xna.Framework.Rectangle windowSize = new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480);
        public List<RenderTarget2D> listTargets = new List<RenderTarget2D>();
        Computers.ImageComputer imageComputer;
        Computers.VideoComputer videoComputer;
        bool play = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            FFmpegLoader.FFmpegPath = @"C:\Users\micha\OneDrive\Desktop\KZPC1\KZPC1\binnaries";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var setup = new Setup();
            setup.SetupAll();
            //setup.DelAllPic();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            videoComputer = new Computers.VideoComputer(new VideoCapture(Setup.GetContentFile("video.mov")), Content.Load<Texture2D>("back"), Content.Load<Effect>("MyShader"), graphics.GraphicsDevice, Const.SAVE_PATH_FILE);

            //imageComputer = new Computers.ImageComputer(Content.Load<Effect>("MyShader"), Content.Load<Texture2D>("foto"), Content.Load<Texture2D>("back"), graphics.GraphicsDevice, Const.SAVE_PATH_FILE);
            //var sda = Content.Load<Video>("video");
            //videoComputer = new Computers.VideoComputer(Content.Load<Video>("video"), Content.Load<Effect>("MyShader"), graphics.GraphicsDevice, Const.SAVE_PATH_FILE);
            ////imageComputer.Calculate();
            //Video video = Content.Load<Video>("video");
            //VideoPlayer videoPlayer = new VideoPlayer();
            //videoPlayer.Play(video);
            //videoPlayer.GetTexture();
            //imageComputer.Calculate();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            state = Keyboard.GetState();
            // TODO: Add your update logic here
            //if (state.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right))
            //{
            //    videoComputer.NextTarget();
            //    Console.WriteLine("Next img");
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left))
            //{
            //    videoComputer.PreviousTarget();
            //    Console.WriteLine("Previous img");
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
            //{
            //    Console.WriteLine("<<<<<-----Calculate------>>>>>>>");
            //    videoComputer.Calculate();
            //    Console.WriteLine("Calculate");
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.S) && !previousState.IsKeyDown(Keys.S))
            //{

            //    videoComputer.SaveTextures();
            //    Console.WriteLine("save");
            //}
            if (state.IsKeyDown(Keys.NumPad1) && !previousState.IsKeyDown(Keys.NumPad1))
            {
                videoComputer.maskUseNumber = 1;
                Console.WriteLine("Next img");
            }
            if (state.IsKeyDown(Keys.NumPad2) && !previousState.IsKeyDown(Keys.NumPad2))
            {
                videoComputer.maskUseNumber = 2;
                Console.WriteLine("Next img");
            }
            if (state.IsKeyDown(Keys.NumPad3) && !previousState.IsKeyDown(Keys.NumPad3))
            {
                videoComputer.maskUseNumber = 3;
                Console.WriteLine("Next img");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && !previousState.IsKeyDown(Keys.D))
            {

                play = !play;
                Console.WriteLine("play");
            }
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            previousState = state;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.LightGray);
            if (!play)
            {
                videoComputer.PlayVideo(spriteBatch, windowSize);
            }

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
        public void OnResize(Object sender, EventArgs e)
        {
            windowSize = Window.ClientBounds;
            windowSize.X = 0;
            windowSize.Y = 0;
            //Console.WriteLine("Resize on " + windowSize.ToString());
        }
    }

}
