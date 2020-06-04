using GraphX.Controls;
using GraphX.Controls.Models;
using QuickGraph;
using ShowcaseApp.WPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TriadWpf.ViewModels;

namespace TriadWpf.View.GraphXModels
{
    public class GraphAreaControl : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    { 
        public GraphAreaControl() : base()
        {

        }
    }
}
