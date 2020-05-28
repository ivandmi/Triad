﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using DrawingPanel;
using TriadCompiler;
using TriadNSim.Ontology;

//????????
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using TriadCore;

namespace TriadNSim.Forms
{

    public partial class frmMain : Form
    {
        private DrawingPanel.BaseObject[] m_oSelectedObjects;
        private frmChangeRoutine m_frmChangeRoutine;
        private string m_sFileName = string.Empty;
        private string sUserIPFileName = "IP.dat";
        private string sSimCondFileName = "SimCond.dat";
        private Dictionary<ListViewItem, Bitmap> ItemImages = new Dictionary<ListViewItem, Bitmap>();
        private COWLOntologyManager ontologyManager;
        public const string sOntologyPath = "Ontologies\\ComputerNetwork.owl";
        public AppDomain domain = null;
        
        /// <summary>
        /// Экземпляр формы
        /// </summary>
        private static frmMain instance = null;
        public List<InfProcedure> userIProcedures = null;
        public List<InfProcedure> standartIProcedures = null;
        public List<SimCondition> simConditions = null;

        public frmMain()
        {
            InitializeComponent();
            m_frmChangeRoutine = new frmChangeRoutine(drawingPanel1);
            ontologyManager = new COWLOntologyManager(sOntologyPath);
            m_frmChangeRoutine.OnNameChecked += delegate(object sender, CancelEventArgs args)
            {
                if (ontologyManager.GetClass(m_frmChangeRoutine.DesignTypeName) != null)
                    args.Cancel = true;
            };
            LoadUserIP();
            LoadStandartIP();
            LoadSimConditions();
            LoadElements();
        }

        /// <summary>
        /// Экземпляр формы
        /// </summary>
        public static frmMain Instance
        {
            get
            {
                if (instance == null)
                    instance = new frmMain();
                return instance;
            }
        }

        public COWLOntologyManager OntologyManager
        {
            get
            {
                return ontologyManager;
            }
        }

        public DrawingPanel.DrawingPanel Panel
        {
            get
            {
                return drawingPanel1;
            }
        }

        public void SaveUserIP()
        {
            Stream StreamWrite = File.Open(sUserIPFileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter BinaryWrite = new BinaryFormatter();
            BinaryWrite.Serialize(StreamWrite, userIProcedures);
            StreamWrite.Close();
        }

        private void LoadUserIP()
        {
            if (File.Exists(sUserIPFileName))
            {
                Stream StreamRead = File.Open(sUserIPFileName, FileMode.Open, FileAccess.Read);
                if (StreamRead.Length > 0)
                {
                    BinaryFormatter BinaryRead = new BinaryFormatter();
                    userIProcedures = (List<InfProcedure>)BinaryRead.Deserialize(StreamRead);
                }
                else
                    userIProcedures = new List<InfProcedure>(); 
                StreamRead.Close();
            }
            else
                userIProcedures = new List<InfProcedure>();   
        }

        public void SaveSimConditions()
        {
            Stream StreamWrite = File.Open(sSimCondFileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter BinaryWrite = new BinaryFormatter();
            BinaryWrite.Serialize(StreamWrite, simConditions);
            StreamWrite.Close();
        }

        private void LoadSimConditions()
        {
            if (File.Exists(sSimCondFileName))
            {
                Stream StreamRead = File.Open(sSimCondFileName, FileMode.Open, FileAccess.Read);
                if (StreamRead.Length > 0)
                {
                    BinaryFormatter BinaryRead = new BinaryFormatter();
                    simConditions = (List<SimCondition>)BinaryRead.Deserialize(StreamRead);
                }
                else
                    simConditions = new List<SimCondition>(); 
                StreamRead.Close();
            }
            else
                simConditions = new List<SimCondition>();   
        }

        private void LoadStandartIP()
        {
            standartIProcedures = new List<InfProcedure>();
            List<IProcedureType> iprocedures = SimConditionParser.StandartIP;
            foreach (var ip in iprocedures)
                standartIProcedures.Add(new InfProcedure(ip, true));
        }

        public InfProcedure GetUserIP(string Name)
        {
            foreach (var proc in userIProcedures)
            {
                if (proc.Name == Name)
                    return proc;
            }
            return null;
        }

        public bool DeleteUserIP(string Name)
        {
            foreach (var ip in userIProcedures)
            {
                if (ip.Name == Name)
                {
                    userIProcedures.Remove(ip);
                    SaveUserIP();
                    return true;
                }
            }
            return false;
        }

        public SimCondition GetSimCondition(string Name)
        {
            foreach (var simCond in simConditions)
            {
                if (simCond.Name == Name)
                    return simCond;
            }
            return null;
        }

        public bool DeleteSimCondition(string Name)
        {
            foreach (var simCond in simConditions)
            {
                if (simCond.Name == Name)
                {
                    simConditions.Remove(simCond);
                    SaveSimConditions();
                    return true;
                }
            }
            return false;
        }

        public void Open()
        {
            string sNewFileName = drawingPanel1.Loader();
            if (sNewFileName != string.Empty)
            {
                this.m_sFileName = sNewFileName;
                this.Text = Util.ApplicationName + " [" + System.IO.Path.GetFileName(this.m_sFileName) + "]";
            }
        }

        public int GetEndModelTime()
        {
            int iResult;
            try
            {
                iResult = Int32.Parse(tstModelTime.Text);
            }
            catch
            {
                tstModelTime.Text = "100";
                iResult = 100;
            }
            return iResult;
        }

        public void Save(string fileName)
        {
            string sNewFileName = drawingPanel1.Saver(fileName);
            if (sNewFileName != string.Empty)
            {
                this.m_sFileName = sNewFileName;
                this.Text = this.m_sFileName;
            }
        }

        public void UpdateZoom()
        {
            try
            {
                string strZoom = toolStripcmbZoom.Text.Trim();
                if (strZoom.EndsWith("%")) strZoom = strZoom.Substring(0, strZoom.Length - 1);
                int value = Int32.Parse(strZoom);
                drawingPanel1.Zoom = value / 100.0f;
            }
            catch
            {
                toolStripcmbZoom.Text = Convert.ToString(drawingPanel1.Zoom * 100) + "%";
            }
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void miSave_Click(object sender, EventArgs e)
        {
           Save(m_sFileName);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripcmbZoom.Text = Convert.ToString(drawingPanel1.Zoom * 100) + "%";
            drawingPanel1.CurrentTool = DrawingPanel.ToolType.ttSelect;
            toolStripbtnSelect.Checked = true;
            listView1.Items[0].Tag =new object[2] { "Workstation", ENetworkObjectType.Client };
            listView1.Items[1].Tag = new object[2] { "Server", ENetworkObjectType.Server };
            listView1.Items[2].Tag = new object[2] { "Router", ENetworkObjectType.Undefined };
            listView1.Items[3].Tag = new object[2] { "UserObj", ENetworkObjectType.UserObject };
            listView1.Items[4].Tag = new object[2] { "Vertex", ENetworkObjectType.UserObject };
        }

        private void toolStripcmbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        private void toolStripcmbZoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                UpdateZoom();
        }

        private void toolStripbtnLink_Click(object sender, EventArgs e)// Cоединить
        {
            drawingPanel1.CurrentTool = DrawingPanel.ToolType.ttLine;
            toolStripbtnSelect.Checked = false;
            toolStripbtnLink.Checked = true;
        }

        private void toolStripbtnSelect_Click(object sender, EventArgs e)
        {
            drawingPanel1.CurrentTool = DrawingPanel.ToolType.ttSelect;
            toolStripbtnSelect.Checked = true;
            toolStripbtnLink.Checked = false;
        }

        private void miRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void toolStripbtnOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void toolStripbtnSave_Click(object sender, EventArgs e)
        {
            Save(m_sFileName);
        }

        private void toolStripbtnRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run(bool bSelectSimCondition = false)
        {
            //m_oSimulation.Start(true);
            SimulationInfo simInfo = new SimulationInfo(drawingPanel1.Shapes, GetEndModelTime());

            if (simInfo.Nodes.Count == 0)
            {
                Util.ShowErrorBox("Сеть пуста");
                return;
            }
            bool bFind = false;
            foreach (NetworkObject obj in simInfo.Nodes)
            {
                if (obj.Routine == null)
                {
                    bFind = true;
                    break;
                }
            }
            
            if (bFind)
            {
                if (Util.ShowConformationBox("У некоторых элементов неопределенное поведение. Доопределить автоматически?"))
                {
                    if (!ontologyManager.Complete(simInfo))
                    {
                        Util.ShowErrorBox(ontologyManager.sCompleteError);
                        return;
                    }
                }
                else
                    return;
            }

            if (bSelectSimCondition && simConditions.Count > 0)
            {
                frmSimulate frmSim = new frmSimulate(simInfo);
                if (frmSim.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Simulation.Instance.Begin(simInfo);
            }
            catch(Exception ex)
            {
                Util.ShowErrorBox(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tsbCalcStaticProp_Click(object sender, EventArgs e)
        {
            if (drawingPanel1.Shapes.Count == 0)
            {
                Util.ShowErrorBox("Сеть пуста");
                return;
            }
            frmCalculate frmCalc = new frmCalculate(drawingPanel1);
            frmCalc.ShowDialog();
        }

        private void drawingPanel1_objectSelected(object sender, PropertyEventArgs e)
        {
            m_oSelectedObjects = e.ele;
        }

        private void drawingPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pntShow = drawingPanel1.PointToScreen(e.Location); 
                if (m_oSelectedObjects != null && m_oSelectedObjects.Length == 1)
                {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    NetworkObject obj = m_oSelectedObjects[0] as NetworkObject;
                    if (obj != null)
                    {
                        ToolStripMenuItem miRoutine = (ToolStripMenuItem)menu.Items.Add("Рутина", null);
                        if (obj.Routine != null)
                        {
                            menu.Items.Add("Информационные процедуры", null, miIP_Click);
                            menu.Items.Add("Параметры", null, miCharacteristics_Click);
                        }
                        {
                            miRoutine.DropDownItems.Add("Нет", null, ResetRoutine);
                            if (obj.Type == ENetworkObjectType.UserObject)
                                miRoutine.DropDownItems.Add("Пользовательская", null, miUserObjectChangeRoutine_Click);
                            //if (obj.Type == ENetworkObjectType.UserObject)
                            //    miRoutine.DropDownItems.Add("Пользовательская", null, miUserObjectChangeRoutine_Click);
                            bool bFind = false;
                            IOWLClass semanticType = ontologyManager.GetClass(obj.SemanticType);
                            if (semanticType!=null)//
                            foreach (var indiv in ontologyManager.GetIndividuals(semanticType))
                            {
                                IOWLClass cls = ontologyManager.GetIndividualClass(indiv);
                                string sName = cls.Comment;
                                if (sName.Length == 0)
                                    sName = cls.Name;
                                ToolStripMenuItem mi = (ToolStripMenuItem)miRoutine.DropDownItems.Add(sName, null, SelectRoutine);
                                mi.Tag = cls.Name;
                                if (obj.Routine != null && !bFind && obj.Routine.Type == mi.Tag.ToString())
                                {
                                    mi.Checked = true;
                                    bFind = true;
                                }
                            }
                            if (!bFind)
                            {
                                int iIndex = obj.Type == ENetworkObjectType.UserObject && obj.Routine != null ? 1 : 0;
                                ((ToolStripMenuItem)miRoutine.DropDownItems[iIndex]).Checked = true;
                            }
                        }
                        menu.Items.Add("Изменить изображение", null, miUserObjectImage_Click);    
                    }
                    else
                        menu.Items.Add("Изменить", null, miLinkChange_Click);

                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("Удалить", null, miDelete_Click);
                    menu.Show(this, pntShow, ToolStripDropDownDirection.AboveRight);
                }
            }
        }


        public NetworkObject GetSelectedObject()
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return null;
            return m_oSelectedObjects[0] as NetworkObject;
        }

        public void SelectRoutine(Object sender, EventArgs e)
        {
            NetworkObject obj = GetSelectedObject();
            if (obj != null)
            {
                string sType = ((ToolStripMenuItem)sender).Tag.ToString();
                IOWLClass cls = ontologyManager.GetClass(sType);
                if (!ontologyManager.CompleteNode(new SimulationInfo(drawingPanel1.Shapes, GetEndModelTime()), obj, ontologyManager.GetIndividuals(cls)[0]))
                {
                    Util.ShowErrorBox("Не удалось наложить рутину. Неподходящее кол-во дуг и/или типы соседей");
                }
            }
        }

        public void ResetRoutine(Object sender, EventArgs e)
        {
            NetworkObject obj = GetSelectedObject();
            if (obj != null && obj.Routine != null)
            {
                foreach (BaseObject baseobj in drawingPanel1.Shapes)
                {
                    if (baseobj is Link)
                    {
                        Link link = baseobj as Link;
                        if (link.FromCP.Owner == obj)
                            link.PolusFrom = null;
                        if (link.ToCP.Owner == obj)
                            link.PolusTo = null;
                    }
                }
                obj.Routine = null;
            }
        }

        private void miUserObjectChangeRoutine_Click(object sender, EventArgs e)
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return;
            NetworkObject obj = m_oSelectedObjects[0] as NetworkObject;
            if (obj.Type != ENetworkObjectType.UserObject)
                return;
            Routine prevRout = obj.Routine;
            if (prevRout == null || obj.Routine.Type.Length > 0)
            {
                obj.Routine = new Routine();
                obj.Routine.Name = "R" + obj.Name;
                obj.Routine.Text = "routine " + obj.Routine.Name + "(InOut pol)\nendrout";
                obj.Routine.Poluses.Add(new Polus("pol"));
            }
            m_frmChangeRoutine.SetObject(obj);
            m_frmChangeRoutine.ShowDialog();
            if (m_frmChangeRoutine.Successed)
            {
                frmChangeRoutine.SaveLastCompiledRoutine(obj.Routine.Name + ".dll");
                obj.Routine.Type = string.Empty;
            }
            else
                obj.Routine = prevRout;
        }

        public static Assembly GenerateAssemblyFromFile(string sFile, string[] references = null)
        {
            if (File.Exists(sFile))
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.CompilerOptions = "/t:library";
                compilerParameters.GenerateInMemory = true;
                //compilerParameters.GenerateInMemory = false;
                //compilerParameters.OutputAssembly = "SimCondition.dll";
                compilerParameters.ReferencedAssemblies.Add("system.dll");
                compilerParameters.ReferencedAssemblies.Add("TriadCore.dll");
                compilerParameters.ReferencedAssemblies.Add("TriadNSim.exe");
                if (references != null)
                {
                    foreach (string reference in references)
                        compilerParameters.ReferencedAssemblies.Add(reference.ToLower());
                }
                return new CSharpCodeProvider().CompileAssemblyFromFile(compilerParameters, sFile).CompiledAssembly;
            }
            return null;
        }

        private void miHelp_Click(object sender, EventArgs e)
        {

        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            Save(string.Empty);
        }

        private void toolStripbtnNew_Click(object sender, EventArgs e)
        {
            drawingPanel1.Clear();
        }

        private void miPrintPreview_Click(object sender, EventArgs e)
        {
            drawingPanel1.PrintPreview(1);
        }

        private void miUserObjectImage_Click(object sender, EventArgs e)
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return;
            NetworkObject obj = m_oSelectedObjects[0] as NetworkObject;
            //if (obj.Type != ENetworkObjectType.UserObject)
            //    return;
            obj.Load_IMG();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            base.DoDragDrop(e.Item,DragDropEffects.Move);
        }

        private void drawingPanel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
                e.Effect = DragDropEffects.Move;
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (listView1.GetItemAt(e.X, e.Y) != null)
                Cursor = Cursors.SizeAll;
            else
                Cursor = Cursors.Default;
        }


        public Boolean EditLink(Link oLink, Boolean bNeedShowDialog)
        {
            ModifyRelation frmRelation = new ModifyRelation(oLink);
            if (frmRelation.Success && !bNeedShowDialog || frmRelation.ShowDialog() == DialogResult.OK)
            {
                oLink.PolusFrom = frmRelation.From != null ? frmRelation.From.Name : null;
                oLink.PolusTo = frmRelation.To != null ? frmRelation.To.Name : null;
                return true;
            }
            return false;
        }

        public Link AddLink(CConnectionPoint From, CConnectionPoint To)
        {
            Link r = new Link(drawingPanel1, From, To);
            drawingPanel1.ShapeCollection.AddShape(r);
            return r;
        }

        private Dictionary<string, bool> GetShapeNames()
        {
            Dictionary<string, bool> ShapeNames = new Dictionary<string, bool>();
            foreach (BaseObject obj in drawingPanel1.Shapes)
            {
                if (obj.Name != null && obj.Name.Length > 0)
                    ShapeNames[obj.Name.ToLower()] = true;
            }
            return ShapeNames;
        }

        private string GetUniqueShapeName(string sName)
        {
            int nIndex = 1;
            string sRes = sName.ToLower() + nIndex.ToString();
            Dictionary<string, bool> names = GetShapeNames();
            while(names.ContainsKey(sRes))
            {
                nIndex++;
                sRes = sName.ToLower() + nIndex.ToString();
            }
            return sName + nIndex.ToString();
        }

        private void drawingPanel1_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            Point pt = drawingPanel1.PointToClient(new Point(e.X, e.Y));
            object[] tag = li.Tag as object[];
            ENetworkObjectType type = (ENetworkObjectType)tag[1];

            float fZoom = drawingPanel1.Zoom;
            int delta = 100;
            
            int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
            int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
            NetworkObject shape = new NetworkObject(drawingPanel1);
            shape.Type = type;
            shape.Rect = new Rectangle(X, Y, delta, delta);
            shape.Name = GetUniqueShapeName(li.Text);
            if(type!=ENetworkObjectType.UserObject)
                shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
            if (ItemImages.ContainsKey(li))
            {
                shape.img = new Bitmap(ItemImages[li]);
                shape.showBorder = false;//type == ENetworkObjectType.UserObject;
                shape.Trasparent = false;
            }
            else{//!!
           
               shape.showBorder = false;
            }
            drawingPanel1.ShapeCollection.AddShape(shape);

            drawingPanel1.Focus();
        }

        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            drawingPanel1.DeleteSelected();
        }

        private void miLinkChange_Click(object sender, EventArgs e)
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return;
            Link obj = m_oSelectedObjects[0] as Link;
            if (obj == null)
                return;
            EditLink(obj, true);
        }

        private void miCharacteristics_Click(object sender, EventArgs e)
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return;
            NetworkObject obj = m_oSelectedObjects[0] as NetworkObject;
            if (obj.Routine == null || obj.Routine.Parameters.Count == 0)
                return;

            frmEditParam param = new frmEditParam(obj.Routine.Parameters, obj.Routine.ParameterValues);
            param.Descriptions = obj.Routine.ParamDescription.ToArray();
            if (param.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                obj.Routine.ParameterValues = param.Values;
                obj.Routine.ParamDescription = param.Descriptions.ToList();
            }
        }

        private void miIP_Click(object sender, EventArgs e)
        {
            if (m_oSelectedObjects == null || m_oSelectedObjects.Length != 1)
                return;
            frmObjectIPs frmObjectIPs = new frmObjectIPs(m_oSelectedObjects[0] as NetworkObject);
            frmObjectIPs.ShowDialog();
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new frmIProcedures().ShowDialog();
        }

        private void drawingPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BaseObject selObj = drawingPanel1.ShapeCollection.selectedObj;
            if (selObj is Link)
                EditLink(selObj as Link, true);
        }

        private void drawingPanel1_beforeAddLine(object sender, BeforeAddLineEventArgs e)
        {
            e.cancel = true;
            NetworkObject objFrom = e.fromCP.Owner as NetworkObject;
            NetworkObject objTo = e.toCP.Owner as NetworkObject;
            //if (drawingPanel1.ShapeCollection.GetLine(objFrom, objTo) == null) -ЕСЛИ НЕТ РЕБРА ТО ДОБАВЛЯЕМ
           // {
                Link oLink = new Link(drawingPanel1, e.fromCP, e.toCP);
               // if (EditLink(oLink, false))
                    drawingPanel1.ShapeCollection.AddShape(oLink);
            //}
        }

        private void drawingPanel1_onLineCPChanged(object sender, OnLineCPChangedEventArgs e)
        {
            Link link = e.line as Link;
            if (link != null)
            {
                if (e.fromChanged)
                    link.PolusFrom = null;
                else
                    link.PolusTo = null;
                if (!EditLink(link, false))
                    drawingPanel1.Undo();
            }
        }

        public bool ContainsElement(string sName)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (String.Compare(sName, item.Text, true) == 0)
                    return true;
            }
            return false;
        }

        private void сMenuItemsAdd_Click(object sender, EventArgs e)
        {
            frmAddElement frm = new frmAddElement();
            frm.Bmp = global::TriadNSim.Properties.Resources.question;
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IOWLClass superClass = ontologyManager.GetClass(frm.ParentName);
                IOWLClass cls = ontologyManager.AddClass(frm.Name);
                ontologyManager.AddSubClass(cls, superClass);
                IOWLClass routClass = ontologyManager.AddRoutine(ontologyManager.GetRoutineClass(superClass), cls, frm.Name);
                ontologyManager.SaveOntology(sOntologyPath);

                imageList2.Images.Add(frm.Bmp);
                int nImageIndex = imageList2.Images.Count - 1;
                ListViewItem li = listView1.Items.Add(frm.Name, nImageIndex);
                li.Tag = new object[2] { frm.Name, ENetworkObjectType.Undefined };
                ItemImages[li] = frm.Bmp;
            }
        }

        public static Bitmap LoadImage(string sTitle)
        {
            Bitmap bmp = null;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = sTitle;
            dlg.Filter = "Image Files(*.BMP;*.JPG;*.PNG;*.GIF)|*.BMP;*.JPG;*.PNG;*.GIF|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bmp = new Bitmap(dlg.FileName);
                }
                catch (Exception ex)
                {
                    bmp = null;
                }
            }
            return bmp;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveImageList();
            base.OnFormClosing(e);
        }
        
        private void SaveImageList()
        {
            XmlWriter writer = XmlWriter.Create("elements.xml");
            writer.WriteStartElement("elements");
            foreach (KeyValuePair<ListViewItem, Bitmap> pair in ItemImages)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", pair.Key.Text);
                MemoryStream ms = new MemoryStream();
                pair.Value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                writer.WriteBase64(ms.ToArray(), 0, (int)ms.Length);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }


        private void LoadElements()
        {
            
            listView1.Clear();
            imageList2.Images.Clear();

           string[] standartItems = { "Рабочая станция", "Сервер", "Маршрутизатор", "Пользовательский", "Вершина" };
           //string[] standartItems = { "Workstation", "Server", "Router", "Custom" };
            string[] standartItemNames = { "Workstation", "Server", "Router", "ComputerNetworkNode", "Vertex" };
            ENetworkObjectType[] types = { ENetworkObjectType.Client, ENetworkObjectType.Server, ENetworkObjectType.Undefined, ENetworkObjectType.UserObject, ENetworkObjectType.Vertex };
            Dictionary<string, ListViewItem> Items = new Dictionary<string, ListViewItem>();
            for(int i = 0; i < standartItems.Length; i++)
            {
                string sName = standartItems[i];
                ListViewItem item = listView1.Items.Add(sName);
                item.Tag = new object[2] { standartItemNames[i], types[i] };
                Items[sName.ToLower()] = item;
            }

            foreach (IOWLClass cls in ontologyManager.GetComputerNetworkElements())
            {
                string sName = cls.Comment;
                if (sName.Length == 0)
                    sName = cls.Name;
                if (Items.ContainsKey(sName.ToLower()))
                    continue;
                ListViewItem item = listView1.Items.Add(sName);
                item.Tag = new object[2] { cls.Name, ENetworkObjectType.Undefined };
                Items[sName.ToLower()] = item;
            }

            Dictionary<string, Bitmap> images = LoadImageList();
            foreach (KeyValuePair<string, Bitmap> pair in images)
            {
                if (!Items.ContainsKey(pair.Key))
                    continue;
                ItemImages[Items[pair.Key]] = pair.Value;
                imageList2.Images.Add(pair.Value);
                Items[pair.Key].ImageIndex = imageList2.Images.Count - 1;
            }
        }

        private Dictionary<string, Bitmap> LoadImageList()
        {
            Dictionary<string, Bitmap> res = new Dictionary<string, Bitmap>();
            if (File.Exists("elements.xml"))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                XmlDocument doc = new XmlDocument();
                doc.Load("elements.xml");
                int nCount = doc.ChildNodes.Count;
                for (int i = 0; i < nCount; i++)
                {
                    XmlNode node = doc.ChildNodes[i];
                    if (node.Name == "elements")
                    {
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            string sName = item.Attributes["name"].Value;
                            string sBase64 = item.InnerXml;
                            if (sBase64.Length > 0)
                            {
                                byte[] buffer = Convert.FromBase64String(sBase64);
                                MemoryStream ms = new MemoryStream();
                                ms.Write(buffer, 0, buffer.Length);
                                res[sName.ToLower()] = new Bitmap(ms);
                            }
                        }
                        break;
                    }
                }
            }
            return res;
        }

        private void сMenuItemsRoutines_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            string sName = (string)(item.Tag as object[])[0];
            IOWLClass cls = ontologyManager.GetClass(sName);
            if (cls == null)
            {
                Util.ShowErrorBox("Элемент '" + sName + "' не найден");
                return;
            }
            frmRoutines frm = new frmRoutines(cls);
            frm.ShowDialog();
        }
        
        private void ChangeElementImage(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            if (item != null)
            {
                Bitmap bmp = LoadImage("Изображение элемента '" + item.Text + "'");
                if (bmp != null)
                {
                    ItemImages[item] = bmp;
                    imageList2.Images.Add(bmp);
                    item.ImageIndex = imageList2.Images.Count - 1;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            foreach (BaseObject obj in drawingPanel1.Shapes)
            {
                if (obj is NetworkObject)
                    (obj as NetworkObject).Routine = null;
                else if (obj is Link)
                {
                    Link link = obj as Link;
                    link.PolusFrom = null;
                    link.PolusTo = null;
                }
            }
        }

        private void drawingPanel1_objectDeleted(object sender, ObjectEventArgs e)
        {
            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmIConditions frm = new frmIConditions();
            frm.ShowDialog();
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    ENetworkObjectType type = ENetworkObjectType.Undefined;
                    object[] tag = null;
                    if (item.Tag != null)
                    {
                        tag = item.Tag as object[];
                        type = (ENetworkObjectType)tag[1];
                    }
                    if (type != ENetworkObjectType.UserObject)
                    {
                        menu.Items.Add("Поведения элемента", null, сMenuItemsRoutines_Click);
                        menu.Items.Add("Изменить изображение", null, ChangeElementImage);
                        if (type == ENetworkObjectType.Undefined && tag[0].ToString() != "Router")
                            menu.Items.Add("Удалить элемент", null, cMenuItemsDelElement_Click);
                    }
                }
                else
                {
                    menu.Items.Add("Добавить элемент", null, сMenuItemsAdd_Click);
                }
                if (menu.Items.Count > 0)
                    menu.Show(listView1, e.Location, ToolStripDropDownDirection.AboveRight);
            }
        }


        private void cMenuItemsDelElement_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            if (Util.ShowConformationBox("Удалить элемент '" + item.Text + "'?"))
            {
                ontologyManager.RemoveClass(ontologyManager.GetClass((item.Tag as object[])[0].ToString()));
                ontologyManager.SaveOntology(sOntologyPath);
                ItemImages.Remove(item);
                listView1.Items.Remove(item);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        private void toolStripbtnRun_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItem1.Enabled = simConditions.Count > 0;
        }

        private void btnDefine_Click(object sender, EventArgs e)
        {
            SimulationInfo simInfo = new SimulationInfo(drawingPanel1.Shapes, GetEndModelTime());
            if (!ontologyManager.Complete(simInfo))
                Util.ShowErrorBox(ontologyManager.sCompleteError);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveImageList();
        }

        //private void Models(object sender, FormClosedEventArgs args)
        //{
        //    if (StatClass.CountOfVertex != 0)
        //    {
        //        if (StatClass.Erd)
        //        {
        //            //Erdos_Renyi();
        //        }
        //        else
        //        {
        //            if (StatClass.Barabash)
        //            { Barabashi_Albert(); }

        //            //else { Bollobash_Riordan(); }
        //        }
        //    }
        //}
    

        private void Erdos_Renyi(double p, int count, string NodeName)
        {
            int XC = 550;
            int YC = 350;
            //int count = StatClass.CountOfVertex;
            int X0 = 0;
            int Y0 = 0;
            double a = Math.PI / count * 2;
            //double p = StatClass.Probability;
            int r = 250;
            Random rand = new Random();
            NetworkObject[] shapeArray = new NetworkObject[count + 1];
            double angle = a;
            for (int i = 1; i <= count; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += a;
                ListViewItem li = listView1.Items[5];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 20;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                //shape.Name = GetUniqueShapeName(li.Text);
                shape.Name = NodeName + "[" + (i - 1) + "]";
                shapeArray[i] = shape;
                for (int j = 1; j < i; j++)
                {
                    double V = rand.Next(0, 1000) / 1000.0;
                    if ((drawingPanel1.ShapeCollection.GetLine(shape, shapeArray[j]) == null) && (i > 1) && (V < p))
                    {
                        Link oLink = new Link(drawingPanel1, shape.ConnectionPoint, shapeArray[j].ConnectionPoint);
                        oLink.PolusFrom = "pol";
                        oLink.PolusTo = "pol";
                        //if (EditLink(oLink, false))
                            drawingPanel1.ShapeCollection.AddShape(oLink);
                        //CoreName polus = new CoreName("p1");
                        //oGraph.DeclarePolusInAllNodes(polus);
                        //oGraph.AddEdge(new CoreName(oLink.FromCP.Owner.Name), polus, new CoreName(oLink.ToCP.Owner.Name), polus);
                    }
                }
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {

                    shape.showBorder = true;
                }
                //shape.Routine = new Routine();
                //shape.Routine.Name = "MyRout";
                //shape.Routine.Poluses = new List<Polus> { new Polus("pol")};
                //shape.Routine.Parameters = new List<IExprType> { new VarType(TriadCompiler.TypeCode.Boolean,"Defence")};
                //shape.Routine.Parameters.Add(new VarType(TriadCompiler.TypeCode.Boolean,"isbad"));
                //shape.Routine.Parameters.Add(new VarType(TriadCompiler.TypeCode.Real,"beta"));
                //shape.Routine.Parameters.Add(new VarType(TriadCompiler.TypeCode.Real,"gama"));
                //shape.Routine.ParameterValues = new List<object>();
                //shape.Routine.ParameterValues.Add(false);
                //shape.Routine.ParameterValues.Add(i==1 ? true : false);
                //shape.Routine.ParameterValues.Add(0.7);
                //shape.Routine.ParameterValues.Add(0.25);
                //shape.Routine.Variables = shape.Routine.Parameters;
                drawingPanel1.ShapeCollection.AddShape(shape);
            }

            drawingPanel1.Focus();
        }

        NetworkObject[] shapeArray = new NetworkObject[100 + 1];
        Graph oGraph = new Graph();
        List<NetworkObject> lstObjects = new List<NetworkObject>();
        List<Link> lstLinks = new List<Link>();
        double sum = 0;

        private void Start_BA(int count)
        {
            int n = 3; // кол-во вершин в начальном графе-затравке
            //int count = StatClass.CountOfVertex;
            int XC = 550;
            int YC = 350;
            int X0 = 0;
            int Y0 = 0;
            double a = Math.PI / count * 2;
            int r = 250;
            Random rand = new Random();
            double angle = a;
            for (int i = 1; i <= n; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += a;
                ListViewItem li = listView1.Items[4];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 20;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                shape.Name = GetUniqueShapeName(li.Text);
                shapeArray[i] = shape;
                oGraph.DeclareNode(new CoreName(shape.Name));
                lstObjects.Add(shape as NetworkObject);
                for (int j = 1; j < i; j++)
                {
                    if ((drawingPanel1.ShapeCollection.GetLine(shape, shapeArray[j]) == null) && (i > 1))
                    {
                        Link oLink = new Link(drawingPanel1, shape.ConnectionPoint, shapeArray[j].ConnectionPoint);
                        if (EditLink(oLink, false))
                            drawingPanel1.ShapeCollection.AddShape(oLink);
                        lstLinks.Add(oLink as Link);
                        CoreName polus = new CoreName("p1");
                        oGraph.DeclarePolusInAllNodes(polus);
                        oGraph.AddEdge(new CoreName(oLink.FromCP.Owner.Name), polus, new CoreName(oLink.ToCP.Owner.Name), polus);
                    }
                }
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {
                    shape.showBorder = true;
                }
                drawingPanel1.ShapeCollection.AddShape(shape);

            }

            drawingPanel1.Focus();
            for (int jj = 0; jj < oGraph.NodeCount; jj++)
            {
                sum += StandartFunctions.GetNodeDegree(oGraph[jj]);
            }
        }

        private void Barabashi_Albert(int count,int m)
        {
            sum = 0;
            //int m = StatClass.CountStep; // Кол-во ребер на каждом шаге
            int mm = 0;
            int XC = 550;
            int YC = 350;
            //int count = StatClass.CountOfVertex; // Кол-во вершин в графе
            int X0 = 0;
            int Y0 = 0;
            double a = Math.PI / count * 2;
            double p = 0; //вероятность, с которой появляется ребро
            int r = 250;
            Random rand = new Random();

            int k = 0;
            // NetworkObject[] shapeArray = new NetworkObject[count + 1];
            double angle = a * 4; // на что умножать зависит от начального графа

            Start_BA(count);
            for (int i = 4; i <= count; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += a;
                ListViewItem li = listView1.Items[4];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 20;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                shape.Name = GetUniqueShapeName(li.Text);
                shapeArray[i] = shape;
                oGraph.DeclareNode(new CoreName(shape.Name));
                lstObjects.Add(shape as NetworkObject);
                while (mm < m)
                {
                    for (int j = 1; j < i; j++)
                    {
                        double V = rand.Next(0, 100) / 100.0;
                        int ccc = oGraph.NodeCount;
                        p = StandartFunctions.GetNodeDegree(oGraph[j - 1]) / sum;//(2 * oGraph.NodeCount - 1);
                        if ((drawingPanel1.ShapeCollection.GetLine(shape, shapeArray[j]) == null) && (i > 1) && (V < p))
                        {
                            Link oLink = new Link(drawingPanel1, shape.ConnectionPoint, shapeArray[j].ConnectionPoint);
                            if (EditLink(oLink, false))
                                drawingPanel1.ShapeCollection.AddShape(oLink);
                            lstLinks.Add(oLink as Link);
                            CoreName polus = new CoreName("p1");
                            oGraph.DeclarePolusInAllNodes(polus);
                            oGraph.AddEdge(new CoreName(oLink.FromCP.Owner.Name), polus, new CoreName(oLink.ToCP.Owner.Name), polus);
                            sum += 2;
                            mm++;
                        }
                    }
                }
                mm = 0;
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {
                    shape.showBorder = true;
                }
                drawingPanel1.ShapeCollection.AddShape(shape);
            }

            drawingPanel1.Focus();
        }

        private void Bollobash_Riordan(int N, int k)
        {
            int[,] fGraph = new int[N * k, N * k];
            int[] degree = new int[N * k];
            int j = 0;
            int p;
            int pDegree=0;
            NetworkObject[] shapeArray = new NetworkObject[N + 1];
            Random rand = new Random();
            degree[0] = 2;
            fGraph[0, 0] = 1;
            for(int i=1;i<N*k;i++)
            {
                j = 0;
                p = rand.Next(1,(i+1)*2);
                pDegree = degree[j];
                while(p>pDegree && j<i)
                {
                    j++;
                    pDegree += degree[j];
                }
                fGraph[i, j] = fGraph[j, i] = 1;
                degree[i]++;
                degree[j]++;
            }
            int XC = 550;
            int YC = 350;
            int X0 = 0;
            int Y0 = 0;
            int r = 250;
            double a = Math.PI / N * 2;
            double angle = a;
            for (int i = 0; i < N; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += a;
                ListViewItem li = listView1.Items[4];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 40;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                shape.Name = GetUniqueShapeName(li.Text);
                shapeArray[i] = shape;
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {
                    shape.showBorder = true;
                }
                drawingPanel1.ShapeCollection.AddShape(shape);
            }
            for(int i=0;i<k*N;i++)
                for(j=i;j<k*N;j++)
                {
                    if(fGraph[i,j]==1)
                    {
                        Link oLink = new Link(drawingPanel1, shapeArray[i/k].ConnectionPoint, shapeArray[j/k].ConnectionPoint);
                        if (EditLink(oLink, false))
                            drawingPanel1.ShapeCollection.AddShape(oLink);
                    }
                }
            drawingPanel1.Focus();
        }

        private void Bakly_Oktus(int N, int k, int a)
        {
            int[,] fGraph = new int[N * k, N * k];
            int[] degree = new int[N * k];
            int j = 0;
            int p,hhr;
            int pDegree = 0;
            NetworkObject[] shapeArray = new NetworkObject[N + 1];
            Random rand = new Random();
            degree[0] = 2;
            fGraph[0, 0] = 1;
            for (int i = 1; i < N * k; i++)
            {
                j = 0;
                hhr = (i + 1) * (a + 1) - 1;
                p = rand.Next(1, hhr); 
                pDegree = degree[j]+a-1;
                while (p > pDegree && j < i)
                {
                    j++;
                    pDegree += degree[j]+a-1;
                }
                fGraph[i, j] = fGraph[j, i] = 1;
                degree[i]++;
                degree[j]++;
            }
            int XC = 550;
            int YC = 350;
            int X0 = 0;
            int Y0 = 0;
            int r = 250;
            double an = Math.PI / N * 2;
            double angle = an;
            for (int i = 0; i < N; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += an;
                ListViewItem li = listView1.Items[4];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 40;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                shape.Name = GetUniqueShapeName(li.Text);
                shapeArray[i] = shape;
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {
                    shape.showBorder = true;
                }
                drawingPanel1.ShapeCollection.AddShape(shape);
            }
            for (int i = 0; i < k * N; i++)
                for (j = i; j < k * N; j++)
                {
                    if (fGraph[i, j] == 1)
                    {
                        Link oLink = new Link(drawingPanel1, shapeArray[i / k].ConnectionPoint, shapeArray[j / k].ConnectionPoint);
                        if (EditLink(oLink, false))
                            drawingPanel1.ShapeCollection.AddShape(oLink);
                    }
                }
            drawingPanel1.Focus();
        }

        private void CopyModel(int N,int m,double p)
        {
            double fm;
            List<List<int>> fGraph = new List<List<int>>();
            int[] degree = new int[N];
            int randVert;
            NetworkObject[] shapeArray = new NetworkObject[N];
            Random rand = new Random();
            degree[0] = 2;
            fGraph.Add(new List<int>());
            for(int i=0;i<m;i++)
                fGraph[0].Add(0);
            for(int i=1;i<N;i++)
            {
                fGraph.Add(new List<int>());
                randVert = rand.Next(i);
                for(int j=0;j<m;j++)
                {
                    if (rand.NextDouble()<p)
                    {
                        fGraph[i].Add(rand.Next(i));
                    }
                    else
                    {
                        fGraph[i].Add(fGraph[randVert][j]);
                    }
                }
            }
            int XC = 550;
            int YC = 350;
            int X0 = 0;
            int Y0 = 0;
            double a = Math.PI / N * 2;
            int r = 250;
            double angle = a;
            for (int i = 0; i < N; i++)
            {
                X0 = (int)Math.Round(r * Math.Cos(angle)) + XC;
                Y0 = (int)Math.Round(r * Math.Sin(angle)) + YC;
                angle += a;
                ListViewItem li = listView1.Items[4];
                Point pt = drawingPanel1.PointToClient(new Point(X0, Y0));
                object[] tag = li.Tag as object[];
                ENetworkObjectType type = (ENetworkObjectType)tag[1];

                float fZoom = drawingPanel1.Zoom;
                int delta = 40;
                int X = (int)((pt.X / fZoom - drawingPanel1.dx) - delta / 2);
                int Y = (int)((pt.Y / fZoom - drawingPanel1.dx) - delta / 2);
                NetworkObject shape = new NetworkObject(drawingPanel1);
                shape.Type = type;
                shape.Rect = new Rectangle(X, Y, delta, delta);
                shape.Name = GetUniqueShapeName(li.Text);
                shapeArray[i] = shape;
                if (type != ENetworkObjectType.UserObject)
                    shape.SemanticType = ontologyManager.GetRoutineClass(ontologyManager.GetClass(tag[0] as string)).Name;
                if (ItemImages.ContainsKey(li))
                {
                    shape.img = new Bitmap(ItemImages[li]);
                    shape.showBorder = false;
                    shape.Trasparent = false;
                }
                else
                {
                    shape.showBorder = true;
                }
                drawingPanel1.ShapeCollection.AddShape(shape);
            }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < m; j++)
                {
                    Link oLink = new Link(drawingPanel1, shapeArray[i].ConnectionPoint, shapeArray[fGraph[i][j]].ConnectionPoint);
                    if (EditLink(oLink, false))
                        drawingPanel1.ShapeCollection.AddShape(oLink);
                }
            drawingPanel1.Focus();
        }

        private void tsb_Graphs_Click(object sender, EventArgs e)
        {
            frmParamForGraphs param = new frmParamForGraphs();
            if (param.ShowDialog()== DialogResult.OK)
            {
                switch (param.GraphModel)
                {
                    case 0:
                        Erdos_Renyi(param.Probability, param.CountOfVertex,param.NodeName);
                        break;
                    case 1:
                        Barabashi_Albert(param.CountOfVertex,param.CountStep);
                        break;
                    case 2:
                        Bollobash_Riordan(param.CountOfVertex,param.k);
                        break;
                    case 3:
                        Bakly_Oktus(param.CountOfVertex, param.k, param.a);
                        break;
                    case 4:
                        CopyModel(param.CountOfVertex, param.k, param.Probability);
                        break;
                }
            }
            //param.FormClosed += new FormClosedEventHandler(Models);
            //param.Show();
        }




    }

    //public static class StatClass
    //{
    //    public static int CountOfVertex = 0;
    //    public static double Probability = 0;
    //    public static bool Erd = false;
    //    public static bool Barabash = false;
    //    public static int CountStep = 0;
    //    public static int M = 0;
    //}
}
