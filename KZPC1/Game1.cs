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
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        KeyboardState State;
        KeyboardState PreviousState;
        bool play = false;
        Microsoft.Xna.Framework.Rectangle WindowSize = new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480);
        Computers.ImageComputer ImageComputer;
        Computers.VideoComputer VideoComputer;
        Computers.Computer Computer;
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.ApplyChanges();
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ImageComputer = new Computers.ImageComputer(Content.Load<Effect>("MyShader"), Content.Load<Texture2D>("foto"), Content.Load<Texture2D>("back"), Graphics.GraphicsDevice, Const.SAVE_PATH_FILE);
            //ImageComputer.SetEffects(0,MainTexture: Content.Load<Texture2D>("foto"));

            //videoComputer = new Computers.VideoComputer(new VideoCapture(Setup.GetContentFile("video.mov")), Content.Load<Texture2D>("back"), Content.Load<Effect>("MyShader"), graphics.GraphicsDevice, Const.SAVE_PATH_FILE);
            //videoComputer.AvgPhotoVecBitMap(Content.Load<Texture2D>("back"));
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
            State = Keyboard.GetState();
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
            if (State.IsKeyDown(Keys.NumPad1) && !PreviousState.IsKeyDown(Keys.NumPad1))
            {
                //videoComputer.maskUseNumber = 1;
                ImageComputer.MaskUseNumber = 1;
                Console.WriteLine("Next mask 1");
            }
            if (State.IsKeyDown(Keys.NumPad2) && !PreviousState.IsKeyDown(Keys.NumPad2))
            {
                //videoComputer.maskUseNumber = 2;
                ImageComputer.MaskUseNumber = 2;
                Console.WriteLine("Next mask 2");
            }
            if (State.IsKeyDown(Keys.NumPad3) && !PreviousState.IsKeyDown(Keys.NumPad3))
            {
                //videoComputer.maskUseNumber = 3;
                ImageComputer.MaskUseNumber = 3;
                Console.WriteLine("Next mask 3");
            }
            //if (Keyboard.GetState().IsKeyDown(Keys.D) && !previousState.IsKeyDown(Keys.D))
            //{

            //    play = !play;
            //    Console.WriteLine("play");
            //}
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            PreviousState = State;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.LightGray);
            if (!play)
            {
                ImageComputer.DrawFinalMain(SpriteBatch, WindowSize);
            }

            stopwatch.Stop();
            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
            base.Draw(gameTime);
        }
        public void OnResize(Object sender, EventArgs e)
        {
            WindowSize = Window.ClientBounds;
            WindowSize.X = 0;
            WindowSize.Y = 0;
            //Console.WriteLine("Resize on " + windowSize.ToString());
        }
    }

}
