using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.FixedParameterFolder
{
    using System.IO;
    using System.Windows;
    class FixedData
    {
        public string dt;
        public string ronn;
        public string rond;
        public string roc;
        public string sims;
        public string fnw;
        public string fns1;
        public string fns2;
        public string fno3w;
        public string fno3s1;
        public string fno3s2;
        public string w;
        public string apn;
        public string lat;
        public string d_bound;
        public string amc;
        public string lamdar;
        public FixedParameters fixedwin;
        public static string filePath = Global.projectName + @"\InputFiles\10_fixed_parameters.txt";
        public FixedData(FixedParameters fixedwin) {
            this.fixedwin = fixedwin;
            readData(filePath);
            putValue();
            saveFile();
            fixedwin.status.Text = "";
        }
        public FixedData()
        {
            readData(filePath);
        }

        public void readData(string filePath) {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] content = File.ReadAllLines(filePath);
                    if (content.Length < 7) {
                        defaultValue();
                        return;
                    }
                    string[] parameters = content[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 6)
                    {
                        defaultValue();
                        return;
                    }
                    this.dt = parameters[0];
                    this.ronn = parameters[2];
                    this.rond = parameters[3];
                    this.roc = parameters[4];
                    this.sims = parameters[5];
                    parameters = content[3].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 10)
                    {
                        defaultValue();
                        return;
                    }
                    this.fnw = parameters[0];
                    this.fns1 = parameters[1];
                    this.fns2 = parameters[2];
                    this.fno3w = parameters[3];
                    this.fno3s1 = parameters[4];
                    this.fno3s2 = parameters[5];
                    this.w = parameters[6];
                    this.apn = parameters[7];
                    this.lat = parameters[8];
                    this.d_bound = parameters[9];

                    parameters = content[5].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 2)
                    {
                        defaultValue();
                        return;
                    }
                    this.amc = parameters[0];
                    this.lamdar = parameters[1];

                }
                catch (Exception)
                {
                }
            }
            else {
                defaultValue();
            }
        }

        public void defaultValue() {
            this.dt = "0.1";
            this.ronn = "4.57";
            this.rond = "15.29";
            this.roc = "2.67";
            this.sims = "100";
            this.fnw = "0.5";
            this.fns1 = "0.5";
            this.fns2 = "0.5";
            this.fno3w = "0.5";
            this.fno3s1 = "0.5";
            this.fno3s2 = "0.5";
            this.w = "0.2618";
            this.apn = "0.139";
            this.lat = "0.68";
            this.d_bound = "0";
            this.amc = "0.267";
            this.lamdar = "0.0003";
        }

        public void putValue() {
            fixedwin.dt.Text = this.dt;
            fixedwin.ronn.Text = this.ronn;
            fixedwin.rond.Text = this.rond;
            fixedwin.roc.Text = this.roc;
            fixedwin.sims.Text = this.sims;
            fixedwin.fnw.Text = this.fnw;
            fixedwin.fns1.Text = this.fns1;
            fixedwin.fns2.Text = this.fns2;
            fixedwin.fno3w.Text = this.fno3w;
            fixedwin.fno3s1.Text = this.fno3s1;
            fixedwin.fno3s2.Text = this.fno3s2;
            fixedwin.apn.Text = this.apn;
            fixedwin.lat.Text = this.lat;
            fixedwin.d_bound.Text = this.d_bound;
            fixedwin.amc.Text = this.amc;
            fixedwin.lamdar.Text = this.lamdar;
        }

        public void getValue() {
            this.dt = fixedwin.dt.Text;
            this.ronn = fixedwin.ronn.Text;
            this.rond = fixedwin.rond.Text;
            this.roc = fixedwin.roc.Text;
            this.sims = fixedwin.sims.Text;
            this.fnw = fixedwin.fnw.Text;
            this.fns1 = fixedwin.fns1.Text;
            this.fns2 = fixedwin.fns2.Text;
            this.fno3w = fixedwin.fno3w.Text;
            this.fno3s1 = fixedwin.fno3s1.Text;
            this.fno3s2 = fixedwin.fno3s2.Text;
            this.apn = fixedwin.apn.Text;
            this.lat = fixedwin.lat.Text;
            this.d_bound = fixedwin.d_bound.Text;
            this.amc = fixedwin.amc.Text;
            this.lamdar = fixedwin.lamdar.Text;
        }
        public void saveFile() {
            if (fixedwin != null)
            {
                getValue();
            }
            string[] content = new string[7 + Global.nofDays];
            content[0] = "dt	n	ronn(gO/gN)	rond(go/gN)	roc	sims";
            content[1] = this.dt + "\t" + (Global.nofDays-1).ToString() + "\t" + this.ronn + "\t" + this.rond + "\t" + this.roc + "\t" + this.sims;
            content[2] = "fNw	fNs1	fNs2	fNO3w	fNO3s1	fNO3s2	w	apn	lat	d_bound";
            content[3] = this.fnw + "\t" + this.fns1 + "\t" + this.fns2 + "\t" + this.fno3w + "\t" + this.fno3s1 + "\t" + this.fno3s2 + "\t" + this.w + "\t" + this.apn + "\t" + this.lat + "\t" + this.d_bound;
            content[4] = "amc(gr/gr)	lamdaR(m root m-3 soil)";
            content[5] = this.amc + "\t" + this.lamdar;
            content[6] = "dn(day)";
            DateTime countDate = Global.startDate.AddDays(-1);
            for(int i = 7; i < 7 + Global.nofDays; i++)
            {
                countDate = countDate.AddDays(1);
                content[i] = countDate.DayOfYear.ToString();
            }
            try
            {
                File.WriteAllLines(filePath, content);

            }
            catch (Exception exc)
            {
                fixedwin.status.Text = exc.Message;
            }
            if (fixedwin != null)
            {
                fixedwin.status.Visibility = Visibility.Visible;
                fixedwin.status.Text = "Data saved successfully!";
            }
        }
    }
}
