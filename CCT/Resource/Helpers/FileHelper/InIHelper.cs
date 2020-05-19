using CCT.Model.DataType;
using CCT.ViewModel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CCT.Resource.Helpers.FileHelper
{
    public class InIHelper
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

        public InIHelper(string filePath)
        {
            this.filePath = filePath; 
        }

        #endregion   

        #region 保存文件

        /// <summary>
        /// 保存文件(公开)
        /// </summary>
        /// <param name="newList"></param>
        /// <returns></returns>
        public bool Save(List<SectionItem> newList)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            try
            {
                FileStream fileStream = File.Create(filePath);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.Default);//使用与系统一致的编码方式
                foreach (var section in newList)
                {
                    sw.WriteLine("[" + section.SectionName + "]");
                    foreach (var item in section.PropertyItems)
                    {
                        string key = item.KeyName;
                        string val = (string)item.KeyValue;
                        if (key.StartsWith(";"))
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
        public bool ExportExcel(string path, List<SectionItem> newList)
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
                excelWS.Name = "ini-config";

                //excelWS.Cells.NumberFormat = "@";// 如果数据中存在数字类型 可以让它变文本格式显示
                //将数据导入到工作表的单元格
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        excelWS.Cells[i + 1, j + 1] = dt.Rows[i][j].ToString(); //Excel单元格第一个从索引1开始
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
                excelWS.Cells[1, 1] = "Section"; //Excel单元格赋值 
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
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(245, 204, 153).ToArgb(); //设置单元格的背景色
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

        /// <summary>
        /// 把集合数据放入DataTable
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable ListToDataSet(List<SectionItem> newList)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Section", typeof(string));
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            DataRow row;
            foreach (var section in newList)
            {
                foreach (var item in section.PropertyItems)
                {
                    row = dt.NewRow();
                    row["Section"] = section.SectionName;
                    row["Key"] = item.KeyName;
                    row["Value"] = item.KeyValue;
                    dt.Rows.Add(row);
                }
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

        #region 私有方法

        #region 读取指定section，key

        /// <summary>
        /// 读取指定section，key
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">key的名称</param>
        /// <returns></returns>
        private string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.filePath);
            return temp.ToString();
        }

        /// <summary>
        /// 读取指定section，key
        /// </summary>
        /// <param name="section">section名称</param>
        /// <param name="key">key的名称</param>
        /// <returns></returns>
        private byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.filePath);
            return temp;
        }

        #endregion
      
        #region 读取Section

        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中
        private void ReadSection(string Section, StringCollection Idents)
        {
            byte[] Buffer = new Byte[16384];
            //Idents.Clear();
            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),this.filePath);
            //对Section进行解析
            GetStringsFromBuffer(Buffer, bufLen, Idents);
        }
        /// <summary>
        /// 获取字符串集合
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="bufLen"></param>
        /// <param name="Strings"></param>
        private void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        #endregion

        #region 写入

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        private void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.filePath);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除指定的section
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private bool RemoveSection(string sectionName)
        {
            int rs = (int)WritePrivateProfileString(sectionName, null, "", this.filePath);
            return rs > 0;
        }

        /// <summary>
        /// 移除ini文件下所有段落
        /// </summary>
        private void ClearAllSection()
        {
            IniWriteValue(null, null, null);
        }

        /// <summary>
        /// 移除ini文件下personal段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        private void ClearSection(string Section)
        {
            IniWriteValue(Section, null, null);
        }

        /// <summary>
        /// 移除指定的key
        /// key参数传入null就为移除指定的section
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private bool Removekey(string sectionName, string key)
        {
            int rs = (int)WritePrivateProfileString(sectionName, key, null, this.filePath);
            return rs > 0;
        }

        #endregion

        #region 创建/设置key的值

        /// <summary>
        /// 保存内容到ini文件
        /// 若此key不存在将会创建，否则就为修改此key的值。
        /// <para>若存在相同的key，就覆盖，否则就增加</para>
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="key">key的名称</param>
        /// <param name="value">存储的值</param>
        /// <param name="filePath">文件路径</param>
        private bool SetValue(string sectionName, string key, string value)
        {
            int rs = (int)WritePrivateProfileString(sectionName, key, value, this.filePath);
            return rs > 0;
        }

        #endregion

        #region 验证

        //// <summary>
        /// 验证文件是否存在
        /// </summary>
        /// <returns>布尔值</returns>
        private bool ExistFile()
        {
            return File.Exists(this.filePath);
        }

        //检查某个Section下的某个键值是否存在
        private bool ValueExists(string Section, string Ident)
        {
            StringCollection Idents = new StringCollection();
            ReadSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        #endregion

        #region 释放

        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        private void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, this.filePath);
        }

        //确保资源的释放
        ~InIHelper()
        {
            UpdateFile();
        }

        #endregion

        #endregion

        #region 公开方法

        #region 获取所有section

        /// <summary>
        /// 获取ini文件内所有的section名称
        /// </summary>
        /// <returns>返回一个包含section名称的集合</returns>
        public List<string> GetSectionNames()
        {
            byte[] buffer = new byte[2048];
            int length = GetPrivateProfileString(null, "", "", buffer, 999, filePath);
            String[] rs = System.Text.UTF8Encoding.Default.GetString(buffer, 0, length).Split(new string[] { "\0" }, StringSplitOptions.RemoveEmptyEntries);
            return rs.ToList();
        }

        #endregion

        #region 获取指定section下的所有key名称

        /// <summary>
        /// 获取指定section内的所有key
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回一个包含key名称的集合</returns>
        public List<string> GetKeys(string sectionName)
        {
            byte[] buffer = new byte[2048];
            int length = GetPrivateProfileString(sectionName, null, "", buffer, 999, this.filePath);
            String[] rs = System.Text.UTF8Encoding.Default.GetString(buffer, 0, length).Split(new string[] { "\0" }, StringSplitOptions.RemoveEmptyEntries);
            return rs.ToList();
        }

        #endregion

        #region 获取指定section,key的值

        /// <summary>
        /// 根据Key读取Value
        /// </summary>
        /// <param name="sectionName">section名称</param>
        /// <param name="key">key的名称</param>
        /// <param name="filePath">文件路径</param>
        public string GetValue(string sectionName, string key)
        {
            byte[] buffer = new byte[2048];
            int length = GetPrivateProfileString(sectionName, key, "发生错误", buffer, 999, this.filePath);
            string rs = System.Text.UTF8Encoding.Default.GetString(buffer, 0, length);
            return rs;
        }

        #endregion

        #endregion
    }
}
