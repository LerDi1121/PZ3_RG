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
        AxisAngleRotation3D rotateX;
        AxisAngleRotation3D rotateY;
        public CameraService(PerspectiveCamera camera , Viewport3D viewport,ScaleTransform3D scale, TranslateTransform3D translate, AxisAngleRotation3D rotateX, AxisAngleRotation3D rotateY, Window window)
        {
            this.viewport = viewport;
            this.window = window;
            this.camera = camera;
            this.scale = scale;
          this. translate = translate;
            rotationDelta = Quaternion.Identity;
            this.rotateX = rotateX;
            this.rotateY = rotateY;
            this.rotation = new Quaternion(0, 0, 0, 1);
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
                Point end = e.GetPosition(window);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                double w = window.Width;
                double h = window.Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                var q = rotation;
                if (rotating)
                {
                    var angleY = (rotateY.Angle + -translateX) % 360;
                    var angleX = (rotateX.Angle + translateY) % 360;
                  //  if (-65 < angleY && angleY < 65)
                   // {
                        rotateY.Angle = angleY;
                    //}
                   // if (-65 < angleX && angleX < 65)
                   // {
                        rotateX.Angle = angleX;
                   // }
                    start = end;

                }
                else
                {
                    
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
