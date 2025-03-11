using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace SSYM.OrgDsn.ViewModel.Utility
{
    public partial class XPDLGenerator
    {
        Dictionary<string, string> typesDictionary = new Dictionary<string, string>();
        private float PageX = 0;
        private float PageY = 0;
        List<string> x = new List<string>(), y = new List<string>(), h = new List<string>(), w = new List<string>(), l = new List<string>(), t = new List<string>(), n = new List<string>();
        List<string> EdgTypes = new List<string>(), EdgSource = new List<string>(), EdgDest = new List<string>(), EdgeN = new List<string>();
        Dictionary<string, List<Point>> EdgCoordinats = new Dictionary<string, List<Point>>();
        Dictionary<string, string> ID = new Dictionary<string, string>();
        private string DiagramId;
        List<string> idlist = new List<string>();
        private float LastPoolX = 30;
        private float LastPoolY = 30;
        int NstartID = 0;
        int EstartID = 0;
        private bool firstPage = false;
        private bool MainProcess = true;


        public XPDLGenerator(string GraphMlXml,string pathToSave,string ProcessName="")
        {
            
            XmlDocument xml=new XmlDocument();
            string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            XmlDocument bizagiDocument=new XmlDocument();
            
            generateId(idlist);
            DiagramId = idlist.Last();

            string createdDate = DateTime.Now.ToShortDateString();
            xml.LoadXml(GraphMlXml);
            bizagiDocument.LoadXml(string.Format(DiagramTemplate, DiagramId, "فرآیند خام", createdDate));
            XmlNode currentNode = xml["graphml"];
            
            for (int i = 0; i < currentNode.ChildNodes.Count; i++)
            {
                if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                    currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "NodeGeometry")
                {
                    typesDictionary.Add("NodSize", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
                else if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                         currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "NodeLabels")
                {
                    typesDictionary.Add("NodLabel", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
                else if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                         currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "NodeStyle")
                {
                    typesDictionary.Add("NodStyle", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
                else if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                         currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "EdgeStyle")
                {
                    typesDictionary.Add("EdgStyle", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
                else if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                         currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "EdgeGeometry")
                {
                    typesDictionary.Add("EdgSize", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
                else if (currentNode.ChildNodes[i].Attributes["attr.name"] != null &&
                         currentNode.ChildNodes[i].Attributes["attr.name"].InnerText == "EdgeLabels")
                {
                    typesDictionary.Add("EdgLabel", currentNode.ChildNodes[i].Attributes["id"].InnerText);
                }
            }
            string temptemp = "";
            List<int> adedNodes=new List<int>();

            GenerateAllIds(idlist,currentNode["graph"]);

            GenrateXPDL(temptemp, currentNode, ref bizagiDocument,adedNodes);
                NstartID = h.Count;
                EstartID = EdgeN.Count;
            currentNode = currentNode["graph"];
            for (int r = 0; r < currentNode.ChildNodes.Count; r++)
            {
                XmlNode TempCurrentNode = currentNode.ChildNodes[r];
                if (TempCurrentNode.Name == "node")
                {
                    n.Add(TempCurrentNode.Attributes["id"].InnerText);
                    for (int i = 0; i < TempCurrentNode.ChildNodes.Count; i++)
                    {
                        if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                            TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodSize"])
                        {
                            h.Add(
                                ((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Height"].InnerText.Replace(".", "/"))).ToString());
                            w.Add(
                                ((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Width"].InnerText.Replace(".", "/"))).ToString());
                            x.Add(
                                (((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["X"].InnerText.Replace(".","/")) +
                                 PageX))
                                    .ToString());
                            y.Add(
                                (((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Y"].InnerText.Replace(".", "/")) +
                                 PageY))
                                    .ToString());
                        }
                        if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                            TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodLabel"])
                        {
                            if (!adedNodes.Contains(r))
                            l.Add(TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                .InnerText);
                        }
                        if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                            TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodStyle"])
                        {
                            if(!adedNodes.Contains(r))
                            t.Add(
                                TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes[
                                    "Type"]
                                    .InnerText);
                        }
                    }
                    if (h.Count <= r + NstartID)
                    {
                        h.Add("");
                        w.Add("");
                        x.Add("");
                        y.Add("");
                    }
                    if (l.Count <= r + NstartID)
                    {
                        l.Add("");
                    }
                    if (t.Count <= r + NstartID)
                    {
                        t.Add("");
                    }
                }
                else if (TempCurrentNode.Name == "edge")
                {
                    EdgeN.Add(TempCurrentNode.Attributes["id"].InnerText);
                    EdgSource.Add(ID[TempCurrentNode.Attributes["source"].InnerText]);
                    EdgDest.Add(ID[TempCurrentNode.Attributes["target"].InnerText]);
                    EdgCoordinats.Add(ID[TempCurrentNode.Attributes["id"].InnerText], new List<Point>());
                    EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(getOffsetPoint(currentNode,
                        TempCurrentNode.Attributes["target"].InnerText,
                        TempCurrentNode.Attributes["targetport"].InnerText/*, x, y, w, h*/));
                    
                    for (int i = 0; i < TempCurrentNode.ChildNodes.Count; i++)
                    {
                        if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                            TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["EdgSize"])
                        {
                            foreach (XmlNode item in TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes)
                            {
                                string str1 = item.Attributes["Location"].InnerText;
                                string strTemp = "";
                                for (int k = 0; k < str1.Length && str1[k] != ','; k++)
                                {
                                    strTemp += str1[k];
                                }
                                float first = (int)float.Parse(strTemp.Replace(".", "/")) + PageX;
                                float second = (int)float.Parse(str1.Remove(0, strTemp.Length + 1).Replace(".", "/")) + PageY;
                                EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(new Point(first, second));
                            }

                        }
                        if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                            TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["EdgStyle"])
                        {
                            EdgTypes.Add(
                                TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes[
                                    "Type"]
                                    .InnerText);
                        }
                    }
                    EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(getOffsetPoint(currentNode,
                        TempCurrentNode.Attributes["source"].InnerText,
                        TempCurrentNode.Attributes["sourceport"].InnerText/*, x, y, w, h*/));
                }
            }
            int nodsCount = 0;
            for (int i = 0, ii = NstartID; ii < t.Count; i++, ii++, nodsCount++)
            {
                if (t[ii] == "A1")
                {
                    string tempStr = string.Format(ActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A2")
                {
                    string tempStr = string.Format(ActivityReceiveTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A3")
                {
                    string tempStr = string.Format(ActivityReceiveInstantiateTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A4")
                {
                    string tempStr = string.Format(ActivityReceiveTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A5")
                {
                    string tempStr = string.Format(ActivitySendTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A6")
                {
                    string tempStr = string.Format(ActivityUserTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A7")
                {
                    string tempStr = string.Format(ActivityManualTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A8")
                {
                    string tempStr = string.Format(ActivityBusinessRuleTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "A9")
                {
                    string tempStr = string.Format(ActivityScriptTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA1")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA2")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA3")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA4")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA5")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "CA6")
                {
                    string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E1" || t[ii] == "E2")
                {
                    string tempStr = string.Format(EventTimerTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E3")
                {
                    string tempStr = string.Format(EventTimerNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E4" || t[ii] == "E5")
                {
                    string tempStr = string.Format(EventIntemediateTimerTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E6")
                {
                    string tempStr = string.Format(EventIntemediateTimerNonTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E7" || t[ii] == "E8")
                {
                    string tempStr = string.Format(EventConditionalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E9")
                {
                    string tempStr = string.Format(EventConditionalNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E10" || t[ii] == "E11")
                {
                    string tempStr = string.Format(EventIntemediateConditionalTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E12")
                {
                    string tempStr = string.Format(EventIntemediateConditionalNonTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E13" || t[ii] == "E14")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventMessageTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E15")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventMessageNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E16" || t[ii] == "E17")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventIntemediateMessageTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E18")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventIntemediateMessageNonTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E19")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventThrowIntemediateMessageTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E20")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EndEventThrowMessageTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E21")
                {
                    string tempStr = string.Format(EventErrorTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E22")
                {
                    string tempStr = string.Format(EventIntemediateErrorTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E23")
                {
                    string tempStr = string.Format(EndEventErrorTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E24")
                {
                    string tempStr = string.Format(EventIntemediateCancelTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E25")
                {
                    string tempStr = string.Format(EndEventCancelTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E26" || t[ii] == "E27")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventSignalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E28")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventSignalNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E29" || t[ii] == "E30")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventIntemediateSignalTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E31")
                {
                    generateId(idlist);
                    string tempStr = string.Format(EventIntemediateSignalNonTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii], idlist.Last());
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E32")
                {
                    string tempStr = string.Format(EventThrowIntemediateSignalTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E33")
                {
                    string tempStr = string.Format(EndEventSignalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E34")
                {
                    string tempStr = string.Format(EventNoneTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E35")
                {
                    string tempStr = string.Format(EventIntemediateNoneTemplate, ID[n[ii]], l[ii], h[ii],
                        w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "E36")
                {
                    string tempStr = string.Format(EndEventNoneTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }


                else if (t[ii] == "G1")
                {
                    string tempStr = string.Format(ExlusiveGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G2")
                {
                    string tempStr = string.Format(InclusiveGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G3")
                {
                    string tempStr = string.Format(ParallelGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G4")
                {
                    string tempStr = string.Format(ComplexGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G5")
                {
                    string tempStr = string.Format(EventBasedGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                        x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G6")
                {
                    string tempStr = string.Format(ExlusiveEventBasedGetwayTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G7")
                {
                    //
                    string tempStr = string.Format(ParallelEventBasedIntermediateGetwayTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "G8")
                {
                    string tempStr = string.Format(ParallelEventBasedGetwayTemplate, ID[n[ii]], l[ii],
                        h[ii], w[ii], x[ii], y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "DO1")
                {
                    string tempStr = string.Format(DataObjectTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("DataObjects")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (t[ii] == "DO2")
                {
                    string tempStr = string.Format(DataObjectTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                    y[ii]);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("DataObjects")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
            }

            for (int i = 0, ii = EstartID; ii < EdgTypes.Count; ii++, i++)
            {
                if (EdgTypes[ii] == "SequenceFlow")
                {
                    string tempStr = "";
                    foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                    {
                        tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                    }
                    tempStr = string.Format(SequenceFlowTemplate, ID[EdgeN[ii]], EdgSource[ii],
                        EdgDest[ii],
                        tempStr);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Transitions")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (EdgTypes[ii] == "Association")
                {
                    string tempStr = "";
                    foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                    {
                        tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                    }
                    for (int j = 0; j < ID.Count; j++)
                    {
                        if (ID.ElementAt(j).Value == EdgSource[ii])
                        {
                            for (int k = 0;
                                k <
                                bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes
                                    .Count;
                                k++)
                            {
                                if (
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                        k].Attributes["Id"]
                                        .InnerText == EdgSource[ii])
                                {
                                    var tempXml2 = new XmlDocument();
                                    generateId(idlist);
                                    tempXml2.LoadXml(string.Format(OutputSetTemplate, idlist.Last()));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                        k]["OutputSets"].AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                    tempXml2 = new XmlDocument();
                                    tempXml2.LoadXml(string.Format(DataOutputTemplate, idlist.Last()));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("DataInputOutputs")[0]
                                        .AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                    string BeforeLastId = idlist.Last();
                                    tempXml2 = new XmlDocument();
                                    generateId(idlist);
                                    tempXml2.LoadXml(string.Format(AssociationTemplate, idlist.Last(), EdgSource[ii],
                                        EdgDest[ii], tempStr));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Associations")[0]
                                        .AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));

                                    EdgSource[ii] = BeforeLastId;
                                    break;
                                }

                            }
                        }
                    }

                    tempStr = string.Format(DataAssociationTemplate, ID[EdgeN[ii]], EdgSource[ii],
                        EdgDest[ii],
                        tempStr);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("DataAssociations")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (EdgTypes[ii] == "DirectedAssociation")
                {
                    string tempStr = "";
                    foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                    {
                        tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                    }
                    for (int j = 0; j < ID.Count; j++)
                    {
                        if (ID.ElementAt(j).Value == EdgDest[ii])
                        {
                            for (int k = 0;
                                k <
                                bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes
                                    .Count;
                                k++)
                            {
                                if (
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                        k].Attributes["Id"]
                                        .InnerText == EdgDest[ii])
                                {
                                    var tempXml2 = new XmlDocument();
                                    generateId(idlist);
                                    tempXml2.LoadXml(string.Format(InputSetTemplate, idlist.Last()));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                        k]["InputSets"].AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                    tempXml2 = new XmlDocument();
                                    tempXml2.LoadXml(string.Format(DataInputTemplate, idlist.Last()));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("DataInputOutputs")[0]
                                        .AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));
                                    string BeforeLastId = idlist.Last();

                                    tempXml2 = new XmlDocument();
                                    generateId(idlist);
                                    tempXml2.LoadXml(string.Format(AssociationTemplate, idlist.Last(), EdgSource[ii],
                                        EdgDest[ii], tempStr));
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Associations")[0]
                                        .AppendChild(
                                            bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                                    EdgDest[ii] = BeforeLastId;
                                    break;
                                }

                            }
                        }
                    }


                    tempStr = string.Format(DataAssociationTemplate, ID[EdgeN[ii]], EdgSource[ii],
                        EdgDest[ii],
                        tempStr);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("DataAssociations")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (EdgTypes[ii] == "NonDirectedMessageFlow")
                {
                    string tempStr = "";
                    foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                    {
                        tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                    }
                    //
                    tempStr = string.Format(MessageFlowTemplate, ID[EdgeN[ii]], EdgSource[ii],
                        EdgDest[ii],
                        tempStr);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("MessageFlows")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
                else if (EdgTypes[ii] == "MessageFlow")
                {
                    string tempStr = "";
                    int destindex = n.IndexOf(ID.SingleOrDefault(a => a.Value == EdgDest[ii]).Key);
                    int srcindex = n.IndexOf(ID.SingleOrDefault(a => a.Value == EdgDest[ii]).Key);
                    tempStr += string.Format(CoordinatesTemplate, x[destindex] , y[destindex] );
                    tempStr += string.Format(CoordinatesTemplate, x[srcindex] , y[srcindex] );
                    tempStr = string.Format(AssociationTemplate, ID[EdgeN[ii]], EdgDest[ii], EdgSource[ii],
                        
                        tempStr);
                    var tempXml = new XmlDocument();
                    tempXml.LoadXml(tempStr);
                    bizagiDocument.DocumentElement.GetElementsByTagName("Associations")[0].AppendChild(
                        bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                    bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                }
            }
            bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("(PrsNamMain)",ProcessName));
            bizagiDocument.Save(pathToSave);
        }

        public void GenrateXPDL(string returnStr, XmlNode xml, ref XmlDocument BIZAGI, List<int> adedList)
        {
            returnStr = "";
            XmlNode currentNode1 = xml["graph"];
            for (int r = 0; r < currentNode1.ChildNodes.Count; r++)
            {
                XmlNode checkPool = currentNode1.ChildNodes[r]["graph"];
                if (checkPool == null)
                    continue;
                //generateId(idlist);
                string PoolId = ID[currentNode1.ChildNodes[r].Attributes["id"].InnerText];
                generateId(idlist);
                string ProcesslId = idlist.Last();
                float PageX = 0;
                float PageY = 0;

                XmlDocument bizagiDocument = new XmlDocument();
                bizagiDocument.LoadXml(string.Format(WorkflowProcessTemplate, ProcesslId, "", DateTime.Now.ToShortDateString()));
                XmlNode currentNode = currentNode1.ChildNodes[r];
                for (int i = 0; i < currentNode.ChildNodes.Count; i++)
                {
                    if (currentNode.ChildNodes[i].Attributes["key"] != null &&
                        currentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodSize"])
                    {
                        PageX =(int) (0 - float.Parse(currentNode.ChildNodes[i].ChildNodes[0].Attributes["X"].InnerText.Replace(".", "/")) + 80);
                        PageY = (int)(0 - float.Parse(currentNode.ChildNodes[i].ChildNodes[0].Attributes["Y"].InnerText.Replace(".", "/")));
                        if (!firstPage)
                        {
                            this.PageX = PageX;
                            this.PageY = PageY;
                            firstPage = true;
                        }
                    }
                }
                NstartID = h.Count;
                EstartID = EdgeN.Count;
                currentNode = currentNode["graph"];
                if (currentNode == null)
                    continue;
                adedList.Add(r);

                for (int j = 0; j < currentNode.ChildNodes.Count; j++)
                {
                    XmlNode TempCurrentNode = currentNode.ChildNodes[j];
                    if (TempCurrentNode.Name == "node")
                    {
                        n.Add(TempCurrentNode.Attributes["id"].InnerText);
                        for (int i = 0; i < TempCurrentNode.ChildNodes.Count; i++)
                        {
                            if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                                TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodSize"])
                            {
                                h.Add(
                                ((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Height"].InnerText.Replace(".", "/"))).ToString());
                                w.Add(
                                    ((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Width"].InnerText.Replace(".", "/"))).ToString());
                                x.Add(
                                    (((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["X"].InnerText.Replace(".", "/")) +
                                     PageX))
                                        .ToString());
                                y.Add(
                                    (((int)float.Parse(TempCurrentNode.ChildNodes[i].ChildNodes[0].Attributes["Y"].InnerText.Replace(".", "/")) +
                                     PageY))
                                        .ToString());
                            }
                            if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                                TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodLabel"])
                            {
                                l.Add(TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0]
                                    .InnerText);
                            }
                            if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                                TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["NodStyle"])
                            {
                                t.Add(
                                    TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes[
                                        "Type"]
                                        .InnerText);
                            }
                        }
                        if (h.Count <= j + NstartID)
                        {
                            h.Add("");
                            w.Add("");
                            x.Add("");
                            y.Add("");
                        }
                        if (l.Count <= j + NstartID)
                        {
                            l.Add("");
                        }
                        if (t.Count <= j + NstartID)
                        {
                            t.Add("");
                        }
                    }
                    else if (TempCurrentNode.Name == "edge")
                    {
                        EdgeN.Add(TempCurrentNode.Attributes["id"].InnerText);
                        EdgSource.Add(ID[TempCurrentNode.Attributes["source"].InnerText]);
                        EdgDest.Add(ID[TempCurrentNode.Attributes["target"].InnerText]);
                        EdgCoordinats.Add(ID[TempCurrentNode.Attributes["id"].InnerText], new List<Point>());
                        EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(getOffsetPoint(currentNode,
                            TempCurrentNode.Attributes["source"].InnerText,
                            TempCurrentNode.Attributes["sourceport"].InnerText/*, x, y, w, h*/));
                        for (int i = 0; i < TempCurrentNode.ChildNodes.Count; i++)
                        {
                            if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                                TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["EdgSize"])
                            {
                                foreach (XmlNode item in TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes)
                                {
                                    string str1 = item.Attributes["Location"].InnerText;
                                    string strTemp = "";
                                    for (int k = 0; k < str1.Length && str1[k] != ','; k++)
                                    {
                                        strTemp += str1[k];
                                    }
                                    float first = (int)(float.Parse(strTemp.Replace(".", "/")) + PageX);
                                    float second = (int)(float.Parse(str1.Remove(0, strTemp.Length + 1).Replace(".", "/")) + PageY);
                                    EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(new Point(first, second));
                                }

                            }
                            if (TempCurrentNode.ChildNodes[i].Attributes["key"] != null &&
                                TempCurrentNode.ChildNodes[i].Attributes["key"].InnerText == typesDictionary["EdgStyle"])
                            {
                                EdgTypes.Add(
                                    TempCurrentNode.ChildNodes[i].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes[
                                        "Type"]
                                        .InnerText);
                            }
                        }
                        EdgCoordinats[ID[TempCurrentNode.Attributes["id"].InnerText]].Add(getOffsetPoint(currentNode,
                            TempCurrentNode.Attributes["target"].InnerText,
                            TempCurrentNode.Attributes["targetport"].InnerText/*, x, y, w, h*/));
                    }
                }
                int nodsCount = 0;
                for (int i = 0, ii = NstartID; ii < t.Count; i++, ii++, nodsCount++)
                {
                    if (t[ii] == "A1")
                    {
                        string tempStr = string.Format(ActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A2")
                    {
                        string tempStr = string.Format(ActivityReceiveTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A3")
                    {
                        string tempStr = string.Format(ActivityReceiveInstantiateTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A4")
                    {
                        string tempStr = string.Format(ActivityReceiveTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A5")
                    {
                        string tempStr = string.Format(ActivitySendTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A6")
                    {
                        string tempStr = string.Format(ActivityUserTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A7")
                    {
                        string tempStr = string.Format(ActivityManualTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A8")
                    {
                        string tempStr = string.Format(ActivityBusinessRuleTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "A9")
                    {
                        string tempStr = string.Format(ActivityScriptTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA1")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA2")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA3")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA4")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA5")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "CA6")
                    {
                        string tempStr = string.Format(CallingActivityTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E1" || t[ii] == "E2")
                    {
                        string tempStr = string.Format(EventTimerTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E3")
                    {
                        string tempStr = string.Format(EventTimerNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E4" || t[ii] == "E5")
                    {
                        string tempStr = string.Format(EventIntemediateTimerTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E6")
                    {
                        string tempStr = string.Format(EventIntemediateTimerNonTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E7" || t[ii] == "E8")
                    {
                        string tempStr = string.Format(EventConditionalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E9")
                    {
                        string tempStr = string.Format(EventConditionalNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E10" || t[ii] == "E11")
                    {
                        string tempStr = string.Format(EventIntemediateConditionalTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E12")
                    {
                        string tempStr = string.Format(EventIntemediateConditionalNonTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E13" || t[ii] == "E14")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventMessageTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E15")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventMessageNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E16" || t[ii] == "E17")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventIntemediateMessageTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E18")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventIntemediateMessageNonTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E19")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventThrowIntemediateMessageTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E20")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EndEventThrowMessageTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E21")
                    {
                        string tempStr = string.Format(EventErrorTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E22")
                    {
                        string tempStr = string.Format(EventIntemediateErrorTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E23")
                    {
                        string tempStr = string.Format(EndEventErrorTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E24")
                    {
                        string tempStr = string.Format(EventIntemediateCancelTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E25")
                    {
                        string tempStr = string.Format(EndEventCancelTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E26" || t[ii] == "E27")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventSignalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E28")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventSignalNonTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E29" || t[ii] == "E30")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventIntemediateSignalTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E31")
                    {
                        generateId(idlist);
                        string tempStr = string.Format(EventIntemediateSignalNonTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii], idlist.Last());
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E32")
                    {
                        string tempStr = string.Format(EventThrowIntemediateSignalTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E33")
                    {
                        string tempStr = string.Format(EndEventSignalTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E34")
                    {
                        string tempStr = string.Format(EventNoneTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E35")
                    {
                        string tempStr = string.Format(EventIntemediateNoneTemplate, ID[n[ii]], l[ii], h[ii],
                            w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "E36")
                    {
                        string tempStr = string.Format(EndEventNoneTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }


                    else if (t[ii] == "G1")
                    {
                        string tempStr = string.Format(ExlusiveGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G2")
                    {
                        string tempStr = string.Format(InclusiveGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G3")
                    {
                        string tempStr = string.Format(ParallelGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G4")
                    {
                        string tempStr = string.Format(ComplexGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G5")
                    {
                        string tempStr = string.Format(EventBasedGetwayTemplate, ID[n[ii]], l[ii], h[ii], w[ii],
                            x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G6")
                    {
                        string tempStr = string.Format(ExlusiveEventBasedGetwayTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G7")
                    {
                        //
                        string tempStr = string.Format(ParallelEventBasedIntermediateGetwayTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "G8")
                    {
                        string tempStr = string.Format(ParallelEventBasedGetwayTemplate, ID[n[ii]], l[ii],
                            h[ii], w[ii], x[ii], y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "DO1")
                    {
                        string tempStr = string.Format(DataObjectTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                            y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("DataObjects")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (t[ii] == "DO2")
                    {
                        string tempStr = string.Format(DataObjectTemplate, ID[n[ii]], l[ii], h[ii], w[ii], x[ii],
                        y[ii]);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("DataObjects")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                }

                for (int i = 0, ii = EstartID; ii < EdgTypes.Count; ii++, i++)
                {
                    if (EdgTypes[ii] == "SequenceFlow")
                    {
                        string tempStr = "";
                        foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                        {
                            tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                        }
                        tempStr = string.Format(SequenceFlowTemplate, ID[EdgeN[ii]], EdgSource[ii],
                            EdgDest[ii],
                            tempStr);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("Transitions")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (EdgTypes[ii] == "Association")
                    {
                        string tempStr = "";
                        foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                        {
                            tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                        }
                        for (int j = 0; j < ID.Count; j++)
                        {
                            if (ID.ElementAt(j).Value == EdgSource[ii])
                            {
                                for (int k = 0;
                                    k <
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes
                                        .Count;
                                    k++)
                                {
                                    if (
                                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                            k].Attributes["Id"]
                                            .InnerText == EdgSource[ii])
                                    {
                                        var tempXml2 = new XmlDocument();
                                        generateId(idlist);
                                        tempXml2.LoadXml(string.Format(OutputSetTemplate, idlist.Last()));
                                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                            k]["OutputSets"].AppendChild(
                                                bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                        tempXml2 = new XmlDocument();
                                        tempXml2.LoadXml(string.Format(DataOutputTemplate, idlist.Last()));
                                        bizagiDocument.DocumentElement.GetElementsByTagName("DataInputOutputs")[0]
                                            .AppendChild(
                                                bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                        string BeforeLastId = idlist.Last();
                                        tempXml2 = new XmlDocument();
                                        generateId(idlist);
                                        tempXml2.LoadXml(string.Format(AssociationTemplate, idlist.Last(), EdgSource[ii],
                                            EdgDest[ii], tempStr));
                                        BIZAGI.DocumentElement.GetElementsByTagName("Associations")[0]
                                            .AppendChild(
                                                BIZAGI.ImportNode(tempXml2.DocumentElement, true));

                                        BIZAGI.LoadXml(BIZAGI.InnerXml.Replace("xmlns=\"\"", ""));

                                        EdgSource[ii] = BeforeLastId;
                                        break;
                                    }

                                }
                            }
                        }

                        tempStr = string.Format(DataAssociationTemplate, ID[EdgeN[ii]], EdgSource[ii],
                            EdgDest[ii],
                            tempStr);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("DataAssociations")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                    else if (EdgTypes[ii] == "DirectedAssociation")
                    {
                        string tempStr = "";
                        foreach (var item in EdgCoordinats.ElementAt(ii).Value)
                        {
                            tempStr += string.Format(CoordinatesTemplate, item.X, item.Y);
                        }
                        for (int j = 0; j < ID.Count; j++)
                        {
                            if (ID.ElementAt(j).Value == EdgDest[ii])
                            {
                                for (int k = 0;
                                    k <
                                    bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes
                                        .Count;
                                    k++)
                                {
                                    if (
                                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                            k].Attributes["Id"]
                                            .InnerText == EdgDest[ii])
                                    {
                                        var tempXml2 = new XmlDocument();
                                        generateId(idlist);
                                        tempXml2.LoadXml(string.Format(InputSetTemplate, idlist.Last()));
                                        bizagiDocument.DocumentElement.GetElementsByTagName("Activities")[0].ChildNodes[
                                            k]["InputSets"].AppendChild(
                                                bizagiDocument.ImportNode(tempXml2.DocumentElement, true));

                                        tempXml2 = new XmlDocument();
                                        tempXml2.LoadXml(string.Format(DataInputTemplate, idlist.Last()));
                                        bizagiDocument.DocumentElement.GetElementsByTagName("DataInputOutputs")[0]
                                            .AppendChild(
                                                bizagiDocument.ImportNode(tempXml2.DocumentElement, true));
                                        string BeforeLastId = idlist.Last();

                                        tempXml2 = new XmlDocument();
                                        generateId(idlist);
                                        tempXml2.LoadXml(string.Format(AssociationTemplate, idlist.Last(), EdgSource[ii],
                                            EdgDest[ii], tempStr));
                                        BIZAGI.DocumentElement.GetElementsByTagName("Associations")[0]
                                            .AppendChild(
                                                BIZAGI.ImportNode(tempXml2.DocumentElement, true));

                                        BIZAGI.LoadXml(BIZAGI.InnerXml.Replace("xmlns=\"\"", ""));
                                        EdgDest[ii] = BeforeLastId;
                                        break;
                                    }

                                }
                            }
                        }


                        tempStr = string.Format(DataAssociationTemplate, ID[EdgeN[ii]], EdgSource[ii],
                            EdgDest[ii],
                            tempStr);
                        var tempXml = new XmlDocument();
                        tempXml.LoadXml(tempStr);
                        bizagiDocument.DocumentElement.GetElementsByTagName("DataAssociations")[0].AppendChild(
                            bizagiDocument.ImportNode(tempXml.DocumentElement, true));
                        bizagiDocument.LoadXml(bizagiDocument.InnerXml.Replace("xmlns=\"\"", ""));
                    }
                }
                XmlDocument Xmltemp4 = new XmlDocument();
                Xmltemp4.LoadXml(currentNode1.ChildNodes[r].OuterXml);
                XmlDocument Xmltemp5 = new XmlDocument();
                Xmltemp5.LoadXml(xml.OuterXml);
                XmlNodeList rowsList1 = Xmltemp4.GetElementsByTagName("y:Table.Rows");
                List<XmlNode> rowsList = new List<XmlNode>();
                XmlNodeList colsList = Xmltemp4.GetElementsByTagName("y:Table.Columns");
                for (int i = 0; i < rowsList1[0].ChildNodes.Count; i++)
                {
                    if (rowsList1[0].ChildNodes[i].Name == "y:Row")
                    {
                        rowsList.Add(rowsList1[0].ChildNodes[i]);
                    }
                    else
                    {
                        XmlNodeList tempList = Xmltemp5.GetElementsByTagName("y:Row");
                        for (int j = 0; j < tempList.Count; j++)
                        {
                            if (tempList[i].Attributes["x:Key"].InnerText ==
                                rowsList1[0].ChildNodes[i].Attributes["ResourceKey"].InnerText)
                            {
                                rowsList.Add(tempList[i]);
                                break;
                            }
                        }
                    }
                }
                string ColWidth = "0";
                if (colsList != null)
                {
                    ColWidth = ((int)float.Parse(colsList[0].ChildNodes[0].Attributes["Size"].InnerText.Replace(".", "/")) + 50).ToString();
                }
                string lanes = "";
                float polSize = 0;
                for (int i = 0; i < rowsList.Count; i++)
                {
                    string tempSize = rowsList[i].Attributes["Size"].InnerText;
                    polSize +=(int) float.Parse(tempSize.Replace(".", "/"));
                    string tempName = rowsList[i].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                    generateId(idlist);
                    lanes += string.Format(LaneTemplate, idlist.Last(), tempName, PoolId, tempSize, ColWidth);
                }
                string poolName = "";
                if (MainProcess)
                {
                    poolName = "(PrsNamMain)";
                    MainProcess = false;
                }
                string pool = string.Format(PolTemplate, PoolId, poolName, ProcesslId, lanes, polSize,
                   (int)( float.Parse(ColWidth.Replace(".", "/")) + 20), LastPoolX, LastPoolY);
                LastPoolY += polSize + 30;
                var tempXml3 = new XmlDocument();
                tempXml3.LoadXml(pool);
                returnStr += bizagiDocument.OuterXml;
                BIZAGI.DocumentElement.GetElementsByTagName("Pools")[0].AppendChild(
                    BIZAGI.ImportNode(tempXml3.DocumentElement, true));
                BIZAGI.LoadXml(BIZAGI.InnerXml.Replace("xmlns=\"\"", ""));
            }
            BIZAGI.LoadXml(BIZAGI.OuterXml.Replace("(PROCES)", returnStr));
        }

        public void GenerateAllIds(List<string> Ids, XmlNode xml)
        {
            var tempDocument = new XmlDocument();
            tempDocument.LoadXml(xml.OuterXml);
            XmlNodeList nodeList = tempDocument.GetElementsByTagName("node");
            XmlNodeList edgeList = tempDocument.GetElementsByTagName("edge");
            XmlNodeList graphList = tempDocument.GetElementsByTagName("graph");
            foreach (XmlNode item in nodeList)
            {
                generateId(Ids);
                ID.Add(item.Attributes["id"].InnerText, Ids.Last());
            }
            foreach (XmlNode item in edgeList)
            {
                generateId(Ids);
                ID.Add(item.Attributes["id"].InnerText, Ids.Last());
            }
            foreach (XmlNode item in graphList)
            {
                generateId(Ids);
                ID.Add(item.Attributes["id"].InnerText, Ids.Last());
            }
        }

        public void generateId(List<string> Ids)
        {
            string str = "";
            var rand = new Random();
            for (int i = 0; i < 32; i++)
            {
                str += rand.Next(16).ToString("X").ToLower();
                if (i == 7 || i == 11 || i == 15 || i == 19)
                    str += "-";
            }
            if (Ids.Contains(str))
            {
                generateId(Ids);
            }
            else
            {
                Ids.Add(str);
            }

        }

        public Point getOffsetPoint(XmlNode Doc, string nodId, string portId)
        {
            if (Doc == null)
                return new Point();
            XmlDocument tempDocument = new XmlDocument();
            tempDocument.LoadXml(Doc.OuterXml);
            var temp = tempDocument.GetElementsByTagName("node");
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Attributes["id"] != null && temp[i].Attributes["id"].InnerText == nodId)
                {
                    for (int j = 0; j < temp[i].ChildNodes.Count; j++)
                    {
                        if (temp[i].ChildNodes[j].Name == "port" && temp[i].ChildNodes[j].Attributes["name"] != null &&
                            temp[i].ChildNodes[j].Attributes["name"].InnerText == portId)
                        {

                            string str = "0,0";
                            if (portId == "p0")
                            {
                                str = "0,0.5";
                            }
                            else if (portId == "p1")
                            {
                                str = "0.5,0";
                            }
                            else if (portId == "p2")
                            {
                                str = "0,-0.5";
                            }
                            else if (portId == "p3")
                            {
                                str = "-0.5,0";
                            }
                            string strTemp = "";
                            for (int k = 0; k < str.Length && str[k] != ','; k++)
                            {
                                strTemp += str[k];
                            }
                            float first =float.Parse(strTemp.Replace(".", "/")) + 0.5F;
                            float second = float.Parse(str.Remove(0, strTemp.Length + 1).Replace(".", "/")) + 0.5F;
                            for (int k = 0; k < n.Count; k++)
                            {
                                if (n[k] == nodId)
                                {
                                    return new Point((int)(float.Parse(x[k].Replace(".", "/")) + first * float.Parse(w[k].Replace(".", "/"))),
                                        (int)(float.Parse(y[k].Replace(".", "/")) + second * float.Parse(h[k].Replace(".", "/"))));
                                }
                            }
                        }
                    }
                }
            }
            return new Point();
        }
    }
}
