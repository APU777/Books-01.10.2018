using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Books_01._10._2018
{
    internal static class BooksCheck
    {
        private static string Name = null;
        private static string Link = null;
        private static int BooKey = 0;
        public static string _KeyFile = null;
        public static string GetUrl(string address)
        {
            WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.Accept] = "text/html, */*";
            webClient.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU";
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded,  charset=utf-8";
            try
            {
                return webClient.DownloadString(address);
            }
            catch
            {
                return "Hello";
            }
        }

        public static void GetImages(string _HtmlText)
        {
            bool Check = false;
            string StrBuffer = null;
            long _Qty = 0;
            long control = 0;
            string Buffer = null;

            for (int i = 0; i < _HtmlText.Length; ++i)
            {
                if (_HtmlText[i] == '\"' && Check == true)
                    ++_Qty;
                if (_HtmlText[i] == 'i' && _HtmlText[i + 1] == 'm' && _HtmlText[i + 2] == 'g' && _HtmlText[i + 3] == ' ' && _HtmlText[i + 4] == 's' && _HtmlText[i + 5] == 'r' && _HtmlText[i + 6] == 'c')
                {
                    Check = true;
                    control = i + 8;
                }
                if (_Qty == 2)
                {
                    Check = false;
                    _Qty = 0;

                    if (StrBuffer.Contains(".jpg") || StrBuffer.Contains(".jpeg"))
                    {
                        Link = StrBuffer;
                        for (int n = Link.Length - 1; n > 0; --n)
                        {
                            if (Link[n] == '/')
                                break;

                            Buffer += Link[n];
                        }
                        for (int j = Buffer.Length - 1; j >= 0; --j)
                            Name += Buffer[j];

                        Parallel.Invoke(DownloadImage);
                        Name = null;
                    }

                    StrBuffer = null;
                    Buffer = null;
                }
                if (Check && i > control)
                    StrBuffer += _HtmlText[i];
            }
        }

        public static string GetPicturePath(int Key)
        {
            string[] FilesName = Directory.GetFiles(@"BooksImage\", "*.*").Select(System.IO.Path.GetFileName).ToArray();
            foreach (string path in FilesName)
            {
                if(path.Contains("("+ Key +")"))
                    return @"BooksImage\" + path;
            }
            return null;
        }

        public static Dictionary<int, string> GetBookName(string _HtmlText)
        {
            int _BooKey = 0;
            bool Check = false;
            string StrBuffer = null;
            long control = 0;
            Dictionary<int, string> LsBuffer = new Dictionary<int, string>();
            for (int i = 0; i < _HtmlText.Length; ++i)
            {
                if (Check && _HtmlText[i] == '<' && _HtmlText[i + 1] == '/')
                {
                    Check = false;
                    LsBuffer.Add(++_BooKey, StrBuffer);
                    StrBuffer = null;
                }

                if (_HtmlText[i] == 't' && _HtmlText[i + 1] == 'm' && _HtmlText[i + 2] == 'l' && _HtmlText[i + 3] == '\"' && _HtmlText[i + 4] == '>' && _HtmlText[i + 5] != '<')
                {
                    Check = true;
                    control = i + 4;
                }

                if (Check && i > control)
                    StrBuffer += _HtmlText[i];
            }
            return LsBuffer;
        }

        public static Dictionary<int, string> GetDescription(string _HtmlText)
        {
            int _BooKey = 0;
            bool Check = false;
            string StrBuffer = null;
            long control = 0;
            Dictionary<int, string> LsBuffer = new Dictionary<int, string>();

            for (int i = 0; i < _HtmlText.Length; ++i)
            {
                if (_HtmlText[i] == '<' && _HtmlText[i + 1] == '/' && _HtmlText[i + 2] == 'p' && Check == true)
                {
                    Check = false;
                    StrBuffer += "\n";

                    if (StrBuffer.Contains(".."))
                    {
                        LsBuffer.Add(++_BooKey, StrBuffer);
                        StrBuffer = null;
                    }
                }

                if (_HtmlText[i] == '<' && _HtmlText[i + 1] == 'p' && _HtmlText[i + 2] == '>')
                {
                    Check = true;
                    control = i + 2;
                }

                if (Check && i > control)
                    StrBuffer += _HtmlText[i];
            }
           
            return LsBuffer;
        }

        public static string GetFileKey(string Path)
        {
            bool Check = false;
            int control = 0;
            string BufferPath = null;
            for (int i = 0; i < Path.Length; ++i)
            {
                if (Path[i] == '.')
                {
                    Check = false;
                    break;
                }
                if (Path[i] == ')')
                {
                    Check = true;
                    control = i;
                }

                if (Check && i > control)
                    BufferPath += Path[i];
            }
            return BufferPath;
        }

        private static void DownloadImage()
        {
            if (!Directory.Exists(@"BooksImage\"))
                Directory.CreateDirectory(@"BooksImage\");

            ++BooKey;
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Accept] = "text/html, */*";
            webClient.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU";
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded,  charset=utf-8";

            webClient.DownloadFile(Link, @"BooksImage\" + "(" + BooKey + ")" + Name);
        }

        public static void DownloadFile()
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://avidreaders.ru/api/get.php?b=" + _KeyFile + "&f=txt");
                httpWebRequest.Referer = "http://avidreaders.ru/";
                //httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Timeout = 5000;
                httpWebRequest.ReadWriteTimeout = 10000;

                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                WebClient webClient = new WebClient();
                webClient.DownloadFile(response.ResponseUri, "mybook.zip");
                
                response.Close();
                httpWebRequest.Abort();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public static void RefreshPictures()
        {
            if (Directory.Exists(@"BooksImage"))
            {
                string[] FilesName = Directory.GetFiles(@"BooksImage\", "*.*").Select(System.IO.Path.GetFileName).ToArray();
                foreach (string _FN in FilesName)
                    File.Delete(@"BooksImage\" + _FN);
            }
        }

        public static void WorkWS(ListBox LiVi, double ActualHeight, double ActualWidth)
        {
            foreach (Label lIvI in LiVi.Items)
            {
                lIvI.Height = (45 * ActualHeight) / 100;

                ((Rectangle)((Grid)lIvI.Content).Children[0]).Height = lIvI.Height;
                ((Rectangle)((Grid)lIvI.Content).Children[0]).Width = (25 * ActualWidth) / 100;
                ((Rectangle)((Grid)lIvI.Content).Children[1]).Height = lIvI.Height;
                ((Rectangle)((Grid)lIvI.Content).Children[1]).Width = (25 * ActualWidth) / 100;
                ((TextBlock)((Grid)lIvI.Content).Children[2]).Height = lIvI.Height;
                ((TextBlock)((Grid)lIvI.Content).Children[2]).Width = ((25 * ActualWidth) / 100) - 30;

                if (((TextBlock)((Grid)lIvI.Content).Children[2]).FontSize == 11)
                     ((TextBlock)((Grid)lIvI.Content).Children[2]).FontSize = 20;
                else
                    ((TextBlock)((Grid)lIvI.Content).Children[2]).FontSize = 11;


                ((TextBlock)((Grid)lIvI.Content).Children[3]).Height = lIvI.Height;
                ((TextBlock)((Grid)lIvI.Content).Children[3]).Width = ((25 * ActualWidth) / 100) - 30;

                if (((TextBlock)((Grid)lIvI.Content).Children[3]).FontSize == 11)
                    ((TextBlock)((Grid)lIvI.Content).Children[3]).FontSize = 20;
                else
                    ((TextBlock)((Grid)lIvI.Content).Children[3]).FontSize = 11;
            }
        }

    }
}
