using CCT.Resource.Enums;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CCT.Component.Controls
{
    /// <summary>
    /// 扩展文本框
    /// </summary>
    public class ExtendTextBox : TextBox,IDisposable
    {
        #region 重写基类型的元数据
        //指定依赖属性的实例
        static ExtendTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendTextBox), new FrameworkPropertyMetadata(typeof(ExtendTextBox)));
        }
        #endregion

        #region 模板应用
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            btnEyes = Template.FindName("EyesButton", this) as Button;

            if(btnEyes != null)
            {
                btnEyes.PreviewMouseLeftButtonDown += BtnEyes_MouseLeftButtonDown;
                btnEyes.PreviewMouseLeftButtonUp += BtnEyes_MouseLeftButtonUp;
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 水印文字
        /// </summary>
        public string WaterMark
        {
            get { return GetValue(WaterMarkProperty).ToString(); }
            set { SetValue(WaterMarkProperty, value); }
        }

        /// <summary>
        /// 边框角度
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public new Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        /// <summary>
        /// 是否为密码框
        /// </summary>
        public bool IsPasswordBox
        {
            get { return (bool)GetValue(IsPasswordBoxProperty); }
            set { SetValue(IsPasswordBoxProperty, value); }
        }

        /// <summary>
        /// 替换明文的单个密码字符
        /// </summary>
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        /// <summary>
        /// 密码字符串
        /// </summary>
        public string PasswordStr
        {
            get { return GetValue(PasswordStrProperty).ToString(); }
            set { SetValue(PasswordStrProperty, value); }
        }

        /// <summary>
        /// 图标方向
        /// </summary>
        public IconDirection IconDirection
        {
            get { return (IconDirection)GetValue(IconDirectionProperty); }
            set { SetValue(IconDirectionProperty, value); }
        }
        #endregion

        #region 依赖属性
        //定义依赖属性
        public static DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(ExtendTextBox));

        public static DependencyProperty BorderCornerRadiusProperty =
             DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(ExtendTextBox));

        public static DependencyProperty IsPasswordBoxProperty =
            DependencyProperty.Register("IsPasswordBox", typeof(bool), typeof(ExtendTextBox), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsPasswordBoxChange)));

        public static DependencyProperty PasswordCharProperty =
           DependencyProperty.Register("PasswordChar", typeof(char), typeof(ExtendTextBox), new FrameworkPropertyMetadata('●'));

        public static DependencyProperty PasswordStrProperty =
          DependencyProperty.Register("PasswordStr", typeof(string), typeof(ExtendTextBox), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconDirectionProperty =
            DependencyProperty.Register("IconDirection", typeof(IconDirection), typeof(ExtendTextBox), new PropertyMetadata(IconDirection.Right));
        #endregion

        #region 局部变量
        private bool IsResponseChange = true;
        private StringBuilder PasswordBuilder;
        private int lastOffset;
        private Button btnEyes;
        #endregion

        #region 事件处理

        //控件加载事件
        private void ExtendTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPasswordBox)
            {
                IsResponseChange = false;
                this.Text = ConvertToPasswordChar(PasswordStr.Length);
                IsResponseChange = true;
            }
        }

        //当设置为密码框时，监听TextChange事件，处理Text的变化，这是密码框的核心功能
        private static void OnIsPasswordBoxChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ExtendTextBox).SetEvent();
        }

        /// <summary>
        /// 定义TextChange事件
        /// </summary>
        private void SetEvent()
        {
            if (IsPasswordBox)
                this.TextChanged += ExtendTextBox_TextChanged;
            else
                this.TextChanged -= ExtendTextBox_TextChanged;
        }

        //在TextChange事件中，处理Text为密码文，并将原字符记录给PasswordStr予以存储
        private void ExtendTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsResponseChange) //响应事件标识，替换字符时，不处理后续逻辑
                return;
            Console.WriteLine(string.Format("------{0}------", e.Changes.Count));
            foreach (TextChange c in e.Changes)
            {
                Console.WriteLine(string.Format("addLength:{0} removeLenth:{1} offSet:{2}", c.AddedLength, c.RemovedLength, c.Offset));
                PasswordStr = PasswordStr.Remove(c.Offset, c.RemovedLength); //从密码文中根据本次Change对象的索引和长度删除对应个数的字符
                PasswordStr = PasswordStr.Insert(c.Offset, Text.Substring(c.Offset, c.AddedLength));   //将Text新增的部分记录给密码文
                lastOffset = c.Offset;
            }
            Console.WriteLine(PasswordStr);
            /*将文本转换为密码字符*/
            IsResponseChange = false;  //设置响应标识为不响应
            this.Text = ConvertToPasswordChar(Text.Length);  //将输入的字符替换为密码字符
            IsResponseChange = true;   //回复响应标识
            this.SelectionStart = lastOffset + 1; //设置光标索引
            Console.WriteLine(string.Format("SelectionStar:{0}", this.SelectionStart));
        }

        /// <summary>
        /// 眼睛控制显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEyes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Text != null)
            {
                if (Text.Contains(PasswordChar.ToString()))
                {
                    lastOffset = this.SelectionStart;
                    IsResponseChange = false;
                    this.Text = PasswordStr;
                    IsResponseChange = true;
                    this.SelectionStart = lastOffset;
                }
                var btn = sender as Button;
                btn.Content = null;
                Image img = new Image()
                {
                    Source = GetEyesImage(@"D:/VS开发/CCT/CCT/Resource/Images/eyes.png"),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 11,
                    Height = 11
                };
                btn.Content = img;
            }
        }

        /// <summary>
        /// 眼睛控制隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEyes_MouseLeftButtonUp(object sender,MouseButtonEventArgs e)
        {
            if (Text != null)
            {
                if (!Text.Contains(PasswordChar.ToString()))
                {
                    lastOffset = this.SelectionStart;
                    IsResponseChange = false;
                    this.Text = ConvertToPasswordChar(Text.Length);
                    IsResponseChange = true;
                    this.SelectionStart = lastOffset;
                }
                var btn = sender as Button;
                btn.Content = null;
                Image img = new Image()
                {
                    Source = GetEyesImage(@"D:/VS开发/CCT/CCT/Resource/Images/eyes_disabled.png"),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 11,
                    Height = 11
                };
                btn.Content = img;
            }
        }

        #endregion

        #region private处理方法
        /// <summary>
        /// 按照指定的长度生成密码字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string ConvertToPasswordChar(int length)
        {
            if (PasswordBuilder != null)
                PasswordBuilder.Clear();
            else
                PasswordBuilder = new StringBuilder();
            for (var i = 0; i < length; i++)
                PasswordBuilder.Append(PasswordChar);
            return PasswordBuilder.ToString();
        }

        /// <summary>
        /// 鼠标按下松开图标
        /// </summary>
        /// <returns></returns>
        private BitmapImage GetEyesImage(string imgPath)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();//初始化
            bmp.UriSource = new Uri(imgPath);
            bmp.EndInit();//结束初始化
            return bmp;
        }

        #endregion

        #region public处理方法

        public void Dispose()
        {
            if(btnEyes!=null)
            {
                btnEyes.PreviewMouseLeftButtonDown -= BtnEyes_MouseLeftButtonDown;
                btnEyes.PreviewMouseLeftButtonUp -= BtnEyes_MouseLeftButtonUp;
            }
        }

        #endregion
    }
}
