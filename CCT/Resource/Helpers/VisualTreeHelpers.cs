using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 遍历可视化树查找控件
    /// </summary>
    public class VisualTreeHelpers
    {
        #region 查找子控件

        /// <summary>
        /// 利用visualtreehelper寻找对象的子级对象
        /// </summary>
        ///  List<Button> btnList = FindVisualChild<Button>(TopGrid);
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<T> FindVisualChild1<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                        List<T> childOfChildren = FindVisualChild1<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild1<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }

        /// <summary>
        /// 子控件列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> FindVisualChild2<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> list = new List<T>();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child is T)
                    {
                        list.Add((T)child);
                        List<T> childOfChildren = FindVisualChild2<T>(child);
                        if (childOfChildren != null)
                        {
                            list.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild2<T>(child);
                        if (childOfChildren != null)
                        {
                            list.AddRange(childOfChildren);
                        }
                    }
                }

                return list;
            }
            catch (Exception)
            {
                //MessageBox.Show(ee.Message);
                return null;
            }
        }

        /* 实际应用
         * <ListBox Name = "ListBox_1" HorizontalAlignment="Left" Height="299" Margin="10,10,0,0" VerticalAlignment="Top" Width="497" MouseDoubleClick="ListBox_1_OnMouseDoubleClick">
　　　　<ListBox.ItemTemplate>
　　　　　　<DataTemplate>
　　　　　　　　<Button Name = "Button_1" Content="666"></Button>
　　　　　　</DataTemplate>
　　　　</ListBox.ItemTemplate>
       </ListBox>

       //通过可视化树双击ListBox的ltem把对应的button的Content值从666改成777：
        private void ListBox_1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //1.如果你在使用可视化树执行“ListBoxItem myListBoxItem = (ListBoxItem)ListBox_1.ItemContainerGenerator.ContainerFromItem(ListBox_1.SelectedItem);”这句返回值是空（实际上不是空），
            //可能是因为界面没有初始化完毕，我的理解是，在前台这个控件还没生成完毕，或者是你修改了值但前台还没有修改，可以加上这句：控件名.UpdateLayout();
            ListBoxItem myListBoxItem = (ListBoxItem)ListBox_1.ItemContainerGenerator.ContainerFromItem(ListBox_1.SelectedItem);
            List<Button> btnList = FindVisualChild<Button>(myListBoxItem);
            foreach (var item in btnList)
            {
                item.Content = "777";
            }
         }*/


        /// <summary>
        /// 查找指定类型的子控件
        /// </summary>
        /// <typeparam name="childItem">子控件类型</typeparam>
        /// <param name="obj">父控件</param>
        /// <returns></returns>
        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找某种类型的子控件，并返回一个List集合
        /// </summary>
        /// <example>List<Button> listButtons = GetChildObjects<Button>(parentPanel, typeof(Button))</example>
        /// <typeparam name="T">子控件类型</typeparam>
        /// <param name="obj">父控件</param>
        /// <param name="type">子控件的类</param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, Type type) where T : DependencyObject
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == type))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, type));
            }
            return childList;
        }

        /// <summary>
        /// 通过名称查找子控件，并返回一个List集合
        /// </summary>
        /// <example>List<Button> listButtons = GetChildObjects<Button>(parentPanel, "button1")</example>
        /// <typeparam name="T">子控件类型</typeparam>
        /// <param name="obj">父控件</param>
        /// <param name="name">子控件名称，默认为空</param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name = null) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, name));
            }
            return childList;
        }

        /// <summary>
        /// 通过名称查找某类型的子控件
        /// </summary>
        /// <example>StackPanel sp = GetChildObject<StackPanel>(this.LayoutRoot, "spDemoPanel")</example>
        /// <typeparam name="T">子控件类型</typeparam>
        /// <param name="obj">父控件</param>
        /// <param name="name">子控件名称，默认为空</param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name = null) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        #endregion

        #region 查找父控件

        /// <summary>
        /// 利用VisualTreeHelper寻找指定依赖对象的父级对象
        /// </summary>
        /// List<Grid> gridList = FindVisualParent<Grid>(btn_Two);
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                DependencyObject parent = VisualTreeHelper.GetParent(obj);
                if (parent != null && parent is T)
                {
                    TList.Add((T)parent);
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                else if (parent != null)
                {
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }


        /// <summary>
        /// 通过名称查找父控件
        /// </summary>
        /// <example>Grid layoutGrid = VTHelper.GetParentObject<Grid>(this.spDemoPanel, "LayoutRoot")</example>
        /// <typeparam name="T">父控件类型</typeparam>
        /// <param name="obj">子控件</param>
        /// <param name="name">父控件名称，默认为空</param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj, string name = null) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        #endregion
    }
}