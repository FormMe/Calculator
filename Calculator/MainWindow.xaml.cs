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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.Key.ToString();
            if (k.Contains("NumPad"))
                Controler.DoCommand(k.Substring(6, 1));
            else
            {
                if (k.Contains("D") && k.Length == 2)
                    Controler.DoCommand(k.Substring(1, 1));
                else
                {
                    if (k.Length == 1 || k == "Subtract" || k == "Add" || k == "Multiply" || k == "Divide" || k == "Decimal" || k == "Back" || k == "Delete")
                        Controler.DoCommand(k);
                    else
                        if (k == "Enter" || k == "Return")
                       Controler.DoCommand(k);
                }
            }
            e.Handled = true;
            UpColumnItem.Focus();
            Print();
        }

        private void Print()
        {
            var column = Controler.preH;

            if (column?.Item1 != null)
                UpColumnItem.Content = column.Item1;
            else UpColumnItem.Content = "";

            if (column?.Item2 != null)
            {
                DownColumnItem.Content = column.Item2;
                if (column?.Item3 != null)
                    DownColumnItem.Content += "  " + column.Item3;
            }
            else DownColumnItem.Content = "";


            editableNumberText.Content = Controler.editable;
            editableNumberText_ContentChanged();
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
            Slider.Value = 10;
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

        private void editableNumberText_ContentChanged()
        {
            if (string.IsNullOrEmpty(editableNumberText.Content?.ToString())) return;
            var len = editableNumberText.Content.ToString().Length;
            if (len < 15)
                editableNumberText.FontSize = 32;
            else if (len < 18)
                editableNumberText.FontSize = 30;
            else if (len < 21)
                editableNumberText.FontSize = 28;
            else if (len < 24)
                editableNumberText.FontSize = 26;
            else
                editableNumberText.FontSize = 24;
        }
    }
}
