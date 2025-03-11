using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using yWorks.yFiles.UI;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.UI
{
    class UIUtil
    {
        public static string ReadFileFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static byte[] ReadFileContentFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            byte[] content = null;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                content = new byte[stream.Length];
                stream.Read(content, 0, (int)stream.Length);
                return content;
            }
        }

        public static string getCurrentLocation()
        {
            return Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
        }


        public static MessageBoxResult ShowMessageBox(string message, string caption, MessageBoxButton btn, MessageBoxImage icon, Window sender)
        {
            return MessageBox.Show(sender, message, caption, btn, icon, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static MessageBoxResult ShowMessageBox(int fldCodMsg, Window sender = null)
        {
            TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == fldCodMsg);
            if (sender != null)
            {
                return MessageBox.Show(sender, msg.FldTxtMsg, msg.FldTtlMsg, GetMessageBoxButton(msg.FldTypMsg), GetMessageBoxIcon(msg.FldTypMsg), MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            else
            {
                return MessageBox.Show(msg.FldTxtMsg, msg.FldTtlMsg, GetMessageBoxButton(msg.FldTypMsg), GetMessageBoxIcon(msg.FldTypMsg), MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
        }

        public static MessageBoxResult ShowMessageBox(int fldCodMsg, string placeHolder, Window sender)
        {
            TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == fldCodMsg);
            return MessageBox.Show(sender, string.Format(msg.FldTxtMsg, placeHolder), msg.FldTtlMsg, GetMessageBoxButton(msg.FldTypMsg), GetMessageBoxIcon(msg.FldTypMsg), MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private static MessageBoxImage GetMessageBoxIcon(int type)
        {
            var mTtype = (SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType)type;
            switch (mTtype)
            {
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Information:
                    return MessageBoxImage.Information;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Error:
                    return MessageBoxImage.Error;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Warning:
                    return MessageBoxImage.Warning;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Confirmation:
                    return MessageBoxImage.Question;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Question:
                    return MessageBoxImage.Question;

                default:
                    return MessageBoxImage.Exclamation;
            }
        }

        private static MessageBoxButton GetMessageBoxButton(int type)
        {
            var mTtype = (SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType)type;
            switch (mTtype)
            {
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Information:
                    return MessageBoxButton.OK;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Error:
                    return MessageBoxButton.OK;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Warning:
                    return MessageBoxButton.OK;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Confirmation:
                    return MessageBoxButton.YesNo;

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Question:
                    return MessageBoxButton.YesNo;

                default:
                    return MessageBoxButton.OK;
            }
        }


        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }


        public static void SelectFirstDataItem(System.Windows.Controls.Primitives.Selector grd)
        {
            return;

            var itm = grd.SelectedItem;
            if (itm != null)
            {
                grd.SelectedItem = null;
                grd.SelectedItem = itm;
            }
            else
            {
                if (grd.Items.Count > 0)
                {
                    grd.SelectedItem = grd.Items[0];
                }
                else
                {
                    grd.SelectedItem = null;
                }
            }
        }

        public static void SelectFirstDataItem(Telerik.Windows.Controls.RadTreeView grd)
        {
            return;

            var itm = grd.SelectedItem;
            if (itm != null)
            {
                grd.SelectedItem = null;
                grd.SelectedItem = itm;
            }
            else
            {
                if (grd.Items.Count > 0)
                {
                    grd.SelectedItem = grd.Items[0];
                }
                else
                {
                    grd.SelectedItem = null;
                }
            }
        }


        public static void SelectFirstDataItem(Telerik.Windows.Controls.DataControl grd)
        {
            return;

            var itm = grd.SelectedItem;
            if (itm != null)
            {
                grd.SelectedItem = null;
                grd.SelectedItem = itm;
            }
            else
            {
                if (grd.Items.Count > 0)
                {
                    grd.SelectedItem = grd.Items[0];
                }
                else
                {
                    grd.SelectedItem = null;
                }
            }
        }


        public static INode findNodByTag(GraphControl graphControl, object tag, out INode parentNod)
        {
            parentNod = null;
            List<INode> nods = new List<INode>(graphControl.Graph.Nodes);

            INode curr = nods.FirstOrDefault(n => n.Tag == tag);

            if (curr != null)
            {
                var org = curr.Tag as SSYM.OrgDsn.Model.Base.ISearchableTree;
                var parentOrg = org.SearchParent;
                parentNod = nods.SingleOrDefault(n => n.Tag == parentOrg);
                return curr;
            }

            return null;
        }


        public static List<INode> findAllChilds(IGraph gc, INode nod)
        {
            List<INode> result = new List<INode>();

            Queue<INode> q = new Queue<INode>();

            q.Enqueue(nod);

            while (q.Count > 0)
            {
                var nod1 = q.Dequeue();
                result.Add(nod1);
                foreach (var item in gc.Successors(nod1))
                {
                    q.Enqueue(item);
                }
            }
            return result;

        }

        public static Exception GetInnerExceptionRecusive(Exception ex)
        {
            Exception currentEX = ex;
            while (currentEX.InnerException != null)
            {
                currentEX = currentEX.InnerException;
            }
            return currentEX;
        }
    }
}
