using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RM.Common.DotNetData
{
    public class DataTableHelper
    {
        public static Hashtable DataTableToHashtableByKeyValue(DataTable dt, string keyField, string valFiled)
        {
            Hashtable ht = new Hashtable();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string key = dr[keyField].ToString();
                    ht[key] = dr[valFiled];
                }
            }
            return ht;
        }

        /// <summary>
        /// ��datatableת����xml��ʽ���
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToXML(DataTable dt)
        {
            string result;
            if (dt != null)
            {
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);
                XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                serializer.Serialize(writer, dt);
                writer.Close();
                result = sb.ToString();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// ��dt���ݸ�ʽת��Ϊ��ֵ�Եķ�����
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<Hashtable> DataTableToArrayList(DataTable dt)
        {
            IList<Hashtable> result;
            if (dt == null)
            {
                result = new List<Hashtable>();
            }
            else
            {
                IList<Hashtable> datas = new List<Hashtable>();
                foreach (DataRow dr in dt.Rows)
                {
                    Hashtable ht = DataTableHelper.DataRowToHashTable(dr);
                    datas.Add(ht);
                }
                result = datas;
            }
            return result;
        }

        /// <summary>
        /// ��datatable��ʽת���ɹ�ϣ������
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Hashtable DataTableToHashtable(DataTable dt)
        {
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string key = dt.Columns[i].ColumnName;
                    ht[key.ToUpper()] = dr[key];
                }
            }
            return ht;
        }

        /// <summary>
        /// ��datarow����Դת���ɹ�ϣ������
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Hashtable DataRowToHashTable(DataRow dr)
        {
            Hashtable htReturn = new Hashtable(dr.ItemArray.Length);
            foreach (DataColumn dc in dr.Table.Columns)
            {
                htReturn.Add(dc.ColumnName, dr[dc.ColumnName]);
            }
            return htReturn;
        }

        /// <summary>
        /// datatable����תΪ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList DataTableToIList<T>(DataTable dt)
        {
            IList list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] propertys = obj.GetType().GetProperties();//��ȡ����
                PropertyInfo[] array = propertys;
                int i = 0;
                while (i < array.Length)
                {
                    PropertyInfo pi = array[i];
                    string tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (pi.CanWrite)
                        {
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(obj, value, null);
                            }
                        }
                    }
                IL_B2:
                    i++;
                    continue;
                    goto IL_B2;//��ô���õ���goto����أ�
                }
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// ����conditionƴ���µ�datatable����
        /// </summary>
        /// <param name="dt">dt����Դ</param>
        /// <param name="condition">�µ���</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable result;
            if (DataTableHelper.IsExistRows(dt))
            {
                if (condition.Trim() == "")
                {
                    result = dt;
                }
                else
                {
                    DataTable newdt = new DataTable();
                    newdt = dt.Clone();
                    DataRow[] dr = dt.Select(condition);
                    for (int i = 0; i < dr.Length; i++)
                    {
                        newdt.ImportRow(dr[i]);
                    }
                    result = newdt;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// ������Դ�е�ֵ����ɸѡȥ��
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="FieldNames"></param>
        /// <returns></returns>
        public static DataTable SelectDistinct(DataTable SourceTable, string[] FieldNames)
        {
            if (FieldNames == null || FieldNames.Length == 0)
            {
                throw new ArgumentNullException("FieldNames");
            }
            object[] lastValues = new object[FieldNames.Length];
            DataTable newTable = new DataTable();
            for (int i = 0; i < FieldNames.Length; i++)
            {
                string fieldName = FieldNames[i];
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);
            }
            DataRow[] orderedRows = SourceTable.Select("", string.Join(",", FieldNames));
            DataRow[] array = orderedRows;
            for (int i = 0; i < array.Length; i++)
            {
                DataRow row = array[i];
                if (!DataTableHelper.fieldValuesAreEqual(lastValues, row, FieldNames))//�ȽϷ���
                {
                    newTable.Rows.Add(DataTableHelper.createRowClone(row, newTable.NewRow(), FieldNames));
                    DataTableHelper.setLastValues(lastValues, row, FieldNames);
                }
            }
            return newTable;
        }

        /// <summary>
        /// ��ȡ�����е�ֵ���������µ�datarow���͵�������
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="newRow"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string field = fieldNames[i];
                newRow[field] = sourceRow[field];
            }
            return newRow;
        }

        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                lastValues[i] = sourceRow[fieldNames[i]];
            }
        }
        /// <summary>
        /// ��������ֵ���бȽ�
        /// </summary>
        /// <param name="lastValues"></param>
        /// <param name="currentRow"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;
            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }
            return areEqual;
        }

        /// <summary>
        /// �ж�dt�Ƿ�Ϊ��
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsExistRows(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        /// <summary>
        /// ��dt����һ����ʽ��������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static DataTable SortedTable(DataTable dt, params string[] sorts)
        {
            if (dt.Rows.Count > 0)
            {
                string tmp = "";
                for (int i = 0; i < sorts.Length; i++)
                {
                    tmp = tmp + sorts[i] + ",";
                }
                dt.DefaultView.Sort = tmp.TrimEnd(new char[]
				{
					','
				});
            }
            return dt;
        }

        /// <summary>
        /// ���ݵ�ǰҳ���Լ�һ���ж���ҳ�����ɶ�Ӧ��datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            DataTable result;
            if (PageIndex == 0)
            {
                result = dt;
            }
            else
            {
                DataTable newdt = dt.Copy();
                newdt.Clear();
                int rowbegin = (PageIndex - 1) * PageSize;
                int rowend = PageIndex * PageSize;
                if (rowbegin >= dt.Rows.Count)
                {
                    result = newdt;
                }
                else
                {
                    if (rowend > dt.Rows.Count)
                    {
                        rowend = dt.Rows.Count;
                    }
                    for (int i = rowbegin; i <= rowend - 1; i++)
                    {
                        DataRow newdr = newdt.NewRow();
                        DataRow dr = dt.Rows[i];
                        foreach (DataColumn column in dt.Columns)
                        {
                            newdr[column.ColumnName] = dr[column.ColumnName];
                        }
                        newdt.Rows.Add(newdr);
                    }
                    result = newdt;
                }
            }
            return result;
        }
    }
}