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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace PZ_RG.Service
{/// <summary>
/// Promena pozicije i ugla kamere i scene
/// </summary>
    public class CameraService
    {
        private Point start = new Point();
        private Point diffOffset = new Point(); 
         private Quaternion rotationDelta;
        private Quaternion rotation;
        private int zoomMaxIn = 16;
        private int zoomMaxOut =- 8; 
        private int zoomCurent = 1;
        private bool rotating;
        Window window;
        Viewport3D viewport;
        ScaleTransform3D scale;
        TranslateTransform3D translate;
        AxisAngleRotation3D rotateX;
        AxisAngleRotation3D rotateY;
        public CameraService( Viewport3D viewport,ScaleTransform3D scale, TranslateTransform3D translate, AxisAngleRotation3D rotateX, AxisAngleRotation3D rotateY, Window window)
        {
            this.viewport = viewport;
            this.window = window;
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
   
        private void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleX = 1;
            double scaleY = 1;
            double scaleZ = 1;
            if (e.Delta > 0 && zoomCurent < zoomMaxIn)
            {
                scaleX = scale.ScaleX + 0.1;
                scaleY = scale.ScaleY + 0.1;
                scaleZ = scale.ScaleZ + 0.1;
                scale.CenterX = 5;
                scale.CenterY = 5;
                scale.CenterZ = 0;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
                scale.ScaleZ = scaleZ;
                zoomCurent++;

            }
            else if (e.Delta <= 0 && zoomCurent > zoomMaxOut)
            {
                scaleX = scale.ScaleX - 0.1;
                scaleY = scale.ScaleY - 0.1;
                scaleZ = scale.ScaleZ - 0.1;
                scale.CenterX = 5;
                scale.CenterY = 5;
                scale.CenterZ = 0;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
                scale.ScaleZ = scaleZ;
                zoomCurent--;

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
                    if (-75 < angleY && angleY <75)
                    {
                        rotateY.Angle = angleY;
                    }
                    if (-20 < angleX && angleX < 125)
                    {
                        rotateX.Angle = angleX;
                    }

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
/********
 *Selenic Branislav PR132/2016
 ********/
