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
    /// <summary>
    /// Kreiranje objekata za prikaz
    /// Kreiranje "Kocki"
    /// Kreiranje linija za vodove
    /// </summary>
    static class Object3DService
    {
        const  double HEIGHT =0.06;
        public static GeometryModel3D Create3Delement(PowerEntity entity, Model3DGroup model3DGroup, double size=HEIGHT)
        {
            double   entityZ = 0.04;
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
                while (mesh.Bounds.IntersectsWith(item.Bounds))//da li je neki obj u "dodiru" sa tim nasim
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
            model.SetValue(FrameworkElement.TagProperty, entity);//dodavanje ojekta za" posle"

                       return model;
        }
        public static HashSet<GeometryModel3D> CreateLines(LineEntity entity)
        {
            HashSet<GeometryModel3D> Line = new HashSet<GeometryModel3D>();
            for(int i =0; i<entity.Vertices.Count-1;i++)
            {
                Line.Add(CreateLine(entity.Vertices[i], entity.Vertices[i + 1], CreateBrush(entity.ConductorMaterial),entity));
            }
            return Line;


        }
        private static Brush CreateBrush(string material)
        {
            switch(material)
            {
                case "Steel":
                return Brushes.LightPink;
                case "Copper":
                    return Brushes.Gold;
                case "Acsr":
                    return Brushes.LightSlateGray;
                default :
                return Brushes.Black;
               
            }
        }
        private static GeometryModel3D CreateLine(Point3D start, Point3D end, Brush brush, LineEntity entity)
        {
            double size = HEIGHT /2;
            double entityZ = 0.03;
            var mesh = new MeshGeometry3D();
            Point3DCollection PositionCollection = new Point3DCollection();
            PositionCollection.Add(new Point3D(start.X, start.Y, entityZ));//donji deo
            PositionCollection.Add(new Point3D(start.X + size, start.Y, entityZ));
            PositionCollection.Add(new Point3D(end.X , end.Y, entityZ));
            PositionCollection.Add(new Point3D(end.X +size, end.Y, entityZ));
            PositionCollection.Add(new Point3D(start.X +(size/2), start.Y, entityZ+size));
            PositionCollection.Add(new Point3D(end.X +(size/2), end.Y, entityZ+size));
            mesh.Positions = PositionCollection;
            //pravlljenje prizme za liniju, da bi se videla iz svakog ugla 
            //problem je nastao sto se prilikom rotacije neke linije vodova pojavljuju/nestaju (renderuju)
            //zbog ugla normale elementa i normale kamere...
            //resenje sam nasao u pravljenu "duplog" elementa za prikaz, tako se u svakom trenutku rendenuje vod, tj prizma
            Int32Collection TriangleCollection = new Int32Collection();
            //u jednom smeru
            TriangleCollection.Add(0);
            TriangleCollection.Add(2);
            TriangleCollection.Add(4);

            TriangleCollection.Add(4);
            TriangleCollection.Add(2);
            TriangleCollection.Add(5);
            
            TriangleCollection.Add(2);
            TriangleCollection.Add(3);
            TriangleCollection.Add(5);
            
            TriangleCollection.Add(1);
            TriangleCollection.Add(3);
            TriangleCollection.Add(5);
            
            TriangleCollection.Add(4);
            TriangleCollection.Add(5);
            TriangleCollection.Add(1);
            
            TriangleCollection.Add(1);
            TriangleCollection.Add(4);
            TriangleCollection.Add(0);
            //u drugom smeru zbog ugla gledanja
            TriangleCollection.Add(0);
            TriangleCollection.Add(4);
            TriangleCollection.Add(2);

            TriangleCollection.Add(4);
            TriangleCollection.Add(5);
            TriangleCollection.Add(2);

            TriangleCollection.Add(2);
            TriangleCollection.Add(5);
            TriangleCollection.Add(3);

            TriangleCollection.Add(1);
            TriangleCollection.Add(5);
            TriangleCollection.Add(3);

            TriangleCollection.Add(4);
            TriangleCollection.Add(1);
            TriangleCollection.Add(5);

            TriangleCollection.Add(1);
            TriangleCollection.Add(0);
            TriangleCollection.Add(4);

            mesh.TriangleIndices = TriangleCollection;

            var material = new DiffuseMaterial(brush);

            var model = new GeometryModel3D(mesh, material);
            model.SetValue(FrameworkElement.TagProperty, entity);
            return model;
        }
    }
}
/********
 *Selenic Branislav PR132/2016
 ********/
