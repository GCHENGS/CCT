using CCT.Model.DataType;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CCT.Resource.Helpers.FileHelper
{
    public class PropertiesHelper : System.Collections.Hashtable
    {
        #region 私有变量

        private string fileName = string.Empty;//记录文件地址

        private ArrayList list = new ArrayList();//值集合列表

        #endregion

        #region 公开属性

        public ArrayList List
        {
            get { return list; }
            set { list = value; }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">要读写的properties文件名</param>
        public PropertiesHelper(string fileName)
        {
            this.fileName = fileName;
            this.Load(fileName);
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 重写父类的方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public override void Add(object key, object value)
        {
            base.Add(key, value);
            list.Add(key);
        }

        /// <summary>
        /// 重写父类的方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Update(object key, object value)
        {
            base.Remove(key);
            list.Remove(key);
            this.Add(key, value);
        }

        public override ICollection Keys
        {
            get
            {
                return list;
            }
        }

        #endregion

        #region 加载文件

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        private void Load(string filepath)
        {
            char[] convertBuf = new char[1024];
            int limit;
            int keyLen;
            int valueStart;
            char c;
            string bufLine = string.Empty;
            bool hasSep;
            bool precedingBackslash;
            using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
            {
                while (sr.Peek() >= 0)
                {
                    bufLine = sr.ReadLine();
                    if (bufLine.Length == 0)
                    {
                        continue;//跳过空行
                    }
                    limit = bufLine.Length;
                    keyLen = 0;
                    valueStart = limit;
                    hasSep = false;
                    precedingBackslash = false;
                    if (bufLine.StartsWith("#"))
                    {
                        continue;//跳过注释
                        //keyLen = bufLine.Length;
                    }
                    while (keyLen < limit)
                    {
                        c = bufLine[keyLen];
                        if ((c == '=' || c == ':') & !precedingBackslash)
                        {
                            valueStart = keyLen + 1;
                            hasSep = true;
                            break;
                        }
                        else if ((c == ' ' || c == '\t' || c == '\f') & !precedingBackslash)
                        {
                            valueStart = keyLen + 1;
                            break;
                        }
                        if (c == '\\')
                        {
                            precedingBackslash = !precedingBackslash;
                        }
                        else
                        {
                            precedingBackslash = false;
                        }
                        keyLen++;
                    }
                    while (valueStart < limit)
                    {
                        c = bufLine[valueStart];
                        if (c != ' ' && c != '\t' && c != '\f')
                        {
                            if (!hasSep && (c == '=' || c == ':'))
                            {
                                hasSep = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        valueStart++;
                    }
                    string key = bufLine.Substring(0, keyLen);
                    string values = bufLine.Substring(valueStart, limit - valueStart);
                    if (key == "")
                        key += "#";
                    while (key.StartsWith("#") & this.Contains(key))
                    {
                        key += "#";
                    }
                    this.Add(key, values);
                }
            }
        }

        #endregion

        #region 保存文件

        /// <summary>
        /// 保存文件(私有)
        /// </summary>
        private void Save()
        {
            string filePath = this.fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fileStream = File.Create(filePath);
            StreamWriter sw = new StreamWriter(fileStream, Encoding.Default);//使用与系统一致的编码方式
            foreach (object item in list)
            {
                string key = (string)item;
                string val = (string)this[key];
                if (key.StartsWith("#"))
                {
                    if (val == "")
                    {
                        sw.WriteLine(key);
                    }
                    else
                    {
                        sw.WriteLine(val);
                    }
                }
                else
                {
                    sw.WriteLine(key + "=" + val);
                }
            }
            sw.Close();
            fileStream.Close();
        }

        /// <summary>
        /// 保存文件(公开)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        public bool Save(string filePath,List<PropertyItem> newList)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            try
            {
                FileStream fileStream = File.Create(filePath);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.Default);//使用与系统一致的编码方式
                foreach (var item in newList)
                {
                    string key = item.KeyName;
                    string val = (string)item.KeyValue;
                    if (key.StartsWith("#"))
                    {
                        if (val == "")
                        {
                            sw.WriteLine(key);
                        }
                        else
                        {
                            sw.WriteLine(val);
                        }
                    }
                    else
                    {
                        sw.WriteLine(key + "=" + val);
                    }
                }
                sw.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 导出文件

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="newList"></param>
        /// <returns></returns>
        public bool ExportExcel(string filePath,List<PropertyItem> newList)
        {
            try
            {
                //获取数据集
                System.Data.DataTable dt = ListToDataSet(newList);

                //创建Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //创建工作簿（WorkBook：即Excel文件主体本身）
                Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Worksheet excelWS = (Worksheet)excelWB.Worksheets[1];
                //设置工作表名
                excelWS.Name = "properties-config";

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

                excelWS.Cells[1, 1] = "Properties配置文件"; //Excel单元格赋值 

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
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(245, 204, 153).ToArgb(); //设置单元格的背景色
                range.Borders.LineStyle = 1; //设置单元格边框的粗细 
                range.EntireColumn.AutoFit(); //自动调整列宽 
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick, XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.FromArgb(255, 204, 153).ToArgb()); //给单元格加边框 
                //range.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框 
                
                #endregion

                #endregion

                excelWB.SaveAs(filePath);  //将其进行保存到指定的路径
                excelWB.Close();
                excelApp.Quit();  //KillAllExcel(excelApp); 释放可能还没释放的进程

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 把集合数据放入DataTable
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable ListToDataSet(List<PropertyItem> newList)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            DataRow row;
            foreach (var item in newList)
            {
                row = dt.NewRow();
                row["Key"] = item.KeyName;
                row["Value"] = item.KeyValue;
                dt.Rows.Add(row);
            }
            return dt;
        }

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
    }
}
