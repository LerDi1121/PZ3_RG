using PZ_RG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Selenic Branislav PR132/2016
//korisceni modeli za pz2
namespace PZ_RG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CameraService cameraService;
        HitTestService hitTestService;
        public MainWindow()
        {
            InitializeComponent();
            cameraService = new CameraService(mainViewport,scale,translate,rotateX,rotateY,this);
            Common.LoadModels();
            Common.ConverLatLon();
            Common.CreateElement(model3DGroup);
            hitTestService = new HitTestService(mainViewport,model3DGroup,this);
        }
    
    }
}   
        /********
         *Selenic Branislav PR132/2016
         ********/
