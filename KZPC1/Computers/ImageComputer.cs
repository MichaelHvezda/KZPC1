using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KZPC1.Computers
{
    public class ImageComputer : Computer
    {
        public Effect _effect { get; set; }
        public Texture2D _foto { get; set; }
        public Texture2D _back { get; set; }
        public GraphicsDevice _graphicsDevice { get; set; }
        public Vector3 _vector1 { get; set; }
        public Vector3 _vector2 { get; set; }
        public Vector3 _vector3 { get; set; }
        public string _dirSavePath { get; set; }
        public List<RenderTarget2D> _listTargets { get; set; }
        public int _imageNumber { get; set; }
        public ImageComputer(Effect effect, Texture2D foto, Texture2D back, GraphicsDevice graphicsDevice, string dirSavePath)
        {

            _effect = effect;
            _foto = foto;
            _graphicsDevice = graphicsDevice;
            SetVectors();
            _dirSavePath = dirSavePath;
            SetVectorsForDraw();
            _listTargets = new List<RenderTarget2D>();
            _imageNumber = 0;
        }

        public ImageComputer(Effect effect, Texture2D back, GraphicsDevice graphicsDevice, string dirSavePath)
        {
            _back = back;
            _effect = effect;
            _graphicsDevice = graphicsDevice;
            SetVectors();
            _dirSavePath = dirSavePath;
            SetVectorsForDraw();
            _listTargets = new List<RenderTarget2D>();
        }

        protected override RenderTarget2D AvgPhoto(Texture2D frontFoto, int width, int height,bool fotoDispose = false)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
            _graphicsDevice,
            width / 2, height / 2, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.Parameters["width"].SetValue((float)width);
            _effect.Parameters["height"].SetValue((float)height);
            _effect.CurrentTechnique.Passes[2].Apply();


            _graphicsDevice.SetRenderTarget(renderTarget);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);
            _graphicsDevice.Clear(new Color(1, 1, 1, 1));
            //Console.WriteLine("Height is " + (height / 2) + " and width is " + (width / 2));
            if (fotoDispose)
            {
                frontFoto.Dispose();
            }
            if (height > 2 && width > 2)
            {
                return AvgPhoto(renderTarget, width / 2, height / 2, true);
            }
            else
            {
                return renderTarget;
            }
        }

        protected override Vector3 AvgPhotoVec(Texture2D frontFoto, int width, int height)
        {
            Color colF = new Color();
            using (var jjo = AvgPhoto(frontFoto, width, height))
            {
                Color[] col = new Color[jjo.Height * jjo.Width];
                jjo.GetData<Color>(col);
                foreach (var c in col)
                {
                    //Console.WriteLine(c.ToVector4().ToString());
                    colF = c;
                }
            }
            //frontFoto.Dispose();
            return colF.ToVector3();
        }

        protected override List<RenderTarget2D> BufPhoto(Texture2D frontFoto, Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            RenderTarget2D renderTarget1, renderTarget2, renderTarget3;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget1 = new RenderTarget2D(
            _graphicsDevice,
            4096, 4096, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget2 = new RenderTarget2D(
            _graphicsDevice,
            4096, 4096, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget3 = new RenderTarget2D(
            _graphicsDevice,
            4096, 4096, false,
            SurfaceFormat.Color, DepthFormat.Depth16);
            _graphicsDevice.BlendState = BlendState.Opaque;
            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.Parameters["cent1"].SetValue(vector1);
            _effect.Parameters["cent2"].SetValue(vector2);
            _effect.Parameters["cent3"].SetValue(vector3);
            _effect.CurrentTechnique.Passes[0].Apply();
            _graphicsDevice.SetRenderTargets(renderTarget1, renderTarget2, renderTarget3);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);
            _graphicsDevice.Clear(new Color(1, 1, 1, 1));
            //return new List<RenderTarget2D> { renderTarget1, renderTarget2, renderTarget3 };
            var v1 = AvgPhotoVec(renderTarget1, renderTarget1.Width, renderTarget1.Height);
            var v2 = AvgPhotoVec(renderTarget2, renderTarget2.Width, renderTarget2.Height);
            var v3 = AvgPhotoVec(renderTarget3, renderTarget3.Width, renderTarget3.Height);

            if (!vector1.Equals(v1) && !vector2.Equals(v2) && !vector3.Equals(v3))
            {
                //renderTarget1.Dispose();
                //renderTarget2.Dispose();
                //renderTarget3.Dispose();
                Console.WriteLine("v1 " + v1.ToString() + " v2 " + v2.ToString() + " v3 " + v3.ToString());
                return BufPhoto(frontFoto, v1, v2, v3);
            }
            else
            {
                SetVectors(v1, v2, v3);
                return new List<RenderTarget2D> { renderTarget1, renderTarget2, renderTarget3 };
            }
        }

        protected override void Calculate()
        {
            SetVectorsForDraw();
            //_listTargets.Clear();
            _listTargets = BufPhoto(_foto, _vector1, _vector2, _vector3);
            //SaveTextures();
        }

        protected override void Draw(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            if (_listTargets.Count != 0)
            {
                spriteBatch.Draw(_listTargets[_imageNumber], windowSize, Color.White);
            }
            spriteBatch.End();
        }

        protected override void DrawBack(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_back, windowSize, Color.White);
            spriteBatch.End();
        }

        protected override void DrawFoto(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_foto, windowSize, Color.White);
            spriteBatch.End();
        }

        protected override void NextTarget()
        {
            _imageNumber++;
            if (_listTargets.Count <= _imageNumber)
            {
                _imageNumber = 0;
            }
        }

        protected override void PreviousTarget()
        {
            _imageNumber--;
            if (0 > _imageNumber)
            {
                _imageNumber = _listTargets.Count - 1;
            }
        }

        protected override void SaveTextures()
        {
            var file_path = Path.GetFullPath(_dirSavePath);
            int i = 0;
            foreach (var t in _listTargets)
            {
                var path = Path.Combine(file_path, i.ToString() + DateTime.Now.ToString("yyyymmddhhMMss") + ".png");
                t.SaveAsPng(File.Create(path), t.Width, t.Height);
                i++;
            }
        }

        protected override void SetVectors()
        {
            _vector1 = new Vector3(0.7f, 0.2f, 0.5f);
            _vector2 = new Vector3(1f, 0.5f, 0.7f);
            _vector3 = new Vector3(0.5f, 0.7f, 0.2f);
        }

        protected override void SetVectors(Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            _vector1 = vector1;
            _vector2 = vector2;
            _vector3 = vector3;
            Console.WriteLine("SET v1 " + vector1.ToString() + " v2 " + vector2.ToString() + " v3 " + vector3.ToString());
        }

        protected override void SetVectorsForDraw()
        {
            var vertices = new VertexPositionTexture[6];
            vertices[0] = new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f));
            vertices[1] = new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f));
            vertices[2] = new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f));
            vertices[3] = new VertexPositionTexture(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f));
            vertices[4] = new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f));
            vertices[5] = new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f));

            var vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionTexture), 6, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionTexture>(vertices);
            _graphicsDevice.SetVertexBuffer(vertexBuffer);
        }

        protected override RenderTarget2D ReducePhoto(Texture2D frontFoto)
        {
            RenderTarget2D renderTarget;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.CurrentTechnique.Passes[1].Apply();

            _graphicsDevice.SetRenderTarget(renderTarget);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);

            return renderTarget;
        }

        protected override RenderTarget2D UnionPhoto(Texture2D frontFoto, Texture2D backFoto, int centroidUsed)
        {
            RenderTarget2D renderTarget;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.Parameters["BackGround"].SetValue(backFoto);
            _effect.Parameters["MeansTexture"].SetValue(_listTargets[centroidUsed]);
            _effect.CurrentTechnique.Passes[3].Apply();

            _graphicsDevice.SetRenderTarget(renderTarget);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);
            _graphicsDevice.Clear(new Color(1, 1, 1, 1));
            frontFoto.Dispose();
            return renderTarget;
        }

        protected override void DrawFinal(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            SetVectorsForDraw();
            using (var image = UnionPhoto(_foto, _back, _imageNumber))
            {
                spriteBatch.Begin();
                if (_listTargets.Count != 0)
                {
                    spriteBatch.Draw(image, windowSize, Color.White);
                }
                spriteBatch.End();
            }
        }

        protected override RenderTarget2D BufPhoto(Texture2D frontFoto, Vector3 vector1, Vector3 vector2, Vector3 vector3, int targetNumber)
        {
            RenderTarget2D renderTarget1, renderTarget2, renderTarget3;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget1 = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget2 = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget3 = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);
            _graphicsDevice.BlendState = BlendState.Opaque;
            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.Parameters["cent1"].SetValue(vector1);
            _effect.Parameters["cent2"].SetValue(vector2);
            _effect.Parameters["cent3"].SetValue(vector3);
            _effect.CurrentTechnique.Passes[0].Apply();
            _graphicsDevice.SetRenderTargets(renderTarget1, renderTarget2, renderTarget3);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);
            _graphicsDevice.Clear(new Color(1, 1, 1, 1));
            var v1 = AvgPhotoVec(renderTarget1, renderTarget1.Width, renderTarget1.Height);
            var v2 = AvgPhotoVec(renderTarget2, renderTarget2.Width, renderTarget2.Height);
            var v3 = AvgPhotoVec(renderTarget3, renderTarget3.Width, renderTarget3.Height);

            if (!vector1.Equals(v1) && !vector2.Equals(v2) && !vector3.Equals(v3))
            {
                renderTarget1.Dispose();
                renderTarget2.Dispose();
                renderTarget3.Dispose();
                return BufPhoto(frontFoto, v1, v2, v3, targetNumber);
            }
            else
            {
                //frontFoto.Dispose();
                //SetVectors(v1, v2, v3);
                if (targetNumber == 1)
                {
                    renderTarget2.Dispose();
                    renderTarget3.Dispose();
                    return renderTarget1;
                }
                if (targetNumber == 2)
                {
                    renderTarget1.Dispose();
                    renderTarget3.Dispose();
                    return renderTarget2;
                }
                if (targetNumber == 3)
                {
                    renderTarget1.Dispose();
                    renderTarget2.Dispose();
                    return renderTarget3;
                }
            }
            renderTarget1.Dispose();
            renderTarget2.Dispose();
            renderTarget3.Dispose();
            return new RenderTarget2D(_graphicsDevice, frontFoto.Width, frontFoto.Height);
        }

        protected  RenderTarget2D UnionPhoto(Texture2D frontFoto, Texture2D backFoto, Texture2D centroid)
        {
            RenderTarget2D renderTarget;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget = new RenderTarget2D(
            _graphicsDevice,
            frontFoto.Width, frontFoto.Height, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            //nastaveni shaderu
            _effect.Parameters["MainTexture"].SetValue(frontFoto);
            _effect.Parameters["BackGround"].SetValue(backFoto);
            _effect.Parameters["MeansTexture"].SetValue(centroid);
            _effect.CurrentTechnique.Passes[3].Apply();

            _graphicsDevice.SetRenderTarget(renderTarget);
            //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

            _graphicsDevice.SetRenderTargets(null);
            _graphicsDevice.Clear(new Color(1, 1, 1, 1));
            //frontFoto.Dispose();
            return renderTarget;
        }
    }
}
