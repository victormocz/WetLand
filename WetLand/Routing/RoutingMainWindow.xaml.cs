﻿using System;
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
using System.Windows.Shapes;

namespace WetLand.Routing
{
    /// <summary>
    /// Interaction logic for RoutingMainWindow.xaml
    /// </summary>
    using System.IO;
    using System.Diagnostics;
    public partial class RoutingMainWindow : Window
    {
        private RoutingData data;
        public RoutingMainWindow()
        {
            InitializeComponent();
            try
            {
                data = new RoutingData(this);
                InitializeInputText();
                //addDataSourceToInput();
                //addDataSourceToGeo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error!");
            }
        }

        private void InitializeInputText()
        {

            data.readData(RoutingData.filePath);
            hinitial.Text = data.hinitial.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            data.getValue();
            data.saveFile();
        }

        private void addDataSourceToInput()
        {
            List<InputItem> items = new List<InputItem>();
            string filename = Global.projectName + @"\InputFiles\Routing\2_input_time_series.txt";
            string[] contents = File.ReadAllLines(filename);
            
            if (contents.Length < 2)
            {
                throw new Exception(filename + " length is too short");
            }
            if (contents[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length == 4) {
                MessageBox.Show("Calculate ET","Info");
            }
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Global.projectName + @"\InputFiles\Routing\ET Module";
            info.FileName = Global.projectName + @"\InputFiles\Routing\ET Module\ET.exe";
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(info);
            process.WaitForExit();
            string[] etcontents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\ET Module\ET.txt");
            data.n = contents.Length - 1;
            if (etcontents.Length != contents.Length) {
                throw new Exception("Length of days do not equal.(ET Module)");
            }
            DateTime date = Global.startDate.AddDays(-1);
            for (int i = 1; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length == 5)
                {
                    date = date.AddDays(1);
                    items.Add(new InputItem { date = date, qin = parameters[1], et = parameters[2], ip = parameters[3], qg = parameters[4] });
                }
                else if (parameters.Length == 4)
                {
                    date = date.AddDays(1);
                    items.Add(new InputItem { date = date, qin = parameters[1], et = etcontents[i],ip = parameters[2], qg = parameters[3] });
                }
                else
                {
                    throw new Exception("We need to have 4 or 5 parameters in line "+i.ToString());
                }
            }
            timeSeriesData.ItemsSource = items;
            data.saveFile();
        }

        private void addDataSourceToGeo()
        {
            List<GeoItem> items = new List<GeoItem>();
            string filename = Global.projectName + @"\InputFiles\Routing\3_geometry_inputs.txt";
            string[] contents = File.ReadAllLines(filename);
            if (contents.Length < 2)
            {
                throw new Exception(filename + " length is too short");
            }

            data.rows = contents.Length - 1;

            for (int i = 1; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length == 4)
                {
                    items.Add(new GeoItem { h = parameters[0], area = parameters[1], volume = parameters[2], outflow = parameters[3] });
                }
                else
                {
                    throw new Exception("We need to have 4 parameters in line " + i.ToString());
                }
            }
            geometryData.ItemsSource = items;
            data.saveFile();
        }

        private void addDataSourceToOutput()
        {
            List<OutputItem> items = new List<OutputItem>();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Global.projectName + @"\InputFiles\Routing";
            info.FileName = Global.projectName + @"\InputFiles\Routing\FTABLE.exe";
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            var process = Process.Start(info);
            process.WaitForExit();
            if(!File.Exists(Global.projectName + @"\InputFiles\Routing\5_output.txt"))
            {
                throw new Exception("Cannot find the output file " + Global.projectName + @"\InputFiles\Routing\5_output.txt");
            }
            string[] contents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\5_output.txt");
            if (contents.Length < 2)
            {
                throw new Exception(Global.projectName + @"\InputFiles\Routing\5_output.txt is too short!");
            }
            DateTime date = Global.startDate;
            for (int i = 1; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length == 5)
                {
                    items.Add(new OutputItem { date=date,h=parameters[1],outflow=parameters[2],volume=parameters[3],area=parameters[4]});
                }
                else
                {
                    throw new Exception("We need to have 5 parameters in line " + i.ToString());
                }
                
            }
            outputData.ItemsSource = items;
        }

        private void resizeCol()
        {
            double winsize = this.Width / 5.45;
            col0.Width = winsize;
            col1.Width = winsize;
            col2.Width = winsize;
            col3.Width = winsize;
            col4.Width = winsize;
            ocol0.Width = winsize;
            ocol1.Width = winsize;
            ocol2.Width = winsize;
            ocol3.Width = winsize;
            ocol4.Width = winsize;
            winsize = this.Width / 4.2;
            gcol0.Width = winsize;
            gcol1.Width = winsize;
            gcol2.Width = winsize;
            gcol3.Width = winsize;
            //TODO add left part
        }

        private void RouthingWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeCol();
        }

        private void viewInputGraph_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            InputReport inputreportWin = new InputReport();
            inputreportWin.ShowDialog();
        }

        private void viewGeoGraph_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GeometryReport georeportWin = new GeometryReport();
            georeportWin.ShowDialog();
        }

        private void tabview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                if (inputItem.IsSelected)
                {
                    InitializeInputText();
                } else if (timeItem.IsSelected)
                {
                    addDataSourceToInput();
                } else if (geometryItem.IsSelected)
                {
                    addDataSourceToGeo();
                }else if (outputItem.IsSelected)
                {
                    addDataSourceToOutput();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void viewOutputGraph_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OutputReport outputreportWin = new OutputReport();
            outputreportWin.ShowDialog();
        }

        private void ETdata_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ETWindow et = new ETWindow();
            et.ShowDialog();
        }

        private void viewInputGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space) {
                InputReport inputreportWin = new InputReport();
                inputreportWin.ShowDialog();
            }
        }

        private void ETdata_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                ETWindow et = new ETWindow();
                et.ShowDialog();
            }
        }

        private void viewGeoGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                GeometryReport georeportWin = new GeometryReport();
                georeportWin.ShowDialog();
            }
        }

        private void viewOutputGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                OutputReport outputreportWin = new OutputReport();
                outputreportWin.ShowDialog();
            }
        }
    }
}
