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
using System.Windows.Shapes;

namespace CCT.View
{
    /// <summary>
    /// Replace.xaml 的交互逻辑
    /// </summary>
    public partial class Replace : Window
    {
        private int findPlace, i;

        private TextPointer point1, point2;

        private TextPointer lastpoint1, lastpoint2;

        public Replace()
        {
            InitializeComponent();
            this.textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text == "")
            {
                button1.IsEnabled = false;
                button2.IsEnabled = false;
                button3.IsEnabled = false;
            }
            else
            {
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                button3.IsEnabled = true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string qvfen = textBox1.Text.ToUpper();
            string RichTextBoxtext;
            MainWindow mainform = (MainWindow)this.Owner;
            if(point1!=null) lastpoint1 = point1;
            if(point2!=null) lastpoint2 = point2;
            point1 = mainform.richTextBox.Selection.Start;
            point2 = mainform.richTextBox.Selection.End;
            TextRange textRange = new TextRange(mainform.richTextBox.Document.ContentStart, mainform.richTextBox.Document.ContentEnd);
            if (checkBox.IsChecked == true && radioButton1.IsChecked == true)
            {
                var lastPlace = 0;
                if (findPlace == 0)
                {
                    //首次
                    lastPlace = findPlace;
                }
                else
                {
                    lastPlace = point2.GetOffsetToPosition(lastpoint2);
                }

                RichTextBoxtext = textRange.Text.Trim().ToUpper();
                if (i == 0)
                {
                    findPlace = RichTextBoxtext.Length;
                    i++;
                }

                if ((findPlace = textRange.Text.LastIndexOf(textBox1.Text, findPlace)) == -1)
                {
                    MessageBox.Show("向上查找完毕!");
                    findPlace = 0;
                    i = 0;
                }
                else
                {
                    var p = point2.GetPositionAtOffset(-(RichTextBoxtext.Length - findPlace - lastPlace - textBox1.Text.Length));
                    var q = point2.GetPositionAtOffset(-(RichTextBoxtext.Length - findPlace - lastPlace));
                    if (p != null && q != null)
                    {
                        mainform.richTextBox.Selection.Select(q, p);
                        findPlace = findPlace - textBox1.Text.Length;
                        if (findPlace < 0) findPlace = 0;
                        mainform.richTextBox.Focus();
                    }
                    else
                    {
                        MessageBox.Show("向上查找完毕!");
                        findPlace = 0;
                        i = 0;
                    }
                }
            }
            else if (radioButton1.IsChecked == true)
            {
                var lastPlace = 0;
                if (findPlace == 0)
                {
                    //首次
                    lastPlace = findPlace;
                }
                else
                {
                    lastPlace = point2.GetOffsetToPosition(lastpoint2);
                }

                RichTextBoxtext = textRange.Text.Trim().ToUpper();
                if (i == 0)
                {
                    findPlace = RichTextBoxtext.Length;
                    i++;
                }

                if ((findPlace = RichTextBoxtext.LastIndexOf(qvfen, findPlace)) == -1)
                {
                    MessageBox.Show("向上查找完毕!");
                    findPlace = 0;
                    i = 0;
                }
                else
                {
                    var p = point2.GetPositionAtOffset(-(RichTextBoxtext.Length - findPlace -lastPlace  - textBox1.Text.Length));
                    var q = point2.GetPositionAtOffset(-(RichTextBoxtext.Length - findPlace -lastPlace));
                    if (p != null && q != null)
                    {
                        mainform.richTextBox.Selection.Select(q, p);
                        findPlace = findPlace - textBox1.Text.Length;
                        if (findPlace < 0) findPlace = 0;
                        mainform.richTextBox.Focus();
                    }
                    else
                    {
                        MessageBox.Show("向上查找完毕!");
                        findPlace = 0;
                        i = 0;
                    }
                }
            }
            else if (checkBox.IsChecked == true && radioButton2.IsChecked == true)
            {
                var lastPlace = 0;
                if (findPlace == 0)
                {
                    //首次
                    lastPlace = findPlace;
                }
                else
                {
                    lastPlace = findPlace - 1;
                }

                if ((findPlace = textRange.Text.IndexOf(textBox1.Text, findPlace)) == -1)
                {
                    MessageBox.Show("向下查找完毕!");
                    findPlace = 0; 
                }
                else
                {
                    var p = point1.GetPositionAtOffset(findPlace - lastPlace);
                    var q = point1.GetPositionAtOffset((findPlace - lastPlace) + textBox1.Text.Length);
                    if (p!= null && q != null)
                    {
                        mainform.richTextBox.Selection.Select(p,q);
                        findPlace = findPlace + textBox1.Text.Length;
                        mainform.richTextBox.Focus();
                    }
                    else
                    {
                        MessageBox.Show("向下查找完毕!");
                        findPlace = 0;
                    }
                }
            }
            else
            {
                var lastPlace=0;
                if (findPlace == 0)
                {
                    //首次
                    lastPlace = findPlace;
                }
                else
                {
                    lastPlace = findPlace-1;
                }
                RichTextBoxtext = textRange.Text.ToUpper();
                if ((findPlace = RichTextBoxtext.IndexOf(qvfen, findPlace)) == -1)
                {
                    MessageBox.Show("向下查找完毕!");
                    findPlace = 0;
                }
                else
                {

                    var p = point1.GetPositionAtOffset(findPlace-lastPlace);
                    var q = point1.GetPositionAtOffset((findPlace-lastPlace)+textBox1.Text.Length);
                    if (p != null && q != null)
                    {
                        mainform.richTextBox.Selection.Select(p, q);
                        findPlace = findPlace + textBox1.Text.Length;
                        mainform.richTextBox.Focus();
                    }
                    else
                    {
                        MessageBox.Show("向下查找完毕!");
                        findPlace = 0;
                    }
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainform = (MainWindow)this.Owner;
            string str1 = textBox2.Text;
            string str2 = mainform.richTextBox.Selection.Text;
            if (str2 != "")
            {
                mainform.richTextBox.Selection.Text = str1;
                mainform.Activate();
                button1_Click(sender, e);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainform = (MainWindow)this.Owner;
            TextRange textRange = new TextRange(mainform.richTextBox.Document.ContentStart, mainform.richTextBox.Document.ContentEnd);

            string str1 = textBox1.Text;
            string str2 = textBox2.Text;
            string Ustr1 = str1.ToUpper();
            string Lstr1 = str1.ToLower();

            if (textRange.Text.Length > 0)
            {
                if (checkBox.IsChecked == true)   //区分大小写
                {
                    string str5 = textRange.Text.Replace(str1, str2);
                    textRange.Text = str5;
                }
                else
                {
                    string str3 = textRange.Text.Replace(Ustr1, str2);
                    string str4 = str3.Replace(Lstr1, str2);
                    textRange.Text = str4;
                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
