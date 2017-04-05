using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace lab1._2.Shapes
{
    [DataContract(Name = "Figure")]
    [KnownType(typeof(myFigure))]
    [KnownType(typeof(myCircle))]
    [KnownType(typeof(myEllipse))]
    [KnownType(typeof(myLine))]
    [KnownType(typeof(myRectangle))]
    [KnownType(typeof(mySquare))]
    [KnownType(typeof(myTriangle))]
    public abstract class myFigure
    {
        [DataMember]
        public Point p1;
        [DataMember]
        public System.Windows.Media.Color color;

        public abstract void draw(System.Windows.Controls.Canvas canvas, int index);
        public abstract String getName();

        public abstract bool isSelected { get; set; }
    }
}