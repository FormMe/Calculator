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
        }

        Cntrl Controler = new Cntrl(Mode.Real);

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var tag = e.OriginalSource.ToString().Split(' ').ToList();
            if (tag.Count != 2) return;

            Controler.DoCommand(tag[1]);
            preHistoryText.Content = Controler.preH;
            editableNumberText.Content = Controler.editable;




            //if (tag[1] != "Преобразовать")
            //{
            //    NumberText.Text = res;
            //    if (tag[1] == "Clear")
            //        Result.Content = res;
            //}
            //else
            //    Result.Content = res;
        }

        private void MenuItemWithRadioButtons_Click(object sender, System.Windows.RoutedEventArgs e)
        {
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

                    break;
                case "Простые дроби":
                    ComplexMode.IsChecked = false;
                    RealMode.IsChecked = false;
                    Controler = new Cntrl(Mode.Frac);

                    break;
                case "Комплексные числа":
                    RealMode.IsChecked = false;
                    FracMode.IsChecked = false;
                    Controler = new Cntrl(Mode.Complex);

                    break;


            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Controler.Base = Convert.ToInt32(Slider.Value);
        }

    }
}
