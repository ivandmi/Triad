using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.View
{
    /// <summary>
    /// Логика взаимодействия для ProceduresControl.xaml
    /// </summary>
    public partial class ProceduresControl : UserControl, IProcedureView
    {
        public ProceduresControl()
        {
            InitializeComponent();

            cmbProcType.SelectionChanged += CmbProcType_SelectionChanged;
            lb.DisplayMemberPath = "Name";

            lb.SelectionChanged += Lb_SelectionChanged;
        }

        // Можно здесь прибраться и перенести все в xaml и забиндить
        // говно код, надо исправить
        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb.SelectedItem is ProcedureBlueprint blueprint)
            {
                txtProcName.Text = blueprint.Name;
                cmbProcType.SelectedItem = blueprint.Metadata;
                procParamsPanel.Children.Clear();
                if (blueprint.ModelParamByProcParam.Count == 0)
                {
                    foreach (var param in blueprint.Metadata.Params)
                    {
                        var paramControl = new ParamBox();
                        paramControl.ParamName = param.DisplayName;
                        paramControl.ToolTip = param.Description;
                        procParamsPanel.Children.Add(paramControl);
                    }
                }
                else
                {
                    foreach(var param in blueprint.ModelParamByProcParam)
                    {
                        var paramControl = new ParamBox();
                        paramControl.ParamName = param.Key.DisplayName;
                        paramControl.ToolTip = param.Key.Description;
                        paramControl.ParamValue = param.Value;
                        procParamsPanel.Children.Add(paramControl);
                    }
                }
            }
        }

        private void CmbProcType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbProcType.SelectedItem is IProcedureMetadata proc)
            {
                procParamsPanel.Children.Clear();
                foreach(var param in proc.Params)
                {
                    var paramControl = new ParamBox();
                    paramControl.ParamName = param.DisplayName;
                    paramControl.ToolTip = param.Description;
                    procParamsPanel.Children.Add(paramControl);
                }
            }
        }

        public event EventHandler<ProcedureEventArgs> RemoveProcedure;
        public event EventHandler<ProcedureEventArgs> CreateProcedureBlueprint;
        public event EventHandler<ProcedureEventArgs> SaveProcedure;

        void OnAddProcedureBlueprint()
        {
            CreateProcedureBlueprint?.Invoke(this, new ProcedureEventArgs());
        }

        void OnSaveProcedure(ProcedureEventArgs e)
        {
            SaveProcedure?.Invoke(this, e);
        }

        public void SetProceduresTypes(IEnumerable<IProcedureMetadata> procedures)
        {
            cmbProcType.ItemsSource = procedures;
            cmbProcType.DisplayMemberPath = "Name";
        }

        private void Add_ProcedureBlueprint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnAddProcedureBlueprint();
        }

        public void SetProcedureBluePrints(IEnumerable<ProcedureBlueprint> blueprints)
        {
            throw new NotImplementedException();
        }

        public void AddProcedureBlueprint(ProcedureBlueprint blueprint)
        {
            lb.Items.Insert(0, blueprint);
            lb.SelectedIndex = 0;
        }


        // TODO: исправить это, слишком много if-ов
        private void btnProcSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(lb.SelectedItem is ProcedureBlueprint blueprint)
            {
                blueprint.Name = txtProcName.Text;
                Dictionary<IPParamMetadata, string> dict = new Dictionary<IPParamMetadata, string>();
                var paramsData = cmbProcType.SelectedItem as IProcedureMetadata;
                foreach(var paramBox in procParamsPanel.Children)
                {
                    if (paramBox is ParamBox box)
                    {
                        var data = paramsData.Params.FirstOrDefault(x => x.DisplayName == box.ParamName);
                        dict.Add(data, box.ParamValue);
                    }
                    
                }
                blueprint.ModelParamByProcParam = dict;
                blueprint.Metadata = cmbProcType.SelectedItem as IProcedureMetadata;
                OnSaveProcedure(new ProcedureEventArgs(blueprint));
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
