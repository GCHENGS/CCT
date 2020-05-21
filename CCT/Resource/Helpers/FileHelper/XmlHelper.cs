using CCT.Model.DataType;
using CCT.Resource.Enums;
using CCT.ViewModel;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;

namespace CCT.Resource.Helpers.FileHelper
{
    public class XmlHelper
    {
        #region 私有变量

        private string filePath = string.Empty;//记录文件地址

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        #endregion

        #region 构造方法

        public XmlHelper(string filePath)
        {
            this.filePath = filePath;
        }

        #endregion

        #region 解析文件

        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool Load(Node root,XmlParseWindowViewModel xmlParseWindowViewModel)
        {
            try
            {
                //将XML文件加载进来
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                XmlReader reader = XmlReader.Create(sr, settings);
                doc.Load(reader);
                //获取根节点
                XmlElement rootElement = doc.DocumentElement;
                root.DisplayName = rootElement.Name;
                foreach(var item in rootElement.Attributes)
                {
                    XmlAttribute xa = item as XmlAttribute;
                    root.Attributies.Add(new PropertyItem(root, xmlParseWindowViewModel)
                    {
                         KeyName= xa.Name,
                         KeyValue=xa.Value
                    });
                }
                GenerateNode(root, rootElement,xmlParseWindowViewModel);
                reader.Close();
                return true;//解析成功
            }
            catch
            {
                return false;//解析失败，存在错误的json格式
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="root"></param>
        private void GenerateNode(Node root,XmlElement xmlNode,XmlParseWindowViewModel xmlParseWindowViewModel)
        {
            //获取到XML的根元素进行操作
            foreach (var element in xmlNode.ChildNodes)
            {
                Node node = new Node();
                if (element is XmlElement)
                {
                    XmlElement xe = element as XmlElement;
                    node.DisplayName = xe.Name;
                    node.NodeType = NodeType.Element.ToString();
                    foreach (var attribute in xe.Attributes)
                    {
                        XmlAttribute xa = attribute as XmlAttribute;
                        node.Attributies.Add(new PropertyItem(node, xmlParseWindowViewModel)
                        {
                            KeyName = xa.Name,
                            KeyValue = xa.Value
                        });
                    }
                    GenerateNode(node, xe, xmlParseWindowViewModel);
                }
                else if(element is XmlText)
                {
                    XmlText xt = element as XmlText;
                    node.CanInsert = false;//文本节点不可添加子节点
                    node.DisplayName = xt.Value;
                    node.NodeType = NodeType.Text.ToString();
                }
                node.Parent = root;
                root.Children.Add(node);
            }
        }

        #endregion

        #region 保存文件

        /// <summary>
        /// 保存文件(公开)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool Save(Node root)
        {
            if (File.Exists(filePath))
            {
                //File.Delete(filePath);
            }
            try
            {
                //创建XML文档对象
                XmlDocument doc = new XmlDocument();
                //创建文件声明
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                //给文件节点添加文件声明(添加子节点)
                doc.AppendChild(declaration);

                //创建根元素
                XmlElement rootElement = doc.CreateElement(root.DisplayName);
                //添加属性
                foreach(var item in root.Attributies)
                {
                    //创建属性
                    XmlAttribute attribute = doc.CreateAttribute(item.KeyName);
                    attribute.Value = item.KeyValue;
                    rootElement.SetAttributeNode(attribute);
                }              
                //给文件节点添加根节点
                doc.AppendChild(rootElement);
                SaveData(root, rootElement, doc);
                //将XML文档保存到指定文件，不存在则创建，存在则覆盖、
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="rootElement"></param>
        private void SaveData(Node parent,XmlElement rootElement, XmlDocument doc)
        {
            foreach(var node in parent.Children)
            {
                XmlElement xmlElement = null;
                if (node.NodeType.Equals(NodeType.Element.ToString()))
                {
                    xmlElement = doc.CreateElement(node.DisplayName);
                    foreach (var item in node.Attributies)
                    {
                        //创建属性
                        XmlAttribute attribute = doc.CreateAttribute(item.KeyName);
                        attribute.Value = item.KeyValue;
                        xmlElement.SetAttributeNode(attribute);
                    }
                    if (node.Children.Count > 0)
                    {
                        SaveData(node, xmlElement, doc);
                    }
                }
                else if(node.NodeType.Equals(NodeType.Text.ToString()))
                {
                    xmlElement = doc.CreateElement(node.DisplayName);
                }
                rootElement.AppendChild(xmlElement);
            }
        }

        #endregion

        #region 导出Excel(未实现)

        #region 节点

        #region 导出

        /// <summary>
        /// 导出节点
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool ExportNodeExcel(string path, Node root)
        {
            try
            {
                //获取数据集
                System.Data.DataTable dt = ListToDataSet1(root);

                //创建Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //创建工作簿（WorkBook：即Excel文件主体本身）
                Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];
                //设置工作表名
                excelWS.Name = "xml-config";

                //excelWS.Cells.NumberFormat = "@";// 如果数据中存在数字类型 可以让它变文本格式显示
                //将数据导入到工作表的单元格
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString(); //Excel单元格索引2开始
                    }
                }

                #region 表格属性设置

                Range range = null;

                range = excelWS.Columns;

                range.AutoFit();//自动适应列宽

                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式

                #region 表头
                /*
                range = (Range)excelWS.get_Range("A1", "E1"); //获取Excel多个单元格区域：本例做为Excel表头 

                range.Merge(0); //单元格合并动作   要配合上面的get_Range()进行设计

                excelWS.Cells[1, 1] = "InI配置文件"; //Excel单元格赋值 

                range.Font.Size = 15; //设置字体大小

                range.Font.Underline = true; //设置字体是否有下划线

                range.Font.Name = "黑体"; //设置字体的种类 

                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 

                range.ColumnWidth = 15; //设置单元格的宽度 

                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色 

                range.Borders.LineStyle = 1; //设置单元格边框的粗细 

                range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 

                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range.EntireColumn.AutoFit(); //自动调整列宽 

                range.WrapText = true; //文本自动换行 

                range.Interior.ColorIndex = 39; //填充颜色为淡紫色 

                range.Font.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //字体颜色 

                excelApp.DisplayAlerts = false; //保存Excel的时候，不弹出是否保存的窗口直接进行保存
                */
                #endregion

                #region 列标题

                range = (Range)excelWS.get_Range("A1"); //设置列名
                excelWS.Cells[1, 1] = "Node"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range = (Range)excelWS.get_Range("B1"); //设置列名
                excelWS.Cells[1, 2] = "Type"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                #endregion

                #endregion

                excelWB.SaveAs(path);  //将其进行保存到指定的路径
                excelWB.Close();
                excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 数据放入DataTable

        /// <summary>
        /// 把数据放入DataTable
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private System.Data.DataTable ListToDataSet1(Node root)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Node", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            DataRow row;
            row = dt.NewRow();
            row["Node"] = root.DisplayName;
            row["Type"] = root.NodeType;
            dt.Rows.Add(row);
            GetData1(root, dt);
            return dt;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="parent"></param>
        private void GetData1(Node parent, System.Data.DataTable dt)
        {
            DataRow row;
            row = dt.NewRow();
            row["Node"] = parent.DisplayName;
            row["Type"] = parent.NodeType;
            dt.Rows.Add(row);
            foreach (var node in parent.Children)
            {
                if (node.Children.Count == 0)
                {
                    row = dt.NewRow();
                    row["Node"] = node.DisplayName;
                    row["Type"] = node.NodeType;
                    dt.Rows.Add(row);
                }
                else
                {
                    GetData1(node,dt);
                }             
            }
        }

        #endregion

        #endregion

        #region 属性

        #region 导出

        /// <summary>
        /// 导出属性
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool ExportAttributeExcel(string path, Node root)
        {
            try
            {
                //获取数据集
                System.Data.DataTable dt = ListToDataSet2(root);

                //创建Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //创建工作簿（WorkBook：即Excel文件主体本身）
                Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];
                //设置工作表名
                excelWS.Name = "xml-config";

                //excelWS.Cells.NumberFormat = "@";// 如果数据中存在数字类型 可以让它变文本格式显示
                //将数据导入到工作表的单元格
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        excelWS.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString(); //Excel单元格索引2开始
                    }
                }

                #region 表格属性设置

                Range range = null;

                range = excelWS.Columns;

                range.AutoFit();//自动适应列宽

                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式

                #region 表头
                /*
                range = (Range)excelWS.get_Range("A1", "E1"); //获取Excel多个单元格区域：本例做为Excel表头 

                range.Merge(0); //单元格合并动作   要配合上面的get_Range()进行设计

                excelWS.Cells[1, 1] = "InI配置文件"; //Excel单元格赋值 

                range.Font.Size = 15; //设置字体大小

                range.Font.Underline = true; //设置字体是否有下划线

                range.Font.Name = "黑体"; //设置字体的种类 

                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 

                range.ColumnWidth = 15; //设置单元格的宽度 

                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色 

                range.Borders.LineStyle = 1; //设置单元格边框的粗细 

                range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 

                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range.EntireColumn.AutoFit(); //自动调整列宽 

                range.WrapText = true; //文本自动换行 

                range.Interior.ColorIndex = 39; //填充颜色为淡紫色 

                range.Font.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //字体颜色 

                excelApp.DisplayAlerts = false; //保存Excel的时候，不弹出是否保存的窗口直接进行保存
                */
                #endregion

                #region 列标题

                range = (Range)excelWS.get_Range("A1"); //设置列名
                excelWS.Cells[1, 1] = "Node"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range = (Range)excelWS.get_Range("B1"); //设置列名
                excelWS.Cells[1, 2] = "Key"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(245, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                                              //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                                              //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range = (Range)excelWS.get_Range("C1"); //设置列名
                excelWS.Cells[1, 3] = "Value"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                #endregion

                #endregion

                excelWB.SaveAs(path);  //将其进行保存到指定的路径
                excelWB.Close();
                excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 数据放入DataTable

        /// <summary>
        /// 把数据放入DataTable
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private System.Data.DataTable ListToDataSet2(Node root)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Node", typeof(string));
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            DataRow row;         
            foreach(var item in root.Attributies)
            {
                row = dt.NewRow();
                row["Node"] = root.DisplayName;
                row["Key"] = item.KeyName;
                row["Value"] = item.KeyValue;
                dt.Rows.Add(row);
            }
            GetData2(root, dt);
            return dt;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="parent"></param>
        private void GetData2(Node parent, System.Data.DataTable dt)
        {
            DataRow row;
            if (parent.Attributies.Count > 0)
            {
                foreach (var item in parent.Attributies)//元素节点
                {
                    row = dt.NewRow();
                    row["Node"] = parent.DisplayName;
                    row["Key"] = item.KeyName;
                    row["Value"] = item.KeyValue;
                    dt.Rows.Add(row);
                }
            }
            foreach (var node in parent.Children)
            {
                if (node.Children.Count == 0)
                {
                    if (node.NodeType.Equals(NodeType.Element))
                    {
                        if (node.Attributies.Count > 0)
                        {
                            foreach (var item in node.Attributies)
                            {
                                row = dt.NewRow();
                                row["Node"] = node.DisplayName;
                                row["Key"] = item.KeyName;
                                row["Value"] = item.KeyValue;
                                dt.Rows.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    GetData2(node, dt);
                }
            }
        }

        #endregion

        #endregion

        #region 释放Excel进程

        /// <summary>
        /// 有时候Excel会长时间占用进程，那么我们需要做释放进程的操作
        /// </summary>
        /// <param name="excelApp"></param>
        /// <returns></returns>
        public bool KillAllExcel(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            try
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    //释放COM组件，其实就是将其引用计数减1   
                    //System.Diagnostics.Process theProc;   
                    foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                    {
                        //先关闭图形窗口。如果关闭失败.有的时候在状态里看不到图形窗口的excel了，   
                        //但是在进程里仍然有EXCEL.EXE的进程存在，那么就需要释放它   
                        if (theProc.CloseMainWindow() == false)
                        {
                            theProc.Kill();
                        }
                    }
                    excelApp = null;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region 导出Json

        /// <summary>
        /// 导出Json(指定地址)
        /// </summary>
        /// <returns></returns>
        public bool ExportXml(string jsonPath)
        {
            try
            {
                // 创建文件读取流
                StreamReader file = new StreamReader(filePath, Encoding.Default);// 解决中文乱码问题
                // 获取所有文本
                string xmlText = file.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlText);
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                FileStream fileStream = File.Create(jsonPath);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.Default);//使用与系统一致的编码方式
                sw.WriteLine(jsonText);
                sw.Close();
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换Json显示
        /// </summary>
        /// <returns></returns>
        public bool DisplayJson(RichTextBox richTextBox)
        {
            try
            {
                // 创建文件读取流
                StreamReader file = new StreamReader(filePath, Encoding.Default);// 解决中文乱码问题
                // 获取所有文本
                string xmlText = file.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlText);
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                documentTextRange.Load(new MemoryStream(Encoding.UTF8.GetBytes(jsonText)), dataFormat);
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
