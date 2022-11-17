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
            _b = 1;
            _a = (second.Y - first.Y) / (second.X - first.X);
            _c = -(second.X * (second.Y - first.Y)) / (second.X - first.X) + second.Y;
        }

        private Line(double a, double b, double c)
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
            double a = Math.Tan((Math.Atan(first.A) + Math.Atan(second.A)) / 2.0);
            double b = 1;
            double c = (first.C + second.C) / 2.0;
            return new Line(a, b, c);
        }
    }
}
