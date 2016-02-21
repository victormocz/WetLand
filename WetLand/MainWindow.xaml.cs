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
using System.Windows.Forms;
using System.IO;

namespace WetLand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Global.startDate = new DateTime(1995, 5, 9);
            Global.simulationNumber = "100";
            Global.nofDays = 735;
            Global.DeterminMode = true;
            Global.mainWin = this;
            Global.percentage = 0.01;
            InitializeParameters();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void menuInitial_Click(object sender, RoutedEventArgs e)
        {
            TimeDependentParameters.InitialConcentration initialWindow = new TimeDependentParameters.InitialConcentration();
            initialWindow.ShowDialog();
        }

        private void menuFixed_Click(object sender, RoutedEventArgs e)
        {
            FixedParameterFolder.FixedParameters fpwindow = new FixedParameterFolder.FixedParameters();
            fpwindow.ShowDialog();
        }

        private void menuHydroViewData_Click(object sender, RoutedEventArgs e)
        {
            HydrolicParameters.HydrolicParameters parameterWindow = new HydrolicParameters.HydrolicParameters();
            parameterWindow.ShowDialog();
            menuFixed.IsEnabled = true;
            
        }

        private void menuConcentrationViewData_Click(object sender, RoutedEventArgs e)
        {
            TimeDependentParameters.TimeDependentParameters timeDataWin = new TimeDependentParameters.TimeDependentParameters();
            timeDataWin.ShowDialog();
        }

        private void menuAnalysis_Click(object sender, RoutedEventArgs e)
        {
            Analysis.AnalysisReport analysisReport = new Analysis.AnalysisReport();
            analysisReport.ShowDialog();
        }

        private void Open_Project_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            string path = folderDlg.SelectedPath;
            if (path.Length == 0) {
                return;
            }
            bool result = validatePath(path);
            if (result)
            {
                Global.projectName = path;
                Global.nofDays = getNumberOfDays(Global.projectName + @"\InputFiles\12_hydro_climate.txt");
                Hydro_Climate.IsEnabled = true;
                save_as.IsEnabled = true;
                menuFixed.IsEnabled = true;
                Water_quality.IsEnabled = true;
                Input_Files.IsEnabled = true;
                Model_Parameters.IsEnabled = true;
                menu_run.IsEnabled = true;

                string[] fileName = Directory.GetFiles(Global.projectName + "\\InputFiles\\");
                Regex reg = new Regex(@"([0-9]{3}[_][a-zA-Z0-9]+)\.(txt)$");
                int outputFileCount = 0;

                for (int i = 0; i < fileName.Length; i++)
                {
                    if (reg.IsMatch(fileName[i]))
                    {
                        outputFileCount++;
                    }
                }
               
                if (outputFileCount == 29)
                {
                    //string[] contents = File.ReadAllLines(Global.projectName + @"\InputFiles\102_Onw.txt");
                    int lineNum = 0;
                    using (var reader = File.OpenText(Global.projectName + @"\InputFiles\102_Onw.txt"))
                    {
                        while (reader.ReadLine() != null)
                        {
                            lineNum++;
                        }
                    }
                    if (lineNum == 4)
                    {
                        Global.DeterminMode = true;
                        KSTest.IsEnabled = false;
                    }
                    else
                    {
                        Global.DeterminMode = false;
                        KSTest.IsEnabled = true;
                    }
                    Global.mainWin.menuAnalysis.IsEnabled = true;
                    Global.mainWin.PostProcessing.IsEnabled = true;
                }
            }
        }

        private int getNumberOfDays(String fileName)
        {
            string[] contents = File.ReadAllLines(fileName);
            if (contents.Length > 2)
            {
                return contents.Length - 2;
            }
            else
            {
                return 0;
            }
        }

        private bool validatePath(String projectPath)
        {
            bool folderResult;
            bool fileResult;
            folderResult = Directory.Exists(projectPath + @"\InputFiles\Routing") && Directory.Exists(projectPath + @"\InputFiles\System");
            fileResult = File.Exists(projectPath + @"\InputFiles\1_input_control.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\10_fixed_parameters.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\11_initial_concentration.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\12_hydro_climate.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\13_time_dependent_parameters.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\14_generated_parameters.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\15_generated_parameters_carbon.txt");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\Wet.bat");
            fileResult = fileResult && File.Exists(projectPath + @"\InputFiles\Wetlandmodel.exe");
            if (!(folderResult && fileResult))
            {
                System.Windows.MessageBox.Show("It is not a correct Project Folder or some important files were missing!", "Caution");
                return false;
            }
            return true;
        }

        private void run_simulations_Click(object sender, RoutedEventArgs e)
        {
            
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Global.projectName + @"\InputFiles\Wetlandmodel.exe";
            info.UseShellExecute = true;
            info.WorkingDirectory = Global.projectName + @"\InputFiles";
            var process = Process.Start(info);
            process.WaitForExit();
            
            string[] contents = File.ReadAllLines(Global.projectName + @"\InputFiles\102_Onw.txt");

            if (contents.Length == 4)
            {
                Global.DeterminMode = true;
                KSTest.IsEnabled = false;
            }
            else
            {
                Global.DeterminMode = false;
                if (Global.tempCarbon != null && Global.tempNitrogen == null)
                {
                    Global.Nitrogen = new List<ModelParameters.Parameters>(Global.tempNitrogen);
                    Global.Carbon = new List<ModelParameters.Parameters>(Global.tempCarbon);
                }
                KSTest.IsEnabled = true;
            }
            
            menuAnalysis.IsEnabled = true;
            PostProcessing.IsEnabled = true;
        }

        private void deterministicModel_Click(object sender, RoutedEventArgs e)
        {
            ModelParameters.Deterministic determinWin = new ModelParameters.Deterministic();
            determinWin.ShowDialog();
        }

        private void Nitrogen_Click(object sender, RoutedEventArgs e)
        {
            ModelParameters.Nitrogen nitrogenWin = new ModelParameters.Nitrogen();
            nitrogenWin.ShowDialog();
        }

        private void PostProcessing_Click(object sender, RoutedEventArgs e)
        {
            PostProcess.PostProcess postprocesswin = new PostProcess.PostProcess();
            postprocesswin.ShowDialog();
        }

        private void menuFlowRouting_Click(object sender, RoutedEventArgs e)
        {
            Routing.RoutingMainWindow routingWin = new Routing.RoutingMainWindow();
            routingWin.ShowDialog();
        }

        private void menuStartDate_Click(object sender, RoutedEventArgs e)
        {
            Date.Date dateWin = new Date.Date();
            dateWin.ShowDialog();
        }

        private void KSTest_Click(object sender, RoutedEventArgs e)
        {
            KSTest.KSTest ksWin = new KSTest.KSTest();
            ksWin.ShowDialog();
        }

        private void New_Project_Click(object sender, RoutedEventArgs e)
        {
            try {
                CreateNewProject(true);
                
                
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error");
            }
        }



        private void CreateNewProject(bool flag)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowDialog();
            string path = folderDlg.SelectedPath;
            if (path.Length == 0)
            {
                return;
            }
            if (path == AppDomain.CurrentDomain.BaseDirectory)
            {
                System.Windows.MessageBox.Show("The path can't be default folder");
                return;
            }
            if ((Directory.Exists(path + "\\InputFiles") || Directory.Exists(path + "\\Observations")))
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("There is an existing project here. Do you want to overwrite it?", "Project overwrite", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result.ToString().Equals("No"))
                {
                    return;
                }
            }
            if (Directory.Exists(path + @"\InputFiles"))
            {
                Directory.Delete(path + @"\InputFiles", true);
            }
            if (Directory.Exists(path + @"\Observations"))
            {
                Directory.Delete(path + @"\Observations", true);
            }
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryCopy(currentDir + @"\InputFiles", path + @"\InputFiles", true);
            DirectoryCopy(currentDir + @"\Observations", path + @"\Observations", true);
            Global.projectName = path;
            if (flag)
            {
                save_as.IsEnabled = true;
                Input_Files.IsEnabled = true;
                Hydro_Climate.IsEnabled = false;
                menuFixed.IsEnabled = false;
                Water_quality.IsEnabled = false;
                Model_Parameters.IsEnabled = false;
                menu_run.IsEnabled = false;
                menuAnalysis.IsEnabled = false;
                PostProcessing.IsEnabled = false;
                KSTest.IsEnabled = false;
            }

        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void Carbon_Click(object sender, RoutedEventArgs e)
        {
            ModelParameters.Carbon carbonwin = new ModelParameters.Carbon();
            carbonwin.ShowDialog();
        }

        private void InitializeParameters() {
            Global.Nitrogen = new List<ModelParameters.Parameters>();
            Global.Carbon = new List<ModelParameters.Parameters>();
            //15mon.bat
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\14mon.bat"))
            {
                string[] rows = File.ReadAllLines(Global.projectName + "\\InputFiles\\System\\14mon.bat");
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    string[] parameters = rows[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    int type = Convert.ToInt16(parameters[0]);
                    double max = Convert.ToDouble(parameters[1]);
                    double min = Convert.ToDouble(parameters[2]);
                    double c = Convert.ToDouble(parameters[3]);
                    Global.Nitrogen.Add(new ModelParameters.Parameters(type, min, max, c));
                }
            }
            else {
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 5, 50, 5));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 1.15, 1.35, 1.15));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 100, 400, 100));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0.29, 0.38, 0.29));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 0.032, 80, 0.032));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0.15, 0.45, 0.15));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 0.0009, 0.002, 0.0009));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.0009,0.002, 0.0009));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.000001 ,0.0031 ,0.000001 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.0001 ,0.35 ,0.0001 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.000001 ,0.001 ,0.000001 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.01 ,42 ,0.01 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.004,0.15,0.004 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,1.5 ,2.2 ,1.5 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.025 ,138 ,0.025 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 8, 6750, 8));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.000274 ,0.006575 ,0.000274 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,3.5 ,17.6 ,3.5 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,20 ,100 ,20 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.022 ,0.065 ,0.022 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0 ,0 ,0 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.0864, 0.3456, 0.0864));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.5 ,1 ,0.5 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.04 ,0.14 ,0.04 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,1228, 3686, 1228));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,4.5 ,8.2 ,4.5 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.0004 ,3.5 ,0.0004 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,1024 ,1193731 ,1024 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0,0.4 ,2 ,0.4 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0.66, 0.83, 0.66));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 1024, 1193731, 1024));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 8780, 18549874, 8780));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0, 1, 0));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0.5, 1, 0.5));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 9.3, 2021, 9.3));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1,0.00001, 61, 0.00001));
                Global.Nitrogen.Add(new ModelParameters.Parameters(1, 0.00001,0.46121 ,0.00001 ));
                Global.Nitrogen.Add(new ModelParameters.Parameters(0, 0.65, 0.95, 0.65));
            }

            if (File.Exists(Global.projectName + "\\InputFiles\\System\\15mon.bat"))
            {
                string[] rows = File.ReadAllLines(Global.projectName + "\\InputFiles\\System\\15mon.bat");
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    string[] parameters = rows[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    int type = Convert.ToInt16(parameters[0]);
                    double max = Convert.ToDouble(parameters[1]);
                    double min = Convert.ToDouble(parameters[2]);
                    double c = Convert.ToDouble(parameters[3]);
                    Global.Carbon.Add(new ModelParameters.Parameters(type, min, max, c));
                }
            }
            else {
                Global.Carbon.Add(new ModelParameters.Parameters(0,15,160,15));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.33,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.99,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.99,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.33,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.99,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.04,0.99,0.04));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.000001, 0.003, 0.000001));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.0000001, 0.0003, 0.0000001));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.2,1,0.2));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.0000001,1, 0.0000001));
                Global.Carbon.Add(new ModelParameters.Parameters(1,0.004,0.36,0.004));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.0000001, 0.36, 0.0000001));
                Global.Carbon.Add(new ModelParameters.Parameters(0, 0.0015,0.4, 0.0015));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.001,0.16,0.001));
                Global.Carbon.Add(new ModelParameters.Parameters(0, 0.0005,0.08, 0.0005));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.0000001,0.08, 0.0000001));
                Global.Carbon.Add(new ModelParameters.Parameters(1,0.00002,1,0.00002));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.000005,1, 0.000005));
                Global.Carbon.Add(new ModelParameters.Parameters(0, 0.00000015,0.5, 0.00000015));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.4,0.7,0.4));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.25,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(0,0.01,0.08,0.01));
                Global.Carbon.Add(new ModelParameters.Parameters(1, 0.0000006, 0.077, 0.0000006));
            }
        }

        private void save_as_Click(object sender, RoutedEventArgs e)
        {
            CreateNewProject(false);
        }
    }
}
