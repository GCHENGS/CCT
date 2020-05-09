using CCT.Resource.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CCT.Component.Controls
{
    /// <summary>
    /// 扩展按钮
    /// </summary>
    public class ExtendButton : Button

    {
        static ExtendButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendButton), new FrameworkPropertyMetadata(typeof(ExtendButton)));
        }


        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty ButtonTypeProperty =
         DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(ExtendButton), new PropertyMetadata(ButtonType.Normal));


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
         DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ExtendButton), new PropertyMetadata(null));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
         DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ExtendButton), new PropertyMetadata(new CornerRadius(0)));


        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
         DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(ExtendButton), new PropertyMetadata());


        public Brush MousePressedForeground
        {
            get { return (Brush)GetValue(MousePressedForegroundProperty); }
            set { SetValue(MousePressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedForegroundProperty =
         DependencyProperty.Register("MousePressedForeground", typeof(Brush), typeof(ExtendButton), new PropertyMetadata());


        public Brush MouseOverBorderbrush
        {
            get { return (Brush)GetValue(MouseOverBorderbrushProperty); }
            set { SetValue(MouseOverBorderbrushProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBorderbrushProperty =
         DependencyProperty.Register("MouseOverBorderbrush", typeof(Brush), typeof(ExtendButton), new PropertyMetadata());


        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
         DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ExtendButton), new PropertyMetadata());


        public Brush MousePressedBackground
        {
            get { return (Brush)GetValue(MousePressedBackgroundProperty); }
            set { SetValue(MousePressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedBackgroundProperty =
         DependencyProperty.Register("MousePressedBackground", typeof(Brush), typeof(ExtendButton), new PropertyMetadata());
    }
}
