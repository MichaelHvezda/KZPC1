using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KZPC1.Computers
{
    public class ImageComputer : Computer
    {
        private int _defaulSize = 4096;
        public Texture2D Foto { get; set; }
        public Texture2D Back { get; set; }
        public int MaskUseNumber { get; set; }
        private RenderTarget2D _mask { get; set; }
        public ImageComputer(Effect effect, Texture2D foto, Texture2D back, GraphicsDevice graphicsDevice, string dirSavePath) : base(effect, graphicsDevice, dirSavePath)
        {
            Back = back;
            Foto = foto;
            MaskUseNumber = 1;
        }

        public ImageComputer(Effect effect, Texture2D back, GraphicsDevice graphicsDevice, string dirSavePath) : base(effect, graphicsDevice, dirSavePath)
        {
            Back = back;
            MaskUseNumber = 1;
        }

        //public virtual RenderTarget2D AvgPhoto(Texture2D frontFoto, int width, int height, bool fotoDispose = false)
        //{
        //    RenderTarget2D renderTarget = new RenderTarget2D(
        //    GraphicsDevice,
        //    width / 2, height / 2, true,
        //    SurfaceFormat.Color, DepthFormat.Depth16);

        //    //nastaveni shaderu
        //    Effect.Parameters["MainTexture"].SetValue(frontFoto);
        //    Effect.Parameters["width"].SetValue((float)width);
        //    Effect.Parameters["height"].SetValue((float)height);
        //    Effect.CurrentTechnique.Passes[2].Apply();


        //    _graphicsDevice.SetRenderTarget(renderTarget);
        //    //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

        //    _graphicsDevice.SetRenderTargets(null);
        //    _graphicsDevice.Clear(new Color(1, 1, 1, 1));

        //    /*
        //    Console.WriteLine("Level is " + renderTarget.LevelCount);
        //    Color[] col = new Color[1];
        //    renderTarget.GetData(renderTarget.LevelCount-1, null,col,0,1);
        //    foreach (var c in col)
        //    {
        //        Console.WriteLine(c.ToVector4().ToString() + " !!!!!!!!!!!");
        //    }
        //    */
        //    /*if (renderTarget.Height < 100)
        //    {

        //        WriteColToConsoleFromPicture(renderTarget);
        //    }*/
        //    //Console.WriteLine("Height is " + (height / 2) + " and width is " + (width / 2));
        //    if (fotoDispose)
        //    {
        //        frontFoto.Dispose();
        //    }
        //    if (height > 2 && width > 2)
        //    {
        //        return AvgPhoto(renderTarget, width / 2, height / 2, true);
        //    }
        //    else
        //    {
        //        return renderTarget;
        //    }
        //}

        //public virtual Vector3 AvgPhotoVec(Texture2D frontFoto, int width, int height)
        //{
        //    Color colF = new Color();
        //    using (var jjo = AvgPhoto(frontFoto, width, height))
        //    {
        //        Color[] col = new Color[jjo.Height * jjo.Width];
        //        jjo.GetData<Color>(col);
        //        foreach (var c in col)
        //        {
        //            //Console.WriteLine(c.ToVector4().ToString());
        //            colF = c;
        //        }
        //    }
        //    //Console.WriteLine("out " + colF.ToVector4().ToString());
        //    //frontFoto.Dispose();
        //    return colF.ToVector3();
        //}

        public virtual void BufPhotoMain()
        {
            if (_mask != null)
                _mask.Dispose();

            _mask = BufPhoto();
        }

        public virtual RenderTarget2D BufPhoto()
        {
            RenderTarget2D renderTarget1, renderTarget2, renderTarget3;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget1 = new RenderTarget2D(
            GraphicsDevice,
            _defaulSize, _defaulSize, true,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget2 = new RenderTarget2D(
            GraphicsDevice,
            _defaulSize, _defaulSize, true,
            SurfaceFormat.Color, DepthFormat.Depth16);

            renderTarget3 = new RenderTarget2D(
            GraphicsDevice,
            _defaulSize, _defaulSize, true,
            SurfaceFormat.Color, DepthFormat.Depth16);
            GraphicsDevice.BlendState = BlendState.Opaque;

            //nastaveni shaderu
            SetEffects(0, MainTexture: Foto, Cent1: Vector1, Cent2: Vector2, Cent3: Vector3);

            SetRenderTargetAndDraw(renderTarget1, renderTarget2, renderTarget3);

            //return new List<RenderTarget2D> { renderTarget1, renderTarget2, renderTarget3 };
            //var v1 = AvgPhotoVec(renderTarget1, renderTarget1.Width, renderTarget1.Height);
            //var v2 = AvgPhotoVec(renderTarget2, renderTarget2.Width, renderTarget2.Height);
            //var v3 = AvgPhotoVec(renderTarget3, renderTarget3.Width, renderTarget3.Height);

            var v1 = AvgPhotoVecBitMap(renderTarget1);
            var v2 = AvgPhotoVecBitMap(renderTarget2);
            var v3 = AvgPhotoVecBitMap(renderTarget3);

            //jestli se jiz centroidy stabilizovali
            if (!Vector1.Equals(v1) && !Vector2.Equals(v2) && !Vector3.Equals(v3))
            {
                renderTarget1.Dispose();
                renderTarget2.Dispose();
                renderTarget3.Dispose();
                Console.WriteLine("v1 " + v1.ToString() + " v2 " + v2.ToString() + " v3 " + v3.ToString());
                SetVectors(v1, v2, v3);
                return BufPhoto();
            }
            else
            {
                SetVectors(v1, v2, v3);
                if (MaskUseNumber == 1)
                {
                    renderTarget2.Dispose();
                    renderTarget3.Dispose();
                    return renderTarget1;
                }
                if (MaskUseNumber == 2)
                {
                    renderTarget1.Dispose();
                    renderTarget3.Dispose();
                    return renderTarget2;
                }
                if (MaskUseNumber == 3)
                {
                    renderTarget1.Dispose();
                    renderTarget2.Dispose();
                    return renderTarget3;
                }
            }
            renderTarget1.Dispose();
            renderTarget2.Dispose();
            renderTarget3.Dispose();
            return new RenderTarget2D(GraphicsDevice, _defaulSize, _defaulSize);
        }

        //public virtual void Calculate()
        //{
        //    SetVectorsForDraw();
        //    //_listTargets.Clear();
        //    _listTargets = BufPhoto(_foto, _vector1, _vector2, _vector3);
        //    //SaveTextures();
        //}

        public virtual void DrawMask(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            if (_mask == null)
            {
                throw new ArgumentException("Jak jako null " + nameof(_mask) + "?");
            }
            else
            {
                spriteBatch.Draw(_mask, windowSize, Color.White);
            }
            spriteBatch.End();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle windowSize, Texture2D texture2D)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture2D, windowSize, Color.White);
            spriteBatch.End();
        }

        public virtual void DrawBack(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Back, windowSize, Color.White);
            spriteBatch.End();
        }

        public virtual void DrawFoto(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Foto, windowSize, Color.White);
            spriteBatch.End();
        }


        //public virtual RenderTarget2D ReducePhoto(Texture2D frontFoto)
        //{
        //    RenderTarget2D renderTarget;

        //    //nastavení objektu ktery bude počitan na shaderu
        //    renderTarget = new RenderTarget2D(
        //    _graphicsDevice,
        //    frontFoto.Width, frontFoto.Height, false,
        //    SurfaceFormat.Color, DepthFormat.Depth16);

        //    //nastaveni shaderu
        //    _effect.Parameters["MainTexture"].SetValue(frontFoto);
        //    _effect.CurrentTechnique.Passes[1].Apply();

        //    _graphicsDevice.SetRenderTarget(renderTarget);
        //    //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

        //    _graphicsDevice.SetRenderTargets(null);

        //    return renderTarget;
        //}

        public virtual RenderTarget2D UnionPhoto()
        {
            RenderTarget2D renderTarget;

            //nastavení objektu ktery bude počitan na shaderu
            renderTarget = new RenderTarget2D(
            GraphicsDevice,
            _defaulSize, _defaulSize, false,
            SurfaceFormat.Color, DepthFormat.Depth16);

            //nastaveni shaderu
            SetEffects(3, MainTexture: Foto, BackGround: Back, MeansTexture: _mask);

            SetRenderTargetAndDraw(renderTarget);
            return renderTarget;
        }

        public virtual void DrawFinal(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            SetVectorsForDraw();
            using (var image = UnionPhoto())
            {
                spriteBatch.Begin();
                spriteBatch.Draw(image, windowSize, Color.White);
                spriteBatch.End();
            }
        }
        public virtual void DrawFinalMain(SpriteBatch spriteBatch, Rectangle windowSize)
        {
            SetVectorsForDraw();
            BufPhotoMain();
            SetVectorsForDraw();
            using (var image = UnionPhoto())
            {
                spriteBatch.Begin();
                spriteBatch.Draw(image, windowSize, Color.White);
                spriteBatch.End();
            }
        }

        //public virtual RenderTarget2D BufPhoto(Texture2D frontFoto, Vector3 vector1, Vector3 vector2, Vector3 vector3, int targetNumber)
        //{
        //    RenderTarget2D renderTarget1, renderTarget2, renderTarget3;

        //    //nastavení objektu ktery bude počitan na shaderu
        //    renderTarget1 = new RenderTarget2D(
        //    _graphicsDevice,
        //    4096, 4096, false,
        //    SurfaceFormat.Color, DepthFormat.Depth16);

        //    renderTarget2 = new RenderTarget2D(
        //    _graphicsDevice,
        //    4096, 4096, false,
        //    SurfaceFormat.Color, DepthFormat.Depth16);

        //    renderTarget3 = new RenderTarget2D(
        //    _graphicsDevice,
        //    4096, 4096, false,
        //    SurfaceFormat.Color, DepthFormat.Depth16);
        //    _graphicsDevice.BlendState = BlendState.Opaque;
        //    //nastaveni shaderu
        //    _effect.Parameters["MainTexture"].SetValue(frontFoto);
        //    _effect.Parameters["cent1"].SetValue(vector1);
        //    _effect.Parameters["cent2"].SetValue(vector2);
        //    _effect.Parameters["cent3"].SetValue(vector3);
        //    _effect.CurrentTechnique.Passes[0].Apply();
        //    _graphicsDevice.SetRenderTargets(renderTarget1, renderTarget2, renderTarget3);
        //    //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

        //    _graphicsDevice.SetRenderTargets(null);
        //    _graphicsDevice.Clear(new Color(1, 1, 1, 1));
        //    var v1 = AvgPhotoVec(renderTarget1, renderTarget1.Width, renderTarget1.Height);
        //    var v2 = AvgPhotoVec(renderTarget2, renderTarget2.Width, renderTarget2.Height);
        //    var v3 = AvgPhotoVec(renderTarget3, renderTarget3.Width, renderTarget3.Height);

        //    if (!vector1.Equals(v1) && !vector2.Equals(v2) && !vector3.Equals(v3))
        //    {
        //        renderTarget1.Dispose();
        //        renderTarget2.Dispose();
        //        renderTarget3.Dispose();
        //        return BufPhoto(frontFoto, v1, v2, v3, targetNumber);
        //    }
        //    else
        //    {
        //        //frontFoto.Dispose();
        //        //SetVectors(v1, v2, v3);
        //        if (targetNumber == 1)
        //        {
        //            renderTarget2.Dispose();
        //            renderTarget3.Dispose();
        //            return renderTarget1;
        //        }
        //        if (targetNumber == 2)
        //        {
        //            renderTarget1.Dispose();
        //            renderTarget3.Dispose();
        //            return renderTarget2;
        //        }
        //        if (targetNumber == 3)
        //        {
        //            renderTarget1.Dispose();
        //            renderTarget2.Dispose();
        //            return renderTarget3;
        //        }
        //    }
        //    renderTarget1.Dispose();
        //    renderTarget2.Dispose();
        //    renderTarget3.Dispose();
        //    return new RenderTarget2D(_graphicsDevice, frontFoto.Width, frontFoto.Height);
        //}

        //public RenderTarget2D UnionPhoto(Texture2D frontFoto, Texture2D backFoto, Texture2D centroid)
        //{
        //    RenderTarget2D renderTarget;

        //    //nastavení objektu ktery bude počitan na shaderu
        //    renderTarget = new RenderTarget2D(
        //    _graphicsDevice,
        //    frontFoto.Width, frontFoto.Height, false,
        //    SurfaceFormat.Color, DepthFormat.Depth16);

        //    //nastaveni shaderu
        //    _effect.Parameters["MainTexture"].SetValue(frontFoto);
        //    _effect.Parameters["BackGround"].SetValue(backFoto);
        //    _effect.Parameters["MeansTexture"].SetValue(centroid);
        //    _effect.CurrentTechnique.Passes[3].Apply();

        //    _graphicsDevice.SetRenderTarget(renderTarget);
        //    //ukazani na obrazek odkud pokud se bude spracovavat, podstatě rohy obrazku
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        //    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 3, 5);

        //    _graphicsDevice.SetRenderTargets(null);
        //    _graphicsDevice.Clear(new Color(1, 1, 1, 1));
        //    //frontFoto.Dispose();
        //    return renderTarget;
        //}



    }
}
