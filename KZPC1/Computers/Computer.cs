using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace KZPC1.Computers
{
    public abstract class Computer
    {
        protected abstract List<RenderTarget2D> BufPhoto(Texture2D frontFoto, Vector3 vector1, Vector3 vector2, Vector3 vector3);
        protected abstract RenderTarget2D BufPhoto(Texture2D frontFoto, Vector3 vector1, Vector3 vector2, Vector3 vector3,int targetNumber);
        protected abstract void SaveTextures();
        protected abstract Vector3 AvgPhotoVec(Texture2D frontFoto, int width, int height);
        protected abstract RenderTarget2D AvgPhoto(Texture2D frontFoto, int width, int height, bool fotoDispose);
        protected abstract void Calculate();
        protected abstract void SetVectors();
        protected abstract void SetVectorsForDraw();
        protected abstract void SetVectors(Vector3 vector1, Vector3 vector2, Vector3 vector3);
        protected abstract void Draw(SpriteBatch spriteBatch, Rectangle windowSize);
        protected abstract void DrawFinal(SpriteBatch spriteBatch, Rectangle windowSize);
        protected abstract void DrawFoto(SpriteBatch spriteBatch, Rectangle windowSize);
        protected abstract void DrawBack(SpriteBatch spriteBatch, Rectangle windowSize);
        protected abstract void NextTarget();
        protected abstract void PreviousTarget();
        protected abstract RenderTarget2D ReducePhoto(Texture2D frontFoto);
        protected abstract RenderTarget2D UnionPhoto(Texture2D frontFoto, Texture2D backFoto, int centroidUsed);

    }
}
