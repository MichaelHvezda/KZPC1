using Emgu.CV;
using Emgu.CV.Structure;
using KZPC1.Setting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KZPC1.Computers
{
    public class VideoComputer : ImageComputer
    {
        public VideoCapture _video { get; set; }
        public List<Texture2D> texture2DList { get; set; }
        public int textureCount { get; set; }
        public int maskUseNumber { get; set; }
        public VideoComputer(VideoCapture video, Texture2D back, Effect effect, GraphicsDevice graphicsDevice, string dirSavePath) : base(effect, back, graphicsDevice, dirSavePath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this._video = video;
            textureCount = 0;
            maskUseNumber = 1;
            var videos = new VideoCapture(Setup.GetContentFile("video.mov"));
            texture2DList = new List<Texture2D>();
            Console.Write("video transport");
            int i = 0;
            using (var img = new Image<Rgb, byte>(videos.Width, videos.Height))
            {
                while (videos.Grab())
                {
                    videos.Retrieve(img);
                    if (i % 4 == 0)
                    {
                        var text = new Texture2D(graphicsDevice, videos.Width, videos.Height);
                        Microsoft.Xna.Framework.Color[] col = GetColor(img.Bytes);
                        text.SetData<Color>(col);
                        texture2DList.Add(text);
                        Console.Write(".");
                    }
                    i++;
                    //text.Dispose();
                    //texture2D.SetData<Microsoft.Xna.Framework.Color>(col);
                    //imageComputer = new Computers.ImageComputer(Content.Load<Effect>("MyShader"), texture2D, Content.Load<Texture2D>("back"), graphics.GraphicsDevice, Const.SAVE_PATH_FILE);
                    //imageComputer.Calculate();
                }
                Console.WriteLine("! " + i + " !");
            }
            stopwatch.Stop();
            Console.WriteLine("Video init time: " + stopwatch.ElapsedMilliseconds);
        }
        //public virtual void PlayVideo(SpriteBatch spriteBatch, Rectangle windowSize)
        //{
        //    if (texture2DList.Count > textureCount)
        //    {
        //        base._foto = texture2DList[textureCount];
        //    }
        //    else
        //    {
        //        textureCount = 0;
        //        //base._foto = null;
        //    }

        //    if (_foto != null)
        //    {
        //        //_listTargets.Clear();
        //        //Calculate();
        //        SetVectorsForDraw();
        //        using (var imageMask = BufPhoto(_foto, _vector1, _vector2, _vector3, maskUseNumber))
        //        {
        //            SetVectorsForDraw();
        //            using (var image = UnionPhoto(_foto, _back, imageMask))
        //            {
        //                spriteBatch.Begin();
        //                spriteBatch.Draw(image, windowSize, Color.White);
        //                spriteBatch.End();
        //            }
        //        }
        //        Console.WriteLine("next frame pls " + textureCount);
        //    }
        //    else
        //    {
        //        Console.WriteLine("frame empty");
        //        _video.Dispose();
        //    }
        //    textureCount++;

        //}

        private Color[] GetColor(byte[] b)
        {
            Color[] c = new Color[b.Length / 3];
            for (var i = 0; b.Length > i; i = i + 3)
            {
                c[i / 3] = new Color(b[i + 2], b[i + 1], b[i]);
            }
            return c;
        }
    }
}
