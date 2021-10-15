using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KZPC1.Computers
{
    public class Computer
    {

        public Effect Effect { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Vector3 Vector1 { get; set; }
        public Vector3 Vector2 { get; set; }
        public Vector3 Vector3 { get; set; }
        public string DirSavePath { get; set; }

        public Computer(Effect effect, GraphicsDevice graphicsDevice, string dirSavePath)
        {

            Effect = effect;
            GraphicsDevice = graphicsDevice;
            SetVectors();
            DirSavePath = dirSavePath;
            SetVectorsForDraw();
        }

        public void WriteColToConsoleFromPicture(Texture2D renderT, bool hideBlack = false)
        {

            Color[] col = new Color[renderT.Height * renderT.Width];
            renderT.GetData(col);
            foreach (var c in col)
            {
                if (!hideBlack || !c.ToVector3().Equals(Color.Black.ToVector3()))
                {

                    Console.WriteLine(c.ToVector4().ToString() + " H: " + renderT.Height + " W: " + renderT.Width);
                }
            }
            Console.WriteLine("out img " + renderT.Width + " " + renderT.Height);
        }

        public virtual Vector3 AvgPhotoVecBitMap(Texture2D frontFoto)
        {
            if (frontFoto.LevelCount < 2)
                throw new ArgumentException("Jak jako maly BitMap " + nameof(frontFoto.LevelCount) + "?");

            //WriteColToConsoleFromPicture(frontFoto,true);
            Color colF = new Color();

            Color[] col = new Color[1];
            frontFoto.GetData<Color>(frontFoto.LevelCount - 1, null, col, 0, 1);
            foreach (var c in col)
            {
                //Console.WriteLine(c.ToVector4().ToString());
                colF = c;
            }

            var vec = colF.ToVector4();
            var large = frontFoto.Width * frontFoto.Height;
            var sumVec = vec * large;

            var avrVec = new Vector3(sumVec.X / sumVec.W, sumVec.Y / sumVec.W, sumVec.Z / sumVec.W);


            //Console.WriteLine("out " + colF.ToVector4().ToString());
            //frontFoto.Dispose();
            return colF.ToVector3();
        }

        public virtual void SetVectors()
        {
            Vector1 = new Vector3(0.7f, 0.2f, 0.5f);
            Vector2 = new Vector3(1f, 0.5f, 0.7f);
            Vector3 = new Vector3(0.5f, 0.7f, 0.2f);
        }

        public virtual void SetVectors(Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            Vector1 = vector1;
            Vector2 = vector2;
            Vector3 = vector3;
        }
        public virtual void SetVectorsForDraw()
        {
            var vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f));
            vertices[1] = new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f));
            vertices[2] = new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f));
            vertices[3] = new VertexPositionTexture(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f));
            vertices[4] = new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f));
            vertices[5] = new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f));

            var vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), 6, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionTexture>(vertices);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
        }
        public virtual void SaveTexture(Texture2D texture2D)
        {
            var file_path = Path.GetFullPath(DirSavePath);

            var path = Path.Combine(file_path, nameof(texture2D) + DateTime.Now.ToString("yyyymmddhhMMss") + ".png");
            texture2D.SaveAsPng(File.Create(path), texture2D.Width, texture2D.Height);

        }


        public virtual void SetEffects(int effectPass, Texture2D MainTexture, Texture2D BackGround = null, Texture2D MeansTexture = null, float? Width = null, float? Height = null, Vector3? Cent1 = null, Vector3? Cent2 = null, Vector3? Cent3 = null)
        {

            if (MainTexture == null)
                throw new NullReferenceException("Jak jako null u " + nameof(MainTexture) + "?");

            if (effectPass < 0)
                throw new ArgumentException("Jak jako zaporne " + nameof(effectPass) + "?");

            Effect.Parameters[nameof(MainTexture)].SetValue(MainTexture);

            if (BackGround != null)
                Effect.Parameters[nameof(BackGround)].SetValue(BackGround);

            if (MeansTexture != null)
                Effect.Parameters[nameof(MeansTexture)].SetValue(MeansTexture);

            if (Width.HasValue)
                Effect.Parameters[nameof(Width)].SetValue(Width.Value);

            if (Height.HasValue)
                Effect.Parameters[nameof(Height)].SetValue(Height.Value);

            if (Cent1.HasValue)
                Effect.Parameters[nameof(Cent1)].SetValue(Cent1.Value);

            if (Cent2.HasValue)
                Effect.Parameters[nameof(Cent2)].SetValue(Cent2.Value);

            if (Cent3.HasValue)
                Effect.Parameters[nameof(Cent3)].SetValue(Cent3.Value);

            Effect.CurrentTechnique.Passes[effectPass].Apply();
        }

        public virtual void SetRenderTargetAndDraw(params RenderTargetBinding[] renderTargets)
        {
            SetVectorsForDraw();
            GraphicsDevice.SetRenderTargets(renderTargets);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            GraphicsDevice.SetRenderTargets(null);
            GraphicsDevice.Clear(Color.LightGray);
        }
    }
}
