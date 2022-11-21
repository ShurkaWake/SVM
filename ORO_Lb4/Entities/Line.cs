using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORO_Lb4.Entities
{
    public record Line
    {
        double _a;
        double _b;
        double _c;

        public Line(Point first, Point second)
        {
            if (first.Y == second.Y)
            {
                _a = 0;
                _b = -1;
                _c = first.Y;
            }
            else
            {
                _b = -1;
                _a = (second.Y - first.Y) / (second.X - first.X);
                _c = second.Y - (second.X * (second.Y - first.Y)) / (second.X - first.X);
            }
            
        }

        public Line(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public double A { get => _a; }
        public double B { get => _b; }
        public double C { get => _c; }

        public static Line GetMiddleLine(Line first, Line second)
        {
            double a = (first.A + second.A) / 2.0;
            double b = (first.B + second.B) / 2.0;
            double c = (first.C + second.C) / 2.0;
            return new Line(a, b, c);
        }

        public double GetVectDistance(Point p, bool flag)
        {
            if (flag)
            {
                if (p.Y > (A / -B) * p.X + (C / -B))
                {
                    return GetDistance(p);
                }
                else
                {
                    return -GetDistance(p);
                }
            }
            else
            {
                if (p.Y > (A / -B) * p.X + (C / -B))
                {
                    return -GetDistance(p);
                }
                else
                {
                    return +GetDistance(p);
                }
            }
        }

        public double GetDistance(Point p)
        {
            return Math.Abs(A * p.X + B * p.Y + C)
                         / Math.Sqrt(A * A + B * B); 
        }
    }
}
