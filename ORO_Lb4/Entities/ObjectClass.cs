using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ORO_Lb4.Entities
{
    internal record ObjectClass
    {
        Point[] _objects;
        Line _hyperSquare;
        double _minX = double.MaxValue;
        double _maxX = double.MinValue;

        public ObjectClass(Point[] objects) 
        {
            _objects = objects;

            foreach(var p in _objects)
            {
                if (p.X > _maxX)
                {
                    _maxX = p.X;
                }
                if(p.X < _minX)
                {
                    _minX = p.X;
                }
            }

            FillHyperSquare();
        }

        public Line HyperSquare
        {
            get => _hyperSquare;
        }

        public double MinX
        {
            get => _minX;
        }

        public double MaxX
        {
            get => _maxX;
        }

       /* private void FillHyperSquare()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0 },
                { 0, 0 }
            });

            Matrix<double> B = DenseMatrix.OfArray(new double[,]
            {
                { 0 },
                { 0 },
            });

            foreach(var obj in _objects)
            {
                A[0, 0] = Math.Pow(obj.X, 2);
                A[0, 1] += obj.X;
                A[1, 0] += obj.X;

                B[0, 0] += obj.X * obj.Y;
                B[1, 0] += obj.Y;
            }
            A[1, 1] = _objects.Length;

            var res = A.Inverse() * B;
            _hyperSquare = new Line(res[0, 0], 1, res[1, 0]);
        }*/

        private void FillHyperSquare()
        {
            double sumXY = 0;
            double sumX = 0;
            double sumY = 0;
            double sumSquareX = 0;

            foreach(var obj in _objects)
            {
                sumXY += obj.X * obj.Y;
                sumX += obj.X;
                sumY += obj.Y;
                sumSquareX += obj.X * obj.X;
            }

            double a = (_objects.Length * sumXY - sumX * sumY)
                     / (_objects.Length * sumSquareX - sumX * sumX);

            double cSum = 0;
            foreach(var obj in _objects)
            {
                cSum += obj.Y - a * obj.X;
            }
            double c = cSum / _objects.Length;

            _hyperSquare = new Line(a, 1, c);
        }
    }
}
