using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TriadCore;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.GraphEventArgs;
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
        public event EventHandler<PolusEventArgs> AddPolus;

        void OnChangeName(ChangeNameArgs e)
        {
            ChangeName?.Invoke(this, e);
        }

        void OnUpdateRoutineParam(UpdateParamValueArgs e)
        {
            UpdateRoutineParam?.Invoke(this, e);
        }

        void OnAddPolus(PolusEventArgs e)
        {
            AddPolus?.Invoke(this, e);
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

        public void BadName(string errorText)
        {
            Popup popup = new Popup();
            TextBlock text = new TextBlock();
            text.Background = new SolidColorBrush(Colors.HotPink);
            text.Text = errorText;
            popup.Child = text;
            popup.PlacementTarget = txtVertexName;
            popup.IsOpen = true;
            popup.StaysOpen = false;
        }

        private void btnAddPolus_Click(object sender, RoutedEventArgs e)
        {
            if (txtPolusName.Text != "")
            {
                OnAddPolus(new PolusEventArgs(txtPolusName.Text, txtVertexName.Text));
            }
        }

        public void ShowPoluses(params CoreName[] poluses)
        {
            lbPoluses.Items.Clear();
            foreach(var polus in poluses)
            {
                lbPoluses.Items.Add(polus);
            }
        }
    }
}
