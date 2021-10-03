﻿using System;
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

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CBRService cbr = new CBRService();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = cbr;
        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            cbr.AsyncGetCurrencyRateOnDate(DateTime.Now, "USD");
        }
    }
}