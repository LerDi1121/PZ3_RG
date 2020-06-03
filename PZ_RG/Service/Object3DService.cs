using System.Windows.Media.Media3D;
using MS.Internal.ComponentModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using RG_PZ2.Models;

namespace PZ_RG.Service
{

    static class Object3DService
    {
        const  double HEIGHT =0.06;
        public static GeometryModel3D Create3Delement(PowerEntity entity, Model3DGroup model3DGroup)
        {
            double halfSize = HEIGHT / 2;
            var retVal = new GeometryModel3D();

            var mesh = new MeshGeometry3D();

            mesh.Positions.Add(new Point3D(entity.X - halfSize, entity.Y - halfSize, HEIGHT - halfSize)); // dole levo
            mesh.Positions.Add(new Point3D(entity.X + halfSize, entity.Y - halfSize, HEIGHT - halfSize)); // dole desno
            mesh.Positions.Add(new Point3D(entity.X - halfSize, entity.Y + halfSize, HEIGHT - halfSize)); // gore levo
            mesh.Positions.Add(new Point3D(entity.X + halfSize, entity.Y + halfSize, HEIGHT - halfSize)); // gore desno
            mesh.Positions.Add(new Point3D(entity.X - halfSize, entity.Y - halfSize, HEIGHT + halfSize)); // dole levo dalje
            mesh.Positions.Add(new Point3D(entity.X + halfSize, entity.Y - halfSize, HEIGHT + halfSize)); // dole desno dalje
            mesh.Positions.Add(new Point3D(entity.X - halfSize, entity.Y + halfSize, HEIGHT + halfSize)); // gore levo dalje
            mesh.Positions.Add(new Point3D(entity.X + halfSize, entity.Y + halfSize, HEIGHT + halfSize)); // gore desno dalje

            foreach (var item in model3DGroup.Children)
            {
                while (mesh.Bounds.IntersectsWith(item.Bounds))
                {
                    for (var i = 0; i < mesh.Positions.Count; i++)
                    {
                        mesh.Positions[i] = new Point3D(mesh.Positions[i].X, mesh.Positions[i].Y, mesh.Positions[i].Z + HEIGHT);
                    }
                }
            }

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(1);


            var material = new DiffuseMaterial(entity.SetDefaultColor());
    
            var model = new GeometryModel3D(mesh, material);
            model.SetValue(FrameworkElement.TagProperty, entity);


            return model;
        }
    }
}
