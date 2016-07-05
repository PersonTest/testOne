using System.Configuration;
using System.Xml;

namespace RM.Common.DotNetConfig
{
    public class ConfigHelper
    {
        /// <summary>
        /// ��ȡ�����ļ���key�ڵ��ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }

        /// <summary>
        /// ��xmlĳ�ڵ㸳ֵ
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="selectPath"></param>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        public static void SetValue(XmlDocument xmlDocument, string selectPath, string key, string keyValue)
        {
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    xmlNode.Attributes["value"].Value = keyValue;
                    break;
                }
            }
        }
    }
}