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

namespace Books_01._10._2018
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            BooksCheck.RefreshPictures();
            WorkShop.Front_End(_LiVi, ActualHeight);
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BooksCheck.WorkWS(_LiVi, ActualHeight, ActualWidth);
        }

    }
}
