using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Books_01._10._2018
{
    static class WorkShop
    {
        public static void Front_End(ListBox LiVi, double ActualHeight)
        {
            int PictureKey = 0;
            string _HtmlText = BooksCheck.GetUrl("http://www.avidreaders.ru/books/");
            BooksCheck.GetImages(_HtmlText);
            Dictionary<int, string> DGD = BooksCheck.GetDescription(_HtmlText);
            Dictionary<int, string> DGN = BooksCheck.GetBookName(_HtmlText);

            int MaxIdnexCheck = DGD.Count % 2;

            for (int i = 0; i < (DGD.Count/2) + MaxIdnexCheck; ++i)
            {
                Grid grid = new Grid();
                for (int _co = 0; _co < 30; ++_co)
                {
                    ColumnDefinition ColD = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(ColD);
                    if (_co < 20)
                    {
                        RowDefinition RowD = new RowDefinition();
                        grid.RowDefinitions.Add(RowD);
                    }
                }
                grid.ShowGridLines = false;

                ++PictureKey;
                string BuffOne = BooksCheck.GetPicturePath(PictureKey);
                Rectangle RImage = new Rectangle();
                RImage.Fill = new ImageBrush(new BitmapImage(new Uri(BuffOne, UriKind.Relative)));


                ToolTip toolTipOne = new ToolTip();

                StackPanel toolTipPanelOne = new StackPanel();
                toolTipPanelOne.Children.Add(new TextBlock { Text = DGN[PictureKey], FontSize = 16 });
                toolTipOne.Content = toolTipPanelOne;
                RImage.ToolTip = toolTipOne;
                string BufferOne = DGN[PictureKey];
                string _KFOne = null;


                RImage.MouseDown += delegate (object Sender, MouseButtonEventArgs E)
                {
                    MessageBoxResult _Result = MessageBox.Show("Do you really want to download " + BufferOne + "?", "Download", MessageBoxButton.YesNo);
                    if (_Result == MessageBoxResult.Yes)
                    {
                        _KFOne = BooksCheck.GetFileKey(BuffOne);
                        BooksCheck._KeyFile = _KFOne;
                        Parallel.Invoke(BooksCheck.DownloadFile);
                    }
                };

                TextBlock TBTitle = new TextBlock();
                TBTitle.Text = DGD[PictureKey];
                TBTitle.FontFamily = new FontFamily("Comic Sans MS");
                TBTitle.FontSize = 20;
                TBTitle.TextWrapping = TextWrapping.Wrap;

                ++PictureKey;
                string BuffTwo = BooksCheck.GetPicturePath(PictureKey);
                Rectangle RImageTwo = new Rectangle();
                RImageTwo.Fill = new ImageBrush(new BitmapImage(new Uri(BuffTwo, UriKind.Relative)));


                ToolTip toolTipTwo = new ToolTip();

                StackPanel toolTipPanelTwo = new StackPanel();
                toolTipPanelTwo.Children.Add(new TextBlock { Text = DGN[PictureKey], FontSize = 16 });
                toolTipTwo.Content = toolTipPanelTwo;
                RImageTwo.ToolTip = toolTipTwo;
                string BufferTwo = DGN[PictureKey];
                string _KF = null;
                RImageTwo.MouseDown += delegate (object Sender, MouseButtonEventArgs E)
                {
                    MessageBoxResult _Result = MessageBox.Show("Do you really want to download " + BufferTwo + " ?", "Download", MessageBoxButton.YesNo);
                    if (_Result == MessageBoxResult.Yes)
                    {
                        _KF = BooksCheck.GetFileKey(BuffTwo);
                        BooksCheck._KeyFile = _KF;
                        Parallel.Invoke(BooksCheck.DownloadFile);
                    }
                };
                TextBlock TBTitleTwo = new TextBlock();
                TBTitleTwo.Text = DGD[PictureKey];
                TBTitleTwo.FontFamily = new FontFamily("Comic Sans MS");
                TBTitleTwo.FontSize = 20;
                TBTitleTwo.TextWrapping = TextWrapping.Wrap;

                Grid.SetRow(RImage, 0);
                Grid.SetRowSpan(RImage, 20);
                Grid.SetColumn(RImage, 0);
                Grid.SetColumnSpan(RImage, 6);

                Grid.SetRow(RImageTwo, 0);
                Grid.SetRowSpan(RImageTwo, 20);
                Grid.SetColumn(RImageTwo, 11);
                Grid.SetColumnSpan(RImageTwo, 5);

                Grid.SetRow(TBTitle, 0);
                Grid.SetRowSpan(TBTitle, 20);
                Grid.SetColumn(TBTitle, 6);
                Grid.SetColumnSpan(TBTitle, 5);

                Grid.SetRow(TBTitleTwo, 0);
                Grid.SetRowSpan(TBTitleTwo, 20);
                Grid.SetColumn(TBTitleTwo, 16);
                Grid.SetColumnSpan(TBTitleTwo, 4);

                grid.Children.Add(RImage);
                grid.Children.Add(RImageTwo);
                grid.Children.Add(TBTitle);
                grid.Children.Add(TBTitleTwo);

                Label Lbl = new Label();
                Lbl.Height = (45 * ActualHeight) / 100;
                Lbl.Content = grid;
                LiVi.Items.Add(Lbl);
            }
        }
      
    }
}
