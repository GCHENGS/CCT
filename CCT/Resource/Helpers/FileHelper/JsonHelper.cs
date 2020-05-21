using CCT.Model.DataType;
using CCT.Resource.Enums;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;

namespace CCT.Resource.Helpers.FileHelper
{
    public class JsonHelper
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

        public JsonHelper(string filePath)
        {
            this.filePath = filePath;
        }

        #endregion

        #region 读取文件

        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool Load(Node root)
        {
            try
            {
                // 创建文件读取流
                StreamReader file = new StreamReader(filePath,Encoding.Default);// 解决中文乱码问题
                // 获取所有文本
                string jsonText = file.ReadToEnd();
                // json字符串只有对象和数组两种结构，分别以"{" 和 "[" 开头
                if (!jsonText.StartsWith("[") && !jsonText.StartsWith("{"))
                {
                    return false;//错误的json格式
                }
                else
                {
                    //数组
                    if (jsonText.StartsWith("["))
                    {
                        // 初始化树根节点
                        root.DisplayName = JsonType.Array.ToString();
                        // 进入递归解析
                        ParseArrayTree(jsonText,root);
                    }
                    //对象
                    if (jsonText.StartsWith("{"))
                    {
                        // 初始化树根节点
                        root.DisplayName = JsonType.Object.ToString();
                        // 进入递归解析
                        ParseObjectTree(jsonText, root);
                    }
                    file.Close();//关闭文件流
                    return true;//解析成功
                }
            }
            catch
            {
                return false;//解析失败，存在错误的json格式
            }
        }

        /// <summary>
        /// 解析数组结构(初始化树)
        /// </summary>
        /// <returns></returns>
        private void ParseArrayTree(string json, Node parent)
        {
            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(json);
            for (int i = 0; i < jsonArray.Count; i++)
            {
                Node node = new Node();
                var jstr = jsonArray[i];
                var val = jstr.ToString();
                if (val.StartsWith("["))
                {
                    node.DisplayName = JsonType.Array.ToString();
                    node.Parent = parent;
                    ParseArrayTree(val,node);
                    parent.Children.Add(node);
                }
                else if (val.StartsWith("{"))
                {
                    node.DisplayName = JsonType.Object.ToString();
                    node.Parent = parent;
                    ParseObjectTree(val, node);
                    parent.Children.Add(node);
                }
                else
                {
                    node.DisplayName = val;
                    node.CanInsert = false;
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
            } 
        }

        /// <summary>
        /// 解析对象结构(初始化树)
        /// </summary>
        /// <returns></returns>
        private void ParseObjectTree(string json, Node parent)
        {
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(json);
            foreach (var x in jsonObject)
            {
                Node node = new Node();
                var key = x.Key.ToString();
                var val = x.Value.ToString();
                if(x.Value.ToString().StartsWith("{"))
                {
                    node.DisplayName = key;
                    node.Parent = parent;
                    ParseObjectTree(val,node);
                    parent.Children.Add(node);
                }
                else if(val.StartsWith("["))
                {
                    node.DisplayName = key;
                    node.Parent = parent;
                    ParseArrayTree(val, node);
                    parent.Children.Add(node);
                }
                else
                {
                    node.DisplayName = key + ":" + val;
                    node.CanInsert = false;
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
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
                File.Delete(filePath);
            }
            try
            {
                FileStream fileStream = File.Create(filePath);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.Default);//使用与系统一致的编码方式
                string jsonStr = string.Empty;
                if (root.DisplayName.Equals(JsonType.Array.ToString()))//数组结构
                {
                    jsonStr="[" + ConvertArrayTree(root).ToString() + "]";
                }
                else//对象结构
                {
                    jsonStr = "{" + ConvertObjectTree(root).ToString() + "}";
                }
                sw.WriteLine(jsonStr);
                sw.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换数组数据
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private StringBuilder ConvertArrayTree(Node root)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var node in root.Children)
            {
                if(node.DisplayName.Equals(JsonType.Array.ToString()))
                {
                    //数组类型
                    builder.Append("[" + ConvertArrayTree(node).ToString() + "],");
                }
                else if(node.DisplayName.Equals(JsonType.Object.ToString()))
                {
                    //对象类型
                    builder.Append("{" + ConvertObjectTree(node).ToString() + "},");
                }
                else
                {
                    //数值类型
                    builder.Append("\""+node.DisplayName + "\",");
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder = builder.Remove(builder.Length - 1, 1);
            }
            return builder;
        }

        /// <summary>
        /// 转换对象数据
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private StringBuilder ConvertObjectTree(Node root)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var node in root.Children)
            {
                if (node.DisplayName.Equals(JsonType.Array.ToString()))
                {
                    //数组类型
                    builder.Append("[" + ConvertArrayTree(node).ToString() + "],");
                }
                else if (node.DisplayName.Equals(JsonType.Object.ToString()))
                {
                    //对象类型
                    builder.Append("{" + ConvertObjectTree(node).ToString() + "},");
                }
                else
                {
                    //数值类型
                    builder.Append("\"" + node.DisplayName + "\",");
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder = builder.Remove(builder.Length - 1, 1);
            }
            return builder;
        }

        #endregion

        #region 导出Excel

        #region 导出

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool ExportExcel(string path, Node root)
        {
            try
            {
                //获取数据集
                System.Data.DataTable dt = ListToDataSet(root);

                //创建Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //创建工作簿（WorkBook：即Excel文件主体本身）
                Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];
                //设置工作表名
                excelWS.Name = "json-config";

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
                excelWS.Cells[1, 1] = "Key"; //Excel单元格赋值 
                range.Font.Name = "黑体"; //设置字体的种类 
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 

                range = (Range)excelWS.get_Range("B1"); //设置列名
                excelWS.Cells[1, 2] = "Value"; //Excel单元格赋值 
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
        private System.Data.DataTable ListToDataSet(Node root)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));
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
            if (parent.DisplayName.Contains(":"))
            {
                var str = parent.DisplayName;
                row = dt.NewRow();
                row["Key"] = str.Substring(0,str.IndexOf(":"));
                row["Value"] = str.Substring(str.IndexOf(":")+1, str.Length-str.IndexOf(":")-1);
                dt.Rows.Add(row);
            }
            foreach (var node in parent.Children)
            {
                if (node.DisplayName.Contains(":"))
                {
                    var str = node.DisplayName;
                    row = dt.NewRow();
                    row["Key"] = str.Substring(0, str.IndexOf(":"));
                    row["Value"] = str.Substring(str.IndexOf(":") + 1, str.Length - str.IndexOf(":") - 1);
                    dt.Rows.Add(row);
                }
                else
                {
                    GetData2(node, dt);
                }
            }
        }


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

        #region 导出Xml

        /// <summary>
        /// 导出Xml(指定地址)
        /// </summary>
        /// <returns></returns>
        public bool ExportXml(string xmlPath)
        {
            try
            {
                // 创建文件读取流
                StreamReader file = new StreamReader(filePath, Encoding.Default);// 解决中文乱码问题
                // 获取所有文本
                string jsonText = file.ReadToEnd();
                //转换json串
                string json = string.Empty;
                if(jsonText.StartsWith("["))
                {                  
                    string newText = ParseArray(jsonText).ToString();
                    json = @"{
                                  '?xml': {
                                           '@version': '1.0',
                                           '@encoding': 'UTF-8'
                                           },
                                  'root': {
                                           'array': ["
                                                     + newText
                                                  +"]}}";
                }
                else if(jsonText.StartsWith("{"))
                {
                    json = @"{
                                  '?xml': {
                                           '@version': '1.0',
                                           '@encoding': 'UTF-8'
                                           },
                                  'object': " + jsonText 
                           + "}";
                }
                if(json.Equals(string.Empty))
                {
                    return false;
                }
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
                doc.Save(xmlPath);
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换Xml显示
        /// </summary>
        /// <returns></returns>
        public bool DisplayXml(RichTextBox richTextBox)
        {
            try
            {
                // 创建文件读取流
                StreamReader file = new StreamReader(filePath, Encoding.Default);// 解决中文乱码问题
                // 获取所有文本
                string jsonText = file.ReadToEnd();
                //转换json串
                string json = string.Empty;
                if (jsonText.StartsWith("["))
                {
                    string newText = ParseArray(jsonText).ToString();
                    json = @"{
                                  '?xml': {
                                           '@version': '1.0',
                                           '@encoding': 'UTF-8'
                                           },
                                  'root': {
                                           'array': ["
                                                     + newText
                                                  + "]}}";
                }
                else if (jsonText.StartsWith("{"))
                {
                    json = @"{
                                  '?xml': {
                                           '@version': '1.0',
                                           '@encoding': 'UTF-8'
                                           },
                                  'object': " + jsonText
                           + "}";
                }
                if (json.Equals(string.Empty))
                {
                    return false;
                }
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                string str = doc.OuterXml;
                //str = str.Replace("<", "  <");
                string newStr = str.Replace(">",">\r\n");
                documentTextRange.Load(new MemoryStream(Encoding.UTF8.GetBytes(newStr)), dataFormat);
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 解析数组结构
        /// </summary>
        /// <returns></returns>
        private StringBuilder ParseArray(string json)
        {
            JArray jsonArray = (JArray)JsonConvert.DeserializeObject(json);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                var jstr = jsonArray[i];
                var val = jstr.ToString();
                if (val.StartsWith("["))
                {
                    //数组类型
                    builder.Append("[" + ParseArray(val).ToString() + "],");
                }
                else if (val.StartsWith("{"))
                {
                    //对象类型 
                    builder.Append(val + ",");
                }
                else
                {
                    //数值类型
                    builder.Append("'" + val + "',");
                }
            }
            if (builder.ToString().EndsWith(","))
            {
                builder = builder.Remove(builder.Length - 1, 1);
            }
            return builder;
        }

        #endregion
    }
}
