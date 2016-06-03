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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RealMode.IsChecked = true;
            Slider.Value = 10;
        }

        Cntrl Controler = new Cntrl(Mode.Real);

        bool[] activeButtons = new bool[16];

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var tag = e.OriginalSource.ToString().Split(' ').ToList();
            if (tag.Count != 2) return;
            ChangeMemoryStatus(tag[1]);
            Controler.DoCommand(tag[1]);
            Print();
        }

        public void Print()
        {
            preHistoryText.Content = Controler.preH;
            editableNumberText.Content = Controler.editable;
        }

        private void ChangeMemoryStatus(string tag)
        {
            switch (tag)
            {
                case "MS":
                case "M+":
                case "M-":
                    isMem.Visibility = Visibility.Visible;
                    break;
                case "MC":
                    isMem.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi == null) return;
            mi.IsChecked = true;
            switch (mi.Header.ToString())
            {
                case "Вещественные числа":
                    ComplexMode.IsChecked = false;
                    FracMode.IsChecked = false;
                    Controler = new Cntrl(Mode.Real);
                    Dot.Content = ",";
                    Sqrt.Visibility = Visibility.Visible;
                    ComplexSeparate.Visibility = Visibility.Hidden;
                    break;
                case "Простые дроби":
                    ComplexMode.IsChecked = false;
                    RealMode.IsChecked = false;
                    Controler = new Cntrl(Mode.Frac);
                    Sqrt.Visibility = Visibility.Hidden;
                    ComplexSeparate.Visibility = Visibility.Hidden;
                    Dot.Content = "/";
                    break;
                case "Комплексные числа":
                    RealMode.IsChecked = false;
                    FracMode.IsChecked = false;
                    Controler = new Cntrl(Mode.Complex);
                    Dot.Content = ",";
                    ComplexSeparate.Visibility = Visibility.Visible;
                    Sqrt.Visibility = Visibility.Hidden;
                    break;
            }
            isMem.Visibility = Visibility.Hidden;
            Print();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Controler.Base = Convert.ToInt32(Slider.Value);

            activeButtons.Initialize();

            for (var i = 0; i < activeButtons.Length; ++i)
                activeButtons[i] = false;
            for (var i = Convert.ToInt32(Slider.Value); i < activeButtons.Length; ++i)
                activeButtons[i] = true;
            Null.IsEnabled = !activeButtons[0];
            One.IsEnabled = !activeButtons[1];
            Two.IsEnabled = !activeButtons[2];
            Three.IsEnabled = !activeButtons[3];
            Four.IsEnabled = !activeButtons[4];
            Five.IsEnabled = !activeButtons[5];
            Six.IsEnabled = !activeButtons[6];
            Seven.IsEnabled = !activeButtons[7];
            Eight.IsEnabled = !activeButtons[8];
            Nine.IsEnabled = !activeButtons[9];
            A.IsEnabled = !activeButtons[10];
            B.IsEnabled = !activeButtons[11];
            C.IsEnabled = !activeButtons[12];
            D.IsEnabled = !activeButtons[13];
            E.IsEnabled = !activeButtons[14];
            F.IsEnabled = !activeButtons[15];

            Print();
        }

        private void CtrlC_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(editableNumberText.Content.ToString());
        }
        private void CtrlV_Click(object sender, RoutedEventArgs e)
        {
            Controler.SetClipboard(Clipboard.GetText());
            Print();
        }
    }
}
