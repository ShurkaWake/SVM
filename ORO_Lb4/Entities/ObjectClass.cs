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
        Line _hyperPlane;
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

            FillHyperPlane();
        }

        public Line HyperPlane
        {
            get => _hyperPlane;
        }

        public double MinX
        {
            get => _minX;
        }

        public double MaxX
        {
            get => _maxX;
        }

        private void FillHyperPlane()
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

            _hyperPlane = new Line(a, 1, c);
        }

        public static Line Get2ClassesHyperPlaneAsAvg(ObjectClass a, ObjectClass b)
        {
            return Line.GetMiddleLine(a.HyperPlane, b.HyperPlane);
        }

        public static Line Get2ClassesHyperPlaneAsSVM(ObjectClass a, ObjectClass b)
        {
            double minD = double.MaxValue;
            Point aPoint = new Point();
            Point bPoint = new Point();

            for(int i = 0; i < a._objects.Length; i++)
            {
                for(int j = 0; j < b._objects.Length; j++)
                {
                    if (GetDistance(a._objects[i], b._objects[j]) < minD)
                    {
                        aPoint= a._objects[i];
                        bPoint= b._objects[j];
                        minD= GetDistance(aPoint, bPoint);
                    }
                }
            }

            return FindSVM(aPoint, bPoint);
        }

        private static Line FindSVM(Point a, Point b) 
        {
            const int OneDimensionPositions = 100;
            double Kr = double.MaxValue;
            Line optimal = new Line(a, b);

            Point min = new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
            Point max = new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));

            double step_X = (max.X - min.X) / OneDimensionPositions;
            double step_Y = (max.Y - min.Y) / OneDimensionPositions;

            for(double i = min.X; i < max.X; i += step_X) 
            {
                for(double j = min.Y; j < max.Y; j += step_Y)
                {
                    for(double k = min.X; k < max.X; k += step_X)
                    {
                        for(double m = min.Y; m < max.Y; m += step_Y)
                        {
                            Line temp = new Line(new Point(i, j), new Point(k, m));
                            double Kr_temp = Math.Abs(temp.GetDistance(a) - temp.GetDistance(b));

                            if (Kr_temp < Kr)
                            {
                                optimal = temp;
                                Kr = Kr_temp;
                            }
                        }
                    }
                }
            }

            return optimal;
        }

        private static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
