using RG_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PZ_RG.Service
{/// <summary>
/// HitTest
/// Prikaz toolTip-a
/// promena boja odredjenih cvorova koje povezuje odredjen vod
/// </summary>
    class HitTestService
    {
        private ToolTip toolTipForAll = new ToolTip();
        Window window;
        Model3DGroup model3DGroup;
        Viewport3D viewport;
        HashSet<GeometryModel3D> previusModels;
        public HitTestService(Viewport3D viewpor, Model3DGroup model3DGroup, Window window)
        {
            previusModels = new HashSet<GeometryModel3D>();
            this.viewport = viewpor;
            this.window = window;
            this.model3DGroup = model3DGroup;
            viewpor.MouseLeftButtonDown += MouseLeftButtonDown;
            CreateToolTip();
        }
        private HitTestResultBehavior HitResult(HitTestResult result)
        {
            var hitResult = result as RayHitTestResult;
            var value = hitResult?.ModelHit.GetValue(FrameworkElement.TagProperty);
            if (value is PowerEntity)
            {
                toolTipForAll.Content = value.ToString();
                toolTipForAll.IsOpen = true;
            }
            if (value is LineEntity)
            {
                RestoreColor();
                var line = value as LineEntity;
                var a = hitResult.ModelHit as GeometryModel3D;
                var first = model3DGroup.Children.FirstOrDefault(item => (item.GetValue(FrameworkElement.TagProperty) as PowerEntity)?.Id == line.FirstEnd);
                var second = model3DGroup.Children.FirstOrDefault(item => (item.GetValue(FrameworkElement.TagProperty) as PowerEntity)?.Id == line.SecondEnd);
                var first3D = (first as GeometryModel3D);
                var second3D = (second as GeometryModel3D);
                if (first3D is object)
                {
                    first3D.Material = new DiffuseMaterial(Brushes.Magenta);
                    previusModels.Add(first3D);
                }
                if (second3D is object)
                {
                    second3D.Material = new DiffuseMaterial(Brushes.Magenta);
                    previusModels.Add(second3D);
                }
            }
            return HitTestResultBehavior.Stop;
        }
        private void RestoreColor()
        {
            foreach (var item in previusModels)
            {
                PowerEntity value = (PowerEntity)item.GetValue(FrameworkElement.TagProperty);
                item.Material = new DiffuseMaterial(value.SetDefaultColor());
            }
            previusModels.Clear();
        }
        private void CreateToolTip()
        {
            toolTipForAll.StaysOpen = false;
            toolTipForAll.IsOpen = false;
            toolTipForAll.Background = Brushes.Beige;
            toolTipForAll.BorderBrush = Brushes.Black;
            toolTipForAll.Foreground = Brushes.DarkSlateGray;
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            toolTipForAll.IsOpen = false;
            var mouseposition = e.GetPosition(viewport);
            var testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            var testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            var pointparams = new PointHitTestParameters(mouseposition);
            var rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            VisualTreeHelper.HitTest(viewport, null, HitResult, pointparams);
        }
    }
}
/********
 *Selenic Branislav PR132/2016
 ********/
