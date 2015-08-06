using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeObjectLayer
{
    public class Vertex_withColor
    {

        public Vertex_withColor() {
            __vc__id = 0;
            adjacent = null;
            _white1_gray2_black3 = 1;
            _pi = 9999;
            _discTime = 0; 
            _fintime = 0; _homanyVoisins = 0; _wasVisited = false;  }

        public Vertex_withColor(string str) : this() {
            adjacent = null;
            __vc__id = Int32.Parse(str);
            _white1_gray2_black3 = 1;
            _pi = 9999; _discTime = 0;
            _fintime = 0;
            _homanyVoisins = 0; 
            _wasVisited = false; 
           
        }
       
        //used to Make Transpose 1: 2  put 1, if 2 on vert is free , make a ne vertwithcolor using this const
        public Vertex_withColor(Vertex_withColor thisguy) : this() {

           // adjacent = null;
           
           // _OwnLinkedlistNotattached.Head = null;
            _OwnLinkedlistNotattached = new UneLinkedListDeVoisins();
            adjacent = _OwnLinkedlistNotattached.Head;

            __vc__id = thisguy.__vc__id;
            _white1_gray2_black3 = 1;
            _pi =0;//                CANT SET THE PARENT YET
            _discTime = thisguy._discTime;
            _fintime = thisguy._fintime;
            _homanyVoisins++;

            _wasVisited = false;
           
        }

       


   

        private bool _wasVisited=false;
        public bool WasVisited
        {
            get { return _wasVisited; }
            set { _wasVisited = value; }
        }
        
 
        private int __vc__id;
        public int _VC_ID
        {
            get { return __vc__id; }
            set { __vc__id = value; }
        }

        /// <summary>
        /// as the name suggests , this isnteger will have a value of 
        /// 1  -> white
        /// 2  -> gray
        /// 3  -> black
        /// </summary>
        private int _white1_gray2_black3=1;
        public int Wite1_Gray2_Black3
        {
            get { return _white1_gray2_black3; }
            set { _white1_gray2_black3 = value; }
        }

        private int _pi;
        public int PI
        {
            get { return _pi; }
            set { _pi = value; }
        }


        private int _discTime;
        public int DISCOVERYtime
        {
            get { return _discTime; }
            set { _discTime = value; }
        }


        private int _fintime;
        public int FINtime
        {
            get { return _fintime; }
            set { _fintime = value; }
        }

        private int _homanyVoisins=0;
        public int HowmanyVoisins
        {
            get { return _homanyVoisins; }
            set { _homanyVoisins = value; }
        }


        public Voisin adjacent;

        public UneLinkedListDeVoisins _OwnLinkedlistNotattached = new UneLinkedListDeVoisins();

        public void sort_OWNLIST_ByFinishtime(int vnumber){

            int VoisinageCounter=0;
            for (Voisin vzn = _OwnLinkedlistNotattached.Head; vzn != null; vzn = vzn.nextVoisin)
            {
                VoisinageCounter++;
            }

            if (VoisinageCounter > 1)
            {
                UneLinkedListDeVoisins newOwnlist = new UneLinkedListDeVoisins();
                 //detatch head 
                    Voisin temphead = new Voisin();
                    temphead = this._OwnLinkedlistNotattached.Head;
                    //detach ownlist 
                    this.adjacent = null;
                 int maxcompare = 0;
                //************************************************************************************************Reverse
                    int laps = 0;
                    for (Voisin vzn = temphead; vzn != null; vzn = vzn.nextVoisin)
                    {
                        vzn.IsCHekdforSoring = false;
                        laps++;
                    }


                    Voisin goodvzn = null;
                    for (int lap = 0; lap < laps; lap++) {                      
                        maxcompare = 0;
                        for (Voisin vzn = temphead; vzn != null; vzn = vzn.nextVoisin)
                        {                          
                            if (!vzn.IsCHekdforSoring && vzn.Stamp_FTime > maxcompare) { maxcompare = vzn.Stamp_FTime;  goodvzn = vzn; }
                        }
                        newOwnlist.AddaVoisin(new Voisin(goodvzn));
                        goodvzn.IsCHekdforSoring = true;                    
                    }                   
                this.adjacent = newOwnlist.Head;  
            }    
        
        }

    }
}
