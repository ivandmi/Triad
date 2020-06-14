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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.View
{
    /// <summary>
    /// Логика взаимодействия для VertexPropertiesView.xaml
    /// </summary>
    public partial class VertexPropertiesView : UserControl, IVertexPropertiesView
    {
        public VertexPropertiesView()
        {
            InitializeComponent();
        }

        public event EventHandler<ChangeNameArgs> ChangeName;
        public event EventHandler<UpdateParamValueArgs> UpdateRoutineParam;

        void OnChangeName(ChangeNameArgs e)
        {
            ChangeName?.Invoke(this, e);
        }

        void OnUpdateRoutineParam(UpdateParamValueArgs e)
        {
            UpdateRoutineParam?.Invoke(this, e);
        }

        public void SetVertexName(string name)
        {
            txtVertexName.Text = name;
        }

        public void ShowParamsAndVariables(List<RoutineParameter> parameters, List<RoutineParamMetaData> vars)
        {
            paramPanel.Children.Clear();
            txtVars.Text = "";

            foreach(var param in parameters)
            {
                ParamBox box = new ParamBox();
                box.ParamName = param.MetaData.Name;
                box.ParamValue = param.Value.ToString();
                box.Tag = param.MetaData;
                paramPanel.Children.Add(box);
            }

            foreach(var variable in vars)
            {
                txtVars.Text += variable.Type.Name + " " + variable.Name + "\n";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // сохранение имени вершины
            // TODO: переделать ChangeNameArgs. Думаю, форма не должна запоминать старое имя
            OnChangeName(new ChangeNameArgs(null, txtVertexName.Text));

            // сохранение переменных
            foreach(var paramCntrl in paramPanel.Children)
            {
                ParamBox box = paramCntrl as ParamBox;
                OnUpdateRoutineParam(new UpdateParamValueArgs((RoutineParamMetaData)box.Tag, box.ParamValue));
            }
        }
    }
}
