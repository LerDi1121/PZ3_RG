using System.Windows.Media.Media3D;
using MS.Internal.ComponentModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using RG_PZ2.Models;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;

namespace PZ_RG.Service
{

    static class Object3DService
    {
        const  double HEIGHT =0.06;
        public static GeometryModel3D Create3Delement(PowerEntity entity, Model3DGroup model3DGroup, double size=HEIGHT)
        {
            double   entityZ = 0.1;
            var mesh = new MeshGeometry3D();
            Point3DCollection PositionCollection = new Point3DCollection();
            PositionCollection.Add(new Point3D(entity.X, entity.Y, entityZ));//donji deo
            PositionCollection.Add(new Point3D(entity.X+size, entity.Y, entityZ));
            PositionCollection.Add(new Point3D(entity.X + size, entity.Y + size, entityZ));
            PositionCollection.Add(new Point3D(entity.X, entity.Y + size, entityZ));
            PositionCollection.Add(new Point3D(entity.X, entity.Y, entityZ + size));//gornji deo
            PositionCollection.Add(new Point3D(entity.X + size, entity.Y, entityZ + size));
            PositionCollection.Add(new Point3D(entity.X + size, entity.Y + size, entityZ + size));
            PositionCollection.Add(new Point3D(entity.X, entity.Y + size, entityZ + size));

            mesh.Positions = PositionCollection;
            foreach (var item in model3DGroup.Children)
            {
                while (mesh.Bounds.IntersectsWith(item.Bounds))//da li je neki obj u "dodiru sa tim nasim"
                {
                    for (var i = 0; i < mesh.Positions.Count; i++)
                    {
                        mesh.Positions[i] = new Point3D(mesh.Positions[i].X, mesh.Positions[i].Y, mesh.Positions[i].Z + HEIGHT);//ako jeste stavi ga iznad njega, tj povecaj z osu 
                    }
                }
            }
            Int32Collection TriangleCollection = new Int32Collection();
            //dno
            TriangleCollection.Add(1);
            TriangleCollection.Add(0);
            TriangleCollection.Add(3);
            TriangleCollection.Add(1);
            TriangleCollection.Add(3);
            TriangleCollection.Add(2);
            //plafon XD
            TriangleCollection.Add(5);
            TriangleCollection.Add(6);
          TriangleCollection.Add(7);
          TriangleCollection.Add(5);
          TriangleCollection.Add(7);
          TriangleCollection.Add(4);
            //stranica1
            TriangleCollection.Add(1);
           TriangleCollection.Add(2);
           TriangleCollection.Add(5);
           TriangleCollection.Add(5);
           TriangleCollection.Add(2);
           TriangleCollection.Add(6);
            //stranica2
            TriangleCollection.Add(0);
            TriangleCollection.Add(1);
            TriangleCollection.Add(4);
            TriangleCollection.Add(1);
            TriangleCollection.Add(5);
            TriangleCollection.Add(4);
            // stranica3
            TriangleCollection.Add(0);
           TriangleCollection.Add(4);
           TriangleCollection.Add(3);
           TriangleCollection.Add(4);
           TriangleCollection.Add(7);
           TriangleCollection.Add(3);
            //stranica4
            TriangleCollection.Add(7);
            TriangleCollection.Add(6);
            TriangleCollection.Add(2);
            TriangleCollection.Add(7);
            TriangleCollection.Add(2);
            TriangleCollection.Add(3);

            mesh.TriangleIndices = TriangleCollection;

            var material = new DiffuseMaterial(entity.SetDefaultColor());
    
            var model = new GeometryModel3D(mesh, material);
            model.SetValue(FrameworkElement.TagProperty, entity);

                       return model;
        }
        public static HashSet<GeometryModel3D> CreateLines(LineEntity entity)
        {
            HashSet<GeometryModel3D> Line = new HashSet<GeometryModel3D>();
            for(int i =0; i<entity.Vertices.Count-1;i++)
            {
                Line.Add(CreateLine(entity.Vertices[i], entity.Vertices[i + 1], Brushes.Red));
            }
            return Line;
            


        }
        private static GeometryModel3D CreateLine(Point3D start, Point3D end, Brush brush)
        {
            double size = HEIGHT /2;
            double entityZ = 0.03;
            var mesh = new MeshGeometry3D();
            Point3DCollection PositionCollection = new Point3DCollection();
            PositionCollection.Add(new Point3D(start.X, start.Y, entityZ));//donji deo
            PositionCollection.Add(new Point3D(start.X + size, start.Y, entityZ));
            PositionCollection.Add(new Point3D(end.X , end.Y, entityZ));
            PositionCollection.Add(new Point3D(end.X +size, end.Y, entityZ));
            mesh.Positions = PositionCollection;

            Int32Collection TriangleCollection = new Int32Collection();
            //u jednom smeru
            TriangleCollection.Add(0);
            TriangleCollection.Add(2);
            TriangleCollection.Add(1);
            TriangleCollection.Add(2);
            TriangleCollection.Add(3);
            TriangleCollection.Add(1);
            //u drugom smeru
            TriangleCollection.Add(0);
            TriangleCollection.Add(1);
            TriangleCollection.Add(2);
            TriangleCollection.Add(2);
            TriangleCollection.Add(1);
            TriangleCollection.Add(3);
            mesh.TriangleIndices = TriangleCollection;

            var material = new DiffuseMaterial(brush);

            var model = new GeometryModel3D(mesh, material);
            return model;
        }
    }
}
