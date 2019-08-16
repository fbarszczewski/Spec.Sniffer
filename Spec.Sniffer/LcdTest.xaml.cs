using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Spec.Sniffer
{
    /// <summary>
    ///     Interaction logic for LcdTest.xaml
    /// </summary>
    public partial class LcdTest : Window
    {
        public LcdTest()
        {
            InitializeComponent();
            BackgroundGrid.DataContext = this;


            BackgroundGridColors.Color = Color.FromRgb(255, 255, 255);
        }


        private void BackgroundChange()
        {
            if (BackgroundGridColors.Color == Color.FromRgb(255, 255, 255))
                BackgroundGridColors.Color = Color.FromRgb(255, 0, 0);
            else if (BackgroundGridColors.Color == Color.FromRgb(255, 0, 0))
                BackgroundGridColors.Color = Color.FromRgb(0, 255, 0);
            else if (BackgroundGridColors.Color == Color.FromRgb(0, 255, 0))
                BackgroundGridColors.Color = Color.FromRgb(0, 0, 255);
            else if (BackgroundGridColors.Color == Color.FromRgb(0, 0, 255))
                BackgroundGridColors.Color = Color.FromRgb(0, 0, 0);
            else
                BackgroundGridColors.Color = Color.FromRgb(255, 255, 255);
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    BackgroundChange();
                    break;

                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}