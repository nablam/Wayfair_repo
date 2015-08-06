using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using LeObjectLayer;


namespace kBusinessLayer
{
    public static class NodeManager
    {
        //*******************************************************
        public static Vertex_withColor[] MASTER_Graph;
        public static Vertex_withColor[] MASTER_GT_Graph; //GT stands for graph Transpose 
        public static List<int>[] Forests;
        public static int[] MAPPINGtable;
        //*******************************************************

        #region PrivateMemebers
        private static List<UneLinkedListDeVoisins> listoflists = new List<UneLinkedListDeVoisins>();
        public static string[] lines;
        private static List<string> mainNodesArraySTR = new List<string>();
        private static List<UneLinkedListDeVoisins> adjacentNodesArrayLINKED = new List<UneLinkedListDeVoisins>();   
        private static int TOTLALINESTOREAD = 0;      
        private static int T;
       
        #endregion

        #region PrivateMethods
        private static void Build_MASTER_Graph(string wholeline, int index)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n', ' ' };
            string[] words = wholeline.Split(delimiterChars);
            int entrycount = 0;
            foreach (string s in words)
            {
                if (entrycount == 0)
                {
                    MASTER_Graph[index] = new Vertex_withColor();
                    MASTER_Graph[index].WasVisited = false;
                    MASTER_Graph[index]._VC_ID = Int32.Parse(s);
                }
                else
                {
                    if (s != string.Empty || s != "")
                    {
                        string g = s;
                        MASTER_Graph[index]._OwnLinkedlistNotattached.AddaVoisin(new Voisin(g));
                    }
                }
                entrycount++;
            }
            MASTER_Graph[index].adjacent = MASTER_Graph[index]._OwnLinkedlistNotattached.Head;
        }

        public static void Order_Transpose_getMAPPING_reorder(Vertex_withColor[] bgraph)
        {

            int i2 = 2;
            int j2;
            for (i2 = 2; i2 < bgraph.Length; i2++)
            {
                Vertex_withColor temp = bgraph[i2];
                j2 = i2;
                while (j2 > 1 && bgraph[j2 - 1].FINtime < temp.FINtime)
                {
                    bgraph[j2] = bgraph[j2 - 1];
                    j2 = j2 - 1;
                }
                bgraph[j2] = temp;
            }



            MAPPINGtable = new int[bgraph.Length];
            for (int upper = 0; upper < bgraph.Length; upper++) MAPPINGtable[upper] = bgraph[upper]._VC_ID;




            int i1 = 2;
            int j1;
            for (i1 = 2; i1 < bgraph.Length; i1++)
            {
                if (bgraph[i1] == null) return;
                Vertex_withColor temp1 = bgraph[i1];
                j1 = i1;
                while (j1 > 1 && bgraph[j1 - 1]._VC_ID > temp1._VC_ID)
                {
                    bgraph[j1] = bgraph[j1 - 1];
                    j1 = j1 - 1;
                }
                bgraph[j1] = temp1;
            }



        }

        private static void OrderNodes_HIghtoLow_IN_PLACE(Vertex_withColor[] bgraph)
        {

            MAPPINGtable = new int[bgraph.Length];
            for (int upper = 0; upper < bgraph.Length; upper++) MAPPINGtable[upper] = upper;


            int i = 2;
            int j;
            for (i = 2; i < bgraph.Length; i++)
            {
                Vertex_withColor temp = bgraph[i];
                int te = MAPPINGtable[i];
                j = i;
                while (j > 1 && bgraph[j - 1].FINtime < temp.FINtime)
                {
                    MAPPINGtable[j] = MAPPINGtable[j - 1];
                    j = j - 1;
                }
                bgraph[j] = temp;
                MAPPINGtable[j] = te;
            }


        }

        private static void Stamp_Nodes_via_Adjacent(Vertex_withColor[] Vgraph)
        {
            for (int vertIndex = 1; vertIndex < Vgraph.Length; vertIndex++)
            {
                if (Vgraph[vertIndex] != null)
                {
                    for (Voisin vzn = Vgraph[vertIndex].adjacent; vzn != null; vzn = vzn.nextVoisin)
                    {
                        int VertIndexFinder = vzn._____id_ofthisVoisin;
                        int fstamp = Vgraph[VertIndexFinder].FINtime;
                        int dstamp = Vgraph[VertIndexFinder].DISCOVERYtime;
                        int pstamp = Vgraph[VertIndexFinder].PI;

                        vzn.Stamp_FTime = fstamp;
                        vzn.Stamp_DTime = dstamp;
                        vzn.Stamp_PI = pstamp;

                    }

                }
            }

        }

        private static void SORTeachROw(Vertex_withColor[] Tgraph)
        {

            foreach (Vertex_withColor V in Tgraph)
            {
                if (V != null)
                {
                    if (V.adjacent != null)
                        V.sort_OWNLIST_ByFinishtime(V._VC_ID);
                }


            }


        }
      
        private static void DFS_visit_GT(Vertex_withColor[] Uarray, int u, int Findexis)
        {
            Uarray[u].Wite1_Gray2_Black3 = 2;
            T++;
            Uarray[u].DISCOVERYtime = T;
            for (Voisin V = Uarray[u].adjacent; V != null; V = V.nextVoisin)
            {

                int v = V._____id_ofthisVoisin;
                // int realv = RealIndexof(v, Uarray);
                if (Uarray[v] == null) continue;
                if (Uarray[v].Wite1_Gray2_Black3 == 1)
                {
                    Uarray[v].PI = u;
                    DFS_visit_GT(Uarray, v, Findexis);
                }
            }
            T++;
            Forests[Findexis].Add(Uarray[u]._VC_ID);
            Uarray[u].Wite1_Gray2_Black3 = 3;
            Uarray[u].FINtime = T;
        }

        private static void DFS_visit_G(Vertex_withColor[] Uarray, int u)
        {

            Uarray[u].Wite1_Gray2_Black3 = 2;
            T++;
            Uarray[u].DISCOVERYtime = T;

            for (Voisin V = Uarray[u].adjacent; V != null; V = V.nextVoisin)
            {

                int v = V._____id_ofthisVoisin;
                // int realv = RealIndexof(v, Uarray);
                if (Uarray[v] == null) continue;
                if (Uarray[v].Wite1_Gray2_Black3 == 1 && !Uarray[v].WasVisited)
                {
                    Uarray[v].PI = u;
                    DFS_visit_G(Uarray, v);
                }
            }
            T++;
            Uarray[u].Wite1_Gray2_Black3 = 3;
            Uarray[u].FINtime = T;
        }
        #endregion

        #region PublicMethods
        public static void ReadFile_and_create_stringArray(string filenamepath)
        {
            lines = File.ReadAllLines(filenamepath);

            foreach (string line in lines)
            {
                TOTLALINESTOREAD++;
            }

            MASTER_Graph = new Vertex_withColor[TOTLALINESTOREAD + 1];
            MASTER_GT_Graph = new Vertex_withColor[TOTLALINESTOREAD + 1];

            MASTER_Graph[0] = new Vertex_withColor("113377");
            int theinex = 1;
            foreach (string line in lines)
            {             
                Build_MASTER_Graph(line, theinex);
                theinex++;
            }
        }

        public static void DFS_G(Vertex_withColor[] U)
        {
        

            int sizeU = U.Length;
            //****************************************************
            //  set all nodes to white.. and all are roots
            //****************************************************
            for (int cntload = 1; cntload < sizeU; cntload++)
            {
              

                if (U[cntload] != null)
                {
                    U[cntload].Wite1_Gray2_Black3 = 1;
                    U[cntload].PI = 999999;
                }

            }
            T = 0;
            //****************************************************
            //  DFS on nodes 1, 2, 3, 4  ... , n 
            //****************************************************
            for (int cnt = 1; cnt < sizeU; cnt++)
            {
                if (U[cnt] != null)
                {
                    if (U[cnt].Wite1_Gray2_Black3 == 1)
                    {
                        DFS_visit_G(U, cnt);
                    }
                }
            }






            //
            //   if (U[0]._VC_ID == 113377)
            Stamp_Nodes_via_Adjacent(U);


        }

        public static void CreateTranspose3(Vertex_withColor[] ofthisGraph)
        {
            //Organize_any_DFSedGraph(ofthisGraph);
            int transSize = ofthisGraph.Length;

            // GT = new Vertex_withColor[transSize];
            MASTER_GT_Graph[0] = new Vertex_withColor("74595"); MASTER_GT_Graph[0].PI = 0;

            for (int at_VertIndex = 1; at_VertIndex < transSize; at_VertIndex++)
            {

                if (ofthisGraph[at_VertIndex].PI > 9000)
                {
                    MASTER_GT_Graph[at_VertIndex] = new Vertex_withColor(ofthisGraph[at_VertIndex]);

                    for (Voisin vzn = ofthisGraph[at_VertIndex].adjacent; vzn != null; vzn = vzn.nextVoisin) //for 2 4 
                    {
                        int put_at_vert = vzn._____id_ofthisVoisin;
                        if (MASTER_GT_Graph[put_at_vert] == null)
                        {                      //4                                             //4
                            MASTER_GT_Graph[put_at_vert] = new Vertex_withColor(ofthisGraph[put_at_vert]); //special constructor
                            //4                                                                     //-->1
                            MASTER_GT_Graph[put_at_vert]._OwnLinkedlistNotattached.AddaVoisin(new Voisin(ofthisGraph[at_VertIndex]._VC_ID, ofthisGraph[at_VertIndex].DISCOVERYtime, ofthisGraph[at_VertIndex].FINtime));
                            MASTER_GT_Graph[put_at_vert].adjacent = MASTER_GT_Graph[put_at_vert]._OwnLinkedlistNotattached.Head;
                        }
                        else //stack
                        {                                                                                                   //
                            MASTER_GT_Graph[put_at_vert]._OwnLinkedlistNotattached.AddaVoisin(new Voisin(ofthisGraph[at_VertIndex]._VC_ID, ofthisGraph[at_VertIndex].DISCOVERYtime, ofthisGraph[at_VertIndex].FINtime));
                            MASTER_GT_Graph[put_at_vert].adjacent = MASTER_GT_Graph[put_at_vert]._OwnLinkedlistNotattached.Head;

                        }


                    }


                }

                else
                {
                    for (Voisin vzn = ofthisGraph[at_VertIndex].adjacent; vzn != null; vzn = vzn.nextVoisin)
                    {
                        int XXX = vzn._____id_ofthisVoisin;
                        if (MASTER_GT_Graph[XXX] == null)
                        {
                            MASTER_GT_Graph[XXX] = new Vertex_withColor(ofthisGraph[XXX]); //special constructor

                            MASTER_GT_Graph[XXX]._OwnLinkedlistNotattached.AddaVoisin(new Voisin(ofthisGraph[at_VertIndex]._VC_ID, ofthisGraph[at_VertIndex].DISCOVERYtime, ofthisGraph[at_VertIndex].FINtime));
                            MASTER_GT_Graph[XXX].adjacent = MASTER_GT_Graph[XXX]._OwnLinkedlistNotattached.Head;

                        }
                        else //stack
                        {
                            MASTER_GT_Graph[XXX]._OwnLinkedlistNotattached.AddaVoisin(new Voisin(ofthisGraph[at_VertIndex]._VC_ID, ofthisGraph[at_VertIndex].DISCOVERYtime, ofthisGraph[at_VertIndex].FINtime));
                            MASTER_GT_Graph[XXX].adjacent = MASTER_GT_Graph[XXX]._OwnLinkedlistNotattached.Head;

                        }

                    }


                }




            }

            SORTeachROw(MASTER_GT_Graph);

            // MASTER_GT_Graph = GT;
            Order_Transpose_getMAPPING_reorder(MASTER_GT_Graph);
            //  int yh= MAPPINGtable[0];
            //     GT=MASTER_GT_Graph;
        }

        public static void DFS_GT(Vertex_withColor[] U)
        {
            Forests = new List<int>[U.Length];
            
            for (int cntload = 1; cntload < U.Length; cntload++)
            {
                Forests[cntload] = new List<int>();
                if (U[cntload] != null)
                {
                    U[cntload].Wite1_Gray2_Black3 = 1;
                    U[cntload].PI = 999999;
                }
            }

            T = 0;

            for (int cnt = 1; cnt < U.Length; cnt++)
            {
                int here = MAPPINGtable[cnt];

                if (U[here] != null)
                {
                    if (U[here].Wite1_Gray2_Black3 == 1)
                    {
                        DFS_visit_GT(U, here , here);
                    }
                }
            }

            Stamp_Nodes_via_Adjacent(U);
        }

        public static void OrderByfinishing()
        {
            {

                //int i = 2;
                //int j;
                //for (i = 2; i < agraph.Length; i++)
                //{
                //    if (agraph[i] == null) return;
                //    Vertex_withColor temp = agraph[i];
                //    j = i;
                //    while (j > 1 && agraph[j - 1].FINtime > temp.FINtime)
                //    {
                //        agraph[j] = agraph[j - 1];
                //        j = j - 1;
                //    }
                //    agraph[j] = temp;
                //}



            }
        }

        #endregion


        public static void CLEARALLSTRUCTUIRES() {
            if (MASTER_Graph != null) MASTER_Graph = null;
            if (MASTER_GT_Graph != null) MASTER_GT_Graph = null;
            if (Forests != null) Forests = null;
            if (lines != null) lines = null;
            if (mainNodesArraySTR != null) mainNodesArraySTR = null;
            if (adjacentNodesArrayLINKED != null) adjacentNodesArrayLINKED = null;
            if (listoflists != null) listoflists = null;
            if (MAPPINGtable != null) MAPPINGtable = null;
            TOTLALINESTOREAD = 0;
        
        }



        public static void SAVEAGETXT() {
       

        
        
        
        }

    }
}

