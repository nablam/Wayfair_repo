using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using kBusinessLayer;
using LeObjectLayer;

using System.Reflection;
namespace InterfaceLayer
{
    public partial class MaineWindowForm : Form
    {
        public MaineWindowForm()
        {
            InitializeComponent();
        }

        #region BUTTONS

        private void OnreadTXT(object sender, EventArgs e)
        {
            NodeManager.CLEARALLSTRUCTUIRES();
            _rich_TXT_Main.Clear();
            _rich_TXT_Trans.Clear();
            _richTextBox1_FOrest.Clear();

            OpenFileDialog OpenDiag = new OpenFileDialog();
            OpenDiag.InitialDirectory = INIT_DIR;
            OpenDiag.RestoreDirectory = true;
            using (OpenDiag)
            {
                OpenDiag.Filter = "txt Files|*.txt|All Files|*.*";
                if (OpenDiag.ShowDialog(this) == DialogResult.OK)
                {
                    NodeManager.ReadFile_and_create_stringArray(OpenDiag.FileName);
                }
                else
                    return;
            }

            PrintMainGraph();

            OnDFSMAin(sender, e);
            OnCreateTranspose(sender, e);
            OnDFS_transpose(sender, e);
            OnShowForrest(sender, e);
            showOriginalTXT();
        }

        private void showOriginalTXT()
        {

            _richINPUT.Clear();
            _richINPUT.AppendText("  INPUT FILE:    ");
            _richINPUT.AppendText(Environment.NewLine);
            _richINPUT.AppendText(Environment.NewLine);
   
            for (int x = 0; x < NodeManager.lines.Length; x++)
            {
                _richINPUT.AppendText(NodeManager.lines[x]);


                _richINPUT.AppendText(Environment.NewLine);
            }

        }

        private void OnDFSMAin(object sender, EventArgs e)
        {
            NodeManager.DFS_G(NodeManager.MASTER_Graph);
            PrintMainGraph();
        }

        private void OnCreateTranspose(object sender, EventArgs e)
        {
            NodeManager.CreateTranspose3(NodeManager.MASTER_Graph);

            PrintTranspose();
        }

        private void OnDFS_transpose(object sender, EventArgs e)
        {

            NodeManager.DFS_GT(NodeManager.MASTER_GT_Graph);

            PrintTranspose();
            NodeManager.Order_Transpose_getMAPPING_reorder(NodeManager.MASTER_GT_Graph);
        }

        private void OnShowForrest(object sender, EventArgs e)
        {

            _richTextBox1_FOrest.Clear();
            _richTextBox1_FOrest.AppendText("  FORESTS    ");
            _richTextBox1_FOrest.AppendText(Environment.NewLine);
            _richTextBox1_FOrest.AppendText(Environment.NewLine);
            int Indexnextlargest = 0;
            for (int x = 1; x < NodeManager.Forests.Length; x++)
            {
                Indexnextlargest = NodeManager.MAPPINGtable[x];

                if (NodeManager.MASTER_GT_Graph[Indexnextlargest].PI > 1000)
                    {
                        _richTextBox1_FOrest.AppendText("Root " + Indexnextlargest.ToString() + " forest----> ");
                        foreach (int y in NodeManager.Forests[Indexnextlargest]) { _richTextBox1_FOrest.AppendText(y.ToString() + " , "); }
                    }

                
                _richTextBox1_FOrest.AppendText(Environment.NewLine);
            } 

        }


        public void OUTPUTFILE() {





        }
      
#endregion

        #region PrivateMethods

        private void PrintTranspose() {

            // NodeManager.PrintmyyMaster(NodeManager.MASTER_GT_Graph);
            _rich_TXT_Trans.Clear();
            _rich_TXT_Trans.AppendText("The format is:");
            _rich_TXT_Trans.AppendText(" Node (X)  ( D.time,F.tme )  Adjacent -> Node X (D.time,F.tme) ");
            _rich_TXT_Trans.AppendText(Environment.NewLine);
            for (int x = 1; x < NodeManager.MASTER_GT_Graph.Length; x++)
            {
                if (NodeManager.MASTER_GT_Graph[x] != null)
                {
                    _rich_TXT_Trans.AppendText(Environment.NewLine);

                    string self = string.Empty;
                    if (NodeManager.MASTER_GT_Graph[x].PI < 1) { self = "____ "; }
                    else
                        if (NodeManager.MASTER_GT_Graph[x].PI > 200) { self = "ROOT "; }
                        else
                        {
                            self = "PID   " + NodeManager.MASTER_GT_Graph[x].PI.ToString()
                                + "  ";
                        }

                    _rich_TXT_Trans.AppendText(

                      //  self +
                        "    Node " + NodeManager.MASTER_GT_Graph[x]._VC_ID +
                        "  (" + NodeManager.MASTER_GT_Graph[x].DISCOVERYtime +
                        "," + NodeManager.MASTER_GT_Graph[x].FINtime + ") " +
                        " --> ");

                    //  _richTextBoxMain.AppendText(Environment.NewLine);

                    for (Voisin vzn = NodeManager.MASTER_GT_Graph[x].adjacent; vzn != null; vzn = vzn.nextVoisin)
                    {

                        _rich_TXT_Trans.AppendText(
                            "  V." + vzn._____id_ofthisVoisin +
                            "  ( " + vzn.Stamp_DTime +
                            ", " + vzn.Stamp_FTime + ") "
                            );

                    }
                    _rich_TXT_Trans.AppendText("");
                    _rich_TXT_Trans.AppendText(Environment.NewLine);
                }

            }
        }










        private void PrintMainGraph()
        {
            _rich_TXT_Main.Clear();
            _rich_TXT_Main.AppendText("The format is:");
            _rich_TXT_Main.AppendText(" Node (X)  ( D.time,F.tme )  Adjacent -> Node X (D.time,F.tme) ");
            _rich_TXT_Main.AppendText(Environment.NewLine);
            for (int x = 1; x < NodeManager.MASTER_Graph.Length; x++)
            {
                if (NodeManager.MASTER_Graph[x] != null)
                {
                    _rich_TXT_Main.AppendText(Environment.NewLine);

                    string self = string.Empty;
                    if (NodeManager.MASTER_Graph[x].PI > 2000) { self = "isROOT "; }
                    else { self = "Parent= " + NodeManager.MASTER_Graph[x].PI.ToString() + "   "; }

                    _rich_TXT_Main.AppendText(

                        " Node (" +
                        NodeManager.MASTER_Graph[x]._VC_ID + ")"+

                       // self +
                       
                       "  D.time:" +
                        NodeManager.MASTER_Graph[x].DISCOVERYtime +
                        " , F.tme:" + NodeManager.MASTER_Graph[x].FINtime +
                        " Adjacent -> ");


                    for (Voisin vzn = NodeManager.MASTER_Graph[x].adjacent; vzn != null; vzn = vzn.nextVoisin)
                    {
                        _rich_TXT_Main.AppendText(
                            "  N.(" + vzn._____id_ofthisVoisin + ")" +
                            "  D.time:" + vzn.Stamp_DTime +
                            " , F.tme:" + vzn.Stamp_FTime 
                            );

                    }
                    _rich_TXT_Main.AppendText("");
                    _rich_TXT_Main.AppendText(Environment.NewLine);
                }

            }


        }










        #endregion

        private void OnClearalllRestart(object sender, EventArgs e)
        {
            NodeManager.CLEARALLSTRUCTUIRES();
        }

        private string INIT_DIR = string.Empty;
        private void button2_Click(object sender, EventArgs e)
        {
         
            int Indexnextlargest;

            SaveFileDialog SaveDiag = new SaveFileDialog();
                SaveDiag.InitialDirectory = INIT_DIR;
                SaveDiag.RestoreDirectory = true;
               // SaveDiag.DefaultExt = "p2_out.txt";
                using (SaveDiag)
                {
                    SaveDiag.Filter = "txt Files|*.txt|All Files|*.*";
                    if (SaveDiag.ShowDialog(this) == DialogResult.OK)
                    {

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(SaveDiag.FileName))
                        {

                            for (int x = 1; x < NodeManager.Forests.Length; x++)
                            {
                                Indexnextlargest = NodeManager.MAPPINGtable[x];

                                if (NodeManager.MASTER_GT_Graph[Indexnextlargest].PI > 1000)
                                {
                                   
                                    file.WriteLine(" list of strongly connected nodes at root "+  Indexnextlargest.ToString() );
                                    foreach (int y in NodeManager.Forests[Indexnextlargest]) { file.Write(y.ToString() + " , "); }
                                }


                                file.WriteLine("");
                            } 


                 
                        }
                       

                    }
                    else
                        return;
                }

        }

        private void MaineWindowForm_Load(object sender, EventArgs e)
        {
            Application.Idle += (this.OnIdle);

            string exe = Assembly.GetExecutingAssembly().Location;
            INIT_DIR = Path.GetDirectoryName(exe);
        }
        private void OnIdle(object sender, EventArgs e) {

            _buttonSave.Enabled = NodeManager.MASTER_Graph != null;
        
        }

      

    }
}
