using EasyOpen.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace EasyOpen.Utils
{
    public class XMLHelper
    {
        #region 公共变量
        XmlDocument xmldoc;
        XmlNode xmlnode;
        XmlElement xmlelem;
        #endregion

        #region 创建Xml文档

        /// <summary>
        /// 创建一个带有根节点的Xml文件
        /// </summary>
        /// <param name="FileName">Xml文件名称</param>
        /// <param name="rootName">根节点名称</param>
        /// <param name="Encode">编码方式:gb2312，UTF-8等常见的</param>
        /// <returns></returns>
        public bool CreateXmlDocument(string RootName, string Encode)
        {
            try
            {
                xmldoc = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = xmldoc.CreateXmlDeclaration("1.0", Encode, null);
                xmldoc.AppendChild(xmldecl);
                xmlelem = xmldoc.CreateElement("", RootName, "");
                xmlelem.SetAttribute("Close", "0");//是否直接关闭
                xmlelem.SetAttribute("Clear", "0");//是否自动清理
                xmlelem.SetAttribute("AutoRun", "0");//是否开机启动
                xmlelem.SetAttribute("IsTop", "1");//是否置顶
                xmlelem.SetAttribute("DayCount", "1");//使用频率
                xmlelem.SetAttribute("NearClear", "");//最近清理时间，七天清理一次
                xmldoc.AppendChild(xmlelem);
                xmldoc.Save(Global.mXMLFilePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region 常用操作方法(增删改)

        /// <summary>
        /// 插入一个节点
        /// </summary>
        /// <param name="XmlFile"></param>
        /// <param name="fatherNodeName"></param>
        /// <param name="NewNodeName"></param>
        /// <param name="attributesKey"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool InsertNode(string fatherNode, string NewNodeName, string[] attributesKey, Dictionary<string, string> dic)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(Global.mXMLFilePath);
                XmlNode root = xmldoc.SelectSingleNode(fatherNode);

                foreach (var item in dic)
                {
                    XmlElement child = xmldoc.CreateElement(NewNodeName);
                    child.SetAttribute(attributesKey[0], item.Key);
                    child.SetAttribute(attributesKey[1], item.Value.Split(',')[0]);
                    child.InnerText = item.Value.Split(',')[1];
                    root.AppendChild(child);
                }
                xmldoc.AppendChild(root);
                xmldoc.Save(Global.mXMLFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入一个节点
        /// </summary>
        /// <param name="XmlFile"></param>
        /// <param name="fatherNodeName"></param>
        /// <param name="NewNodeName"></param>
        /// <param name="attributesKey"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool InsertNode(string fatherNode, string NewNodeName, Dictionary<string, string> dic)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(Global.mXMLFilePath);
                XmlNode root = xmldoc.SelectSingleNode(fatherNode);
                XmlElement child = xmldoc.CreateElement(NewNodeName);
                foreach (var item in dic)
                {
                    if (item.Key == "InnerText")
                    {
                        child.InnerText = item.Value;
                    }
                    else
                    {
                        child.SetAttribute(item.Key, item.Value);
                    }
                }
                root.AppendChild(child);
                xmldoc.AppendChild(root);
                xmldoc.Save(Global.mXMLFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNode(string fullpath, string attName, string value)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(Global.mXMLFilePath);
                XmlNodeList root = xmldoc.GetElementsByTagName("apps");
                foreach (XmlNode item in root)
                {
                    if (item.Attributes[0].Value == fullpath)
                    {
                        foreach (XmlAttribute item1 in item.Attributes)
                        {
                            if (item1.Name == attName)
                            {
                                item1.Value = value;
                                break;
                            }
                        }
                        break;
                    }
                }
                xmldoc.Save(Global.mXMLFilePath);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region XML文档节点查询和读取



        /// <summary>
        /// 选择匹配XPath表达式的节点列表XmlNodeList.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNodeList</returns>
        public XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
        {
            xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(xmlFileName); //加载XML文档
                XmlNodeList xmlNodeList = xmldoc.SelectNodes(xpath);
                return xmlNodeList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region 新加
        public string GetXmlAttrValue(string fullpath, string xmlAttributeName)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(Global.mXMLFilePath);
                XmlNodeList root = xmldoc.GetElementsByTagName("apps");
                foreach (XmlNode item in root)
                {
                    if (item.Attributes["fullpath"].Value == fullpath)
                    {
                        return item.Attributes[xmlAttributeName].Value;
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 获取所有应用的信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetApps()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Global.mXMLFilePath);
                XmlNodeList root = xml.GetElementsByTagName("apps");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode item in root)
                {
                    string str = item.Attributes["fullpath"].Value;
                    string value = item.Attributes["Count"].Value + "|";
                    value += item.Attributes["Start"].Value;
                    dic.Add(str, value);
                }
                return dic;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 获取root节点的属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetRootValue(string name)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Global.mXMLFilePath);
                XmlNode root = xml.SelectSingleNode("app");
                return root.Attributes[name].Value;

                //foreach (XmlAttribute item in root.Attributes)
                //{
                //    if (item.Name == name)
                //    {
                //        return item.Value;
                //    }
                //}
                //return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary>
        /// 设置root节点的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetRootValue(string name, string value)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Global.mXMLFilePath);
                XmlNode root = xml.SelectSingleNode("app");
                root.Attributes[name].Value = value;
                xml.Save(Global.mXMLFilePath);
                return true;
                //foreach (XmlAttribute item in root.Attributes)
                //{
                //    if (item.Name == name)
                //    {
                //        item.Value = value;
                //        xml.Save(Global.mXMLFilePath);
                //        return true;
                //    }
                //}

            }
            catch (Exception e)
            {
                return false;
                //throw new Exception(e.Message);
            }
        }

        #endregion


        /// <summary>
        /// 获取XML根元素
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <param name="isLoadFile">是否加载XML文档</param>
        /// <returns></returns>
        public XmlElement GetRootElement(string xmlFileName, bool isLoadFile)
        {
            xmldoc = new XmlDocument();
            if (isLoadFile)
            {
                try
                {
                    xmldoc.Load(xmlFileName); //加载XML文档
                    return xmldoc.DocumentElement;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return xmldoc.DocumentElement;
            }
        }

        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        public bool RemoveXML(string path, string listViewName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Global.mXMLFilePath);
            XmlElement rootElem = doc.DocumentElement;
            XmlNodeList personNodes = listViewName == "lvOneSelf" ? rootElem.SelectNodes("apps") : rootElem.SelectNodes("system");

            bool isok = false;
            string xmlValue = null;
            foreach (XmlNode item in personNodes)
            {
                xmlValue = string.IsNullOrEmpty(((XmlElement)item).GetAttribute("fullpath")) ? ((XmlElement)item).GetAttribute("dos") : ((XmlElement)item).GetAttribute("fullpath");
                if (xmlValue.Equals(path))
                {
                    rootElem.RemoveChild(item);
                    doc.Save(Global.mXMLFilePath);
                    isok = true;
                    break;
                }
            }
            return isok;
        }

        #region 私有方法

        /// <summary>
        /// 设置节点属性
        /// </summary>
        /// <param name="xe">节点所处的Element</param>
        /// <param name="htAttribute">节点属性，Key代表属性名称，Value代表属性值</param>
        private void SetAttributes(XmlElement xe, Hashtable htAttribute)
        {
            foreach (DictionaryEntry de in htAttribute)
            {
                xe.SetAttribute(de.Key.ToString(), de.Value.ToString());
            }
        }

        /// <summary>
        /// 增加子节点到根节点下
        /// </summary>
        /// <param name="rootNode">上级节点名称</param>
        /// <param name="XmlDoc">Xml文档</param>
        /// <param name="rootXe">父根节点所属的Element</param>
        /// <param name="SubNodes">子节点属性，Key为Name值，Value为InnerText值</param>
        private void SetNodes(string rootNode, XmlDocument XmlDoc, XmlElement rootXe, Hashtable SubNodes)
        {
            if (SubNodes == null)
                return;
            foreach (DictionaryEntry de in SubNodes)
            {
                xmlnode = XmlDoc.SelectSingleNode(rootNode);
                XmlElement subNode = XmlDoc.CreateElement(de.Key.ToString());
                subNode.InnerText = de.Value.ToString();
                rootXe.AppendChild(subNode);
            }
        }

        /// <summary>
        /// 更新节点属性和子节点InnerText值
        /// </summary>
        /// <param name="root">根节点名字</param>
        /// <param name="htAtt">需要更改的属性名称和值</param>
        /// <param name="htSubNode">需要更改InnerText的子节点名字和值</param>
        private void UpdateNodes(XmlNodeList root, Hashtable htAtt, Hashtable htSubNode)
        {
            foreach (XmlNode xn in root)
            {
                xmlelem = (XmlElement)xn;
                if (xmlelem.HasAttributes)//如果节点如属性，则先更改它的属性
                {
                    foreach (DictionaryEntry de in htAtt)//遍历属性哈希表
                    {
                        if (xmlelem.HasAttribute(de.Key.ToString()))//如果节点有需要更改的属性
                        {
                            xmlelem.SetAttribute(de.Key.ToString(), de.Value.ToString());//则把哈希表中相应的值Value赋给此属性Key
                        }
                    }
                }
                if (xmlelem.HasChildNodes)//如果有子节点，则修改其子节点的InnerText
                {
                    XmlNodeList xnl = xmlelem.ChildNodes;
                    foreach (XmlNode xn1 in xnl)
                    {
                        XmlElement xe = (XmlElement)xn1;
                        foreach (DictionaryEntry de in htSubNode)
                        {
                            if (xe.Name == de.Key.ToString())//htSubNode中的key存储了需要更改的节点名称，
                            {
                                xe.InnerText = de.Value.ToString();//htSubNode中的Value存储了Key节点更新后的数据
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}




