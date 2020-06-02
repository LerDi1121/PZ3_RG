using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace PZ_RG.Service
{
    public class CameraService
    {
        private Point start = new Point();
        private Point diffOffset = new Point(); 
         private Quaternion rotationDelta;
        private Quaternion rotation;
        private int zoomMax = 10;
        private int zoomCurent = 1;
        private bool rotating;

        Window window;
        PerspectiveCamera camera;
        Viewport3D viewport;
        ScaleTransform3D scale;
        TranslateTransform3D translate;
        public CameraService(PerspectiveCamera camera , Viewport3D viewport,ScaleTransform3D scale, TranslateTransform3D translate, Window window)
        {
            this.viewport = viewport;
            this.window = window;
            this.camera = camera;
            this.scale = scale;
          this. translate = translate;
            rotationDelta = Quaternion.Identity;
            rotation = new Quaternion(0, 0, 0, 1);
            init();
        }
        void init()
        {
            viewport.MouseWheel+= MouseWheel;
            viewport.MouseLeftButtonDown += MouseLeftButtonDown;
            viewport.MouseLeftButtonUp += MouseLeftButtonUp;
            viewport.MouseMove += MouseMove;
            viewport.MouseDown += MiddleButtonDown;
            viewport.MouseUp += MiddleButtonUp;

        }


        private void MouseWheel(object sender, MouseWheelEventArgs e)//skrolovanje
        {
            Point p = e.MouseDevice.GetPosition(window);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = scale.ScaleX + 0.1;
                scaleY = scale.ScaleY + 0.1;
                zoomCurent++;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMax)
            {
                scaleX = scale.ScaleX - 0.1;
                scaleY = scale.ScaleY - 0.1;
                zoomCurent--;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
            }
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewport.CaptureMouse();
            start = e.GetPosition(window);
        
            diffOffset.X = translate.OffsetX;
            diffOffset.Y = translate.OffsetY;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewport.ReleaseMouseCapture();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (viewport.IsMouseCaptured)
            {
                var q = rotation;
                if (rotating)
                {
                   // viewport.RotateGesture= new 
                    var el = (UIElement)sender;
                    var delta = start - e.MouseDevice.GetPosition(el);
                    var mouse = new Vector3D(delta.X, -delta.Y, 0);
                    var cros = Vector3D.CrossProduct(mouse, new Vector3D(0, 0, 1));
                    var len = cros.Length;
                    if(len<1)
                    {
                        rotationDelta = new Quaternion(new Vector3D(0, 0, 1), 0);
                    }
                    else
                    {
                        rotationDelta = new Quaternion(cros, len);
                    }
                       // rotationDelta = new Quaternion(cros, len);
               
                      q = rotationDelta * rotation;
                    var mv = viewport.Children[0];
                    var t3Dg = mv.Transform as Transform3DGroup;
                    var groupScaleTransform = t3Dg.Children[0] as ScaleTransform3D;
                    var groupRotateTransform = t3Dg.Children[1] as RotateTransform3D;
                    var groupTranslateTransform = t3Dg.Children[2] as TranslateTransform3D;
                    //   groupScaleTransform.ScaleX = s;
                    //  groupScaleTransform.ScaleY = s;
                    //   groupScaleTransform.ScaleZ = s;
                    groupRotateTransform.CenterX = 5;
                    groupRotateTransform.CenterY = 5;
                    groupRotateTransform.CenterZ = 0;
                    groupRotateTransform.Rotation = new AxisAngleRotation3D(q.Axis, q.Angle);
                  //  groupTranslateTransform.OffsetX = t.X;
                  //  groupTranslateTransform.OffsetY = t.Y;
                  //  groupTranslateTransform.OffsetZ = t.Z;

                }
                else
                {
                    Point end = e.GetPosition(window);
                    double offsetX = end.X - start.X;
                    double offsetY = end.Y - start.Y;
                    double w = window.Width;
                    double h = window.Height;
                    double translateX = (offsetX * 100) / w;
                    double translateY = -(offsetY * 100) / h;
                    translate.OffsetX = diffOffset.X + (translateX / (100 * scale.ScaleX));
                    translate.OffsetY = diffOffset.Y + (translateY / (100 * scale.ScaleX));
                    translate.OffsetZ = translate.OffsetZ;
                }
            }
        }
        private void MiddleButtonDown(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton== MouseButtonState.Pressed)
            {
                rotating = true;
                viewport.CaptureMouse();
                start = e.GetPosition(window);

                diffOffset.X = translate.OffsetX;
                diffOffset.Y = translate.OffsetY;
            }
        }
        private void MiddleButtonUp(object sender, MouseEventArgs e)
        {
            if(rotating)
                rotation = rotationDelta * rotation;

            if (e.MiddleButton == MouseButtonState.Released)
            {
                rotating = false;
                viewport.ReleaseMouseCapture();
            }

        }


    }
}
