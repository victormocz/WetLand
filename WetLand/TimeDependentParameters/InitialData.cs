using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.TimeDependentParameters
{
    using System.IO;
    using System.Windows;
    class InitialData
    {
        public string oNw;
        public string oNsf;
        public string ONss;
        public string Nw;
        public string Ns1;
        public string Ns2;
        public string N03w;
        public string N03s1;
        public string N03s2;
        public string a;
        public string b;
        public string mw;
        public string ms;
        public string ow;
        public string Pw;
        public string Ps1;
        public string Ps2;
        public string DOCw;
        public string LPOCw;
        public string RPOCw;
        public string DOCs1;
        public string LPOCs1;
        public string RPOCs1;
        public string DOCs2;
        public string LPOCs2;
        public string RPOCs2;
        public string Ch4w;
        public string CH4s1;
        public string CH4s2;
        public InitialConcentration initialwin;
        public static string filePath = Global.projectName + @"\InputFiles\11_initial_concentration.txt";
        public InitialData(InitialConcentration initialwin)
        {
            this.initialwin = initialwin;
            readData(filePath);
            putValue();
        }

        public void readData(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] content = File.ReadAllLines(filePath);
                    if (content.Length != 12)
                    {
                        defaultValue();
                        return;
                    }
                    string[] parameters = content[2].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 9)
                    {
                        defaultValue();
                        return;
                    }
                    this.oNw = parameters[0];
                    this.oNsf = parameters[1];
                    this.ONss = parameters[2];
                    this.Nw = parameters[3];
                    this.Ns1 = parameters[4];
                    this.Ns2 = parameters[5];
                    this.N03w = parameters[6];
                    this.N03s1 = parameters[7];
                    this.N03s2 = parameters[8];

                    parameters = content[4].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 2)
                    {
                        defaultValue();
                        return;
                    }
                    this.a = parameters[0];
                    this.b = parameters[1];

                    parameters = content[6].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 2)
                    {
                        defaultValue();
                        return;
                    }
                    this.mw = parameters[0];
                    this.ms = parameters[1];

                    parameters = content[8].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 4)
                    {
                        defaultValue();
                        return;
                    }
                    this.ow = parameters[0];
                    this.Pw = parameters[1];
                    this.Ps1 = parameters[2];
                    this.Ps2 = parameters[3];

                    parameters = content[11].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length != 12)
                    {
                        defaultValue();
                        return;
                    }
                    this.DOCw = parameters[0];
                    this.LPOCw = parameters[1];
                    this.RPOCw = parameters[2];
                    this.DOCs1 = parameters[3];
                    this.LPOCs1 = parameters[4];
                    this.RPOCs1 = parameters[5];
                    this.DOCs2 = parameters[6];
                    this.LPOCs2 = parameters[7];
                    this.RPOCs2 = parameters[8];
                    this.Ch4w = parameters[9];
                    this.CH4s1 = parameters[10];
                    this.CH4s2 = parameters[11];

                }
                catch (Exception)
                {
                    defaultValue();
                }
            }
            else
            {
                defaultValue();
            }
        }
        public void defaultValue()
        {
            this.oNw = " 2.509";
            this.oNsf = " 0.912";
            this.ONss = " 0.144";
            this.Nw = " 0.551";
            this.Ns1 = " 0.551";
            this.Ns2 = " 0.551";
            this.N03w = " 0.055";
            this.N03s1 = " 0.055";
            this.N03s2 = " 0.005";
            this.a = " 1";
            this.b = " 2000";
            this.mw = " 110.5";
            this.ms = " 0.3";
            this.ow = " 5.967";
            this.Pw = " 0.621";
            this.Ps1 = " 0.621";
            this.Ps2 = " 0.621";
            this.DOCw = " 4.00";
            this.LPOCw = " 9.50";
            this.RPOCw = " 13.10";
            this.DOCs1 = " 4.00";
            this.LPOCs1 = " 9.50";
            this.RPOCs1 = " 13.10";
            this.DOCs2 = " 0.68";
            this.LPOCs2 = " 1.00";
            this.RPOCs2 = " 1.41";
            this.Ch4w = " 2";
            this.CH4s1 = " 2";
            this.CH4s2 = " 2";
            putValue();
        }
        public void saveFile()
        {
            getValue();
            string[] content = new string[12];
            content[0] = "---- (mg/L) ----";
            content[1] = "ONw	ONsf    ONss    Nw      Ns1     Ns2    N03w    		N03s1   	N03s2";
            content[2] = this.oNw + "\t" + this.oNsf + "\t" + this.ONss + "\t" + this.Nw + "\t" + this.Ns1 + "\t" + this.Ns2 + "\t" + this.N03w + "\t" + this.N03s1 + "\t" + this.N03s2;
            content[3] = "a(0)(g)   b(0)(g)";
            content[4] = this.a + "\t" + this.b;
            content[5] = "mw(0)(mg/L)	ms(0) ";
            content[6] = this.mw + "\t" + this.ms;
            content[7] = "Ow(0)(mg/l)    	Pw(0)   Ps1(0)    Ps2(0)";
            content[8] = this.ow + "\t" + this.Pw + "\t" + this.Ps1 + "\t" + this.Ps2;
            content[9] = "----	(mg/L)	----";
            content[10] = "DOCw	LPOCw	RPOCw	DOCs1	LPOCs1	RPOCs1	DOCs2		LPOCs2	RPOCs2	Ch4w		CH4s1		CH4s2";
            content[11] = this.DOCw + "\t" + this.LPOCw + "\t" + this.RPOCw + "\t" + this.DOCs1 + "\t" + this.LPOCs1 + "\t" + this.RPOCs1 + "\t" + DOCs2 + "\t" + this.LPOCs2 + "\t" + this.RPOCs2 + "\t" + this.Ch4w + "\t" + this.CH4s1 + "\t" + this.CH4s2;
            try
            {
                File.WriteAllLines(filePath, content);

            }
            catch (Exception exc)
            {
                initialwin.status.Text = exc.Message;
            }
            initialwin.status.Visibility = Visibility.Visible;
            initialwin.status.Text = "Data saved successfully!";
        }

        public void putValue()
        {
            initialwin.onw.Text = this.oNw;
            initialwin.onsf.Text = this.oNsf;
            initialwin.onss.Text = this.ONss;
            initialwin.nw.Text = this.Nw;
            initialwin.ns1.Text = this.Ns1;
            initialwin.ns2.Text = this.Ns2;
            initialwin.n03w.Text = this.N03w;
            initialwin.n03s1.Text = this.N03s1;
            initialwin.n03s2.Text = this.N03s2;
            initialwin.a.Text = this.a;
            initialwin.b.Text = this.b;
            initialwin.mw.Text = this.mw;
            initialwin.ms.Text = this.ms;
            initialwin.ow.Text = this.ow;
            initialwin.pw.Text = this.Pw;
            initialwin.ps1.Text = this.Ps1;
            initialwin.ps2.Text = this.Ps2;
            initialwin.docw.Text = this.DOCw;
            initialwin.lpocw.Text = this.LPOCw;
            initialwin.rpocw.Text = this.RPOCw;
            initialwin.docs1.Text = this.DOCs1;
            initialwin.lpocs1.Text = this.LPOCs1;
            initialwin.rpocs1.Text = this.RPOCs1;
            initialwin.docs2.Text = this.DOCs2;
            initialwin.lpocs2.Text = this.LPOCs2;
            initialwin.rpocs2.Text = this.RPOCs2;
            initialwin.ch4w.Text = this.Ch4w;
            initialwin.ch4s1.Text = this.CH4s1;
            initialwin.ch4s2.Text = this.CH4s2;
        }
        public void getValue()
        {
            this.oNw = initialwin.onw.Text;
            this.oNsf = initialwin.onsf.Text;
            this.ONss = initialwin.onss.Text;
            this.Nw = initialwin.nw.Text;
            this.Ns1 = initialwin.ns1.Text;
            this.Ns2 = initialwin.ns2.Text;
            this.N03w = initialwin.n03w.Text;
            this.N03s1 = initialwin.n03s1.Text;
            this.N03s2 = initialwin.n03s2.Text;
            this.a = initialwin.a.Text;
            this.b = initialwin.b.Text;
            this.mw = initialwin.mw.Text;
            this.ms = initialwin.ms.Text;
            this.ow = initialwin.ow.Text;
            this.Pw = initialwin.pw.Text;
            this.Ps1 = initialwin.ps1.Text;
            this.Ps2 = initialwin.ps2.Text;
            this.DOCw = initialwin.docw.Text;
            this.LPOCw = initialwin.lpocw.Text;
            this.RPOCw = initialwin.rpocw.Text;
            this.DOCs1 = initialwin.docs1.Text;
            this.LPOCs1 = initialwin.lpocs1.Text;
            this.RPOCs1 = initialwin.rpocs1.Text;
            this.DOCs2 = initialwin.docs2.Text;
            this.LPOCs2 = initialwin.lpocs2.Text;
            this.RPOCs2 = initialwin.rpocs2.Text;
            this.Ch4w = initialwin.ch4w.Text;
            this.CH4s1 = initialwin.ch4s1.Text;
            this.CH4s2 = initialwin.ch4s2.Text;
        }
    }
}
