using System;
using ConsoleGameEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Catmull_Rom_Splines
{
    public struct Spline
    {
        public Point[] Points;

        static Matrix<float> M = DenseMatrix.OfArray(new[,]
        {
            {0f, -1f, 2f, -1f},
            {2f, 0f, -5f, 3f},
            {0f, 1f, 4f, -3f},
            {0f, 0f, -1f, 1f}
        }) * 0.5f;
        
        private Matrix<float> P;
        private Vector<float> T;

        private void SetT(Vector<float> T)
        {
            this.T = T;
        }

        private void SetP(Matrix<float> P)
        {
            this.P = P;
        }

        public Point GetSplinePoint(float t)
        {
            int p0 = (int) t, 
                p1 = p0 + 1, 
                p2 = p1 + 1, 
                p3 = p2 + 1;

            t %= 1f;

            // TODO: fix memory allocation issue
            SetP(DenseMatrix.OfArray(new float[,]
            {
                {Points[p0].X, Points[p1].X, Points[p2].X, Points[p3].X},
                {Points[p0].Y, Points[p1].Y, Points[p2].Y, Points[p3].Y}
            }));
            SetT(DenseVector.OfArray(new[] {1f, t, t * t, t * t * t}));
            
            // Console.WriteLine(result.ToVectorString());
            return new Point((int) P.Multiply(M).Multiply(T)[0], (int) P.Multiply(M).Multiply(T)[1]);
        }
    }
}