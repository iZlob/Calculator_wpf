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

namespace Calculator_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<Key, string> dic = new Dictionary<Key, string>()
        {
                { Key.Delete, "CE" },
                { Key.Back, "<" },
                { Key.Enter, "=" },
                { Key.OemPeriod, "." },
                { Key.Oem2, "/" },
                { Key.Multiply, "*" },
                { Key.OemPlus, "=" },
                { Key.OemMinus, "-" },
                { Key.D0, "0" },
                { Key.D1, "1" },
                { Key.D2, "2" },
                { Key.D3, "3" },
                { Key.D4, "4" },
                { Key.D5, "5" },
                { Key.D6, "6" },
                { Key.D7, "7" },
                { Key.D8, "8" },
                { Key.D9, "9" }
        };
        public MainWindow()
        {
            InitializeComponent();           
        }

        private void ClickButton(object sender, RoutedEventArgs args)
        {
            Button button = sender as Button;
            string str = button.Content.ToString();

            Calculate(str);
        }

        private void PressKey(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            string str = "";

            foreach (var el in dic)
                if (el.Key == key)
                    str = el.Value;

            if (str == "CE" && tb2.Text == "") 
                str = "C";

            if (e.Key == Key.D8 && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                str = "*";

            if (e.Key == Key.OemPlus && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                str = "+";

            Calculate(str);
        }

        private void Calculate(string str)
        {
            switch (str)
            {
                case "CE":
                    {
                        tb2.Text = "";
                        break;
                    }
                case "C":
                    {
                        tb1.Text = "";
                        tb2.Text = "";
                        break;
                    }
                case "<":
                    {
                        if (tb1.Text != "")
                            tb1.Text = tb1.Text.Substring(0, tb1.Text.Length - 1);
                        break;
                    }
                case "=":
                    {
                        if (tb1.Text == "") break;
                        if (!(tb1.Text[tb1.Text.Length - 1] == '-' || tb1.Text[tb1.Text.Length - 1] == '+' ||
                              tb1.Text[tb1.Text.Length - 1] == '/' || tb1.Text[tb1.Text.Length - 1] == '*' ||
                              tb1.Text[tb1.Text.Length - 1] == '='))
                        {
                            tb2.Text = new System.Data.DataTable().Compute(tb1.Text, null).ToString();
                            tb1.Text += str;
                        }
                        break;
                    }
                default:
                    {
                        if (str == ".")
                        {
                            if (tb1.Text == "") break;
                            else
                            {
                                char last_ch = tb1.Text[tb1.Text.Length - 1];
                                if (last_ch == '+' || last_ch == '-' || last_ch == '/' || last_ch == '*' || last_ch == '=')
                                    break;                            
                            }
                            int dot = 0;
                            int znak = 0;
                            foreach (var ch in tb1.Text)
                            {
                                if (ch == '.')
                                    dot++;
                                if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                                    znak++;
                            }

                            if (znak != dot) break;
                        }

                        if (str == "+" || str == "-" || str == "/" || str == "*")
                        {
                            if (tb1.Text == "") break;

                            if (tb1.Text != "" && tb1.Text[tb1.Text.Length - 1] == '=')
                            {
                                for (int i = 0; i < tb2.Text.Length; i++)
                                    if (tb2.Text[i] == ',')
                                    {
                                        tb2.Text = tb2.Text.Remove(i, 1);
                                        tb2.Text = tb2.Text.Insert(i, ".");
                                    }
                                tb1.Text = tb2.Text + str;
                                tb2.Text = "";
                                break;
                            }

                            if (tb1.Text != "" && (tb1.Text[tb1.Text.Length - 1] == '+' || tb1.Text[tb1.Text.Length - 1] == '-' ||
                                                     tb1.Text[tb1.Text.Length - 1] == '/' || tb1.Text[tb1.Text.Length - 1] == '*'))
                            {
                                tb1.Text = tb1.Text.Substring(0, tb1.Text.Length - 1);
                                tb1.Text += str;
                                break;
                            }
                        }

                        if (tb1.Text != "" && tb1.Text[tb1.Text.Length - 1] == '=')
                        {
                            tb1.Text = str;
                            tb2.Text = "";
                            break;
                        }

                        if (tb1.Text != "" && str == "0" && tb1.Text[0] == '0') break;

                        if (tb1.Text != "" && tb1.Text[0] == '0')
                        {
                            tb1.Text = str;
                            break;
                        }

                        tb1.Text += str;

                        if (tb1.Text != "" && (tb1.Text[tb1.Text.Length - 1] != '-' && tb1.Text[tb1.Text.Length - 1] != '+' &&
                                               tb1.Text[tb1.Text.Length - 1] != '/' && tb1.Text[tb1.Text.Length - 1] != '*'))
                            tb2.Text = new System.Data.DataTable().Compute(tb1.Text, null).ToString();
                        break;
                    }
            }
        }
    }
}
