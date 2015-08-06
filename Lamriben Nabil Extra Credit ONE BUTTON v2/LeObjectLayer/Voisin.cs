using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeObjectLayer
{
    public class Voisin
    {
        public Voisin() { }
        public Voisin(string str) : this() { _____id_ofthisVoisin = Int32.Parse(str); _ischecke = false; _ftimestamp = 0; _stamp_dtime = 0; }
        public Voisin(Voisin copyguy) : this() {
            _____id_ofthisVoisin = copyguy._____id_ofthisVoisin;
            nextVoisin = null;
            _ischecke =  copyguy._ischecke;
            _ftimestamp = copyguy._ftimestamp;
            _stamp_dtime = copyguy._stamp_dtime;

        }


        public Voisin(int i,  int d, int f ) //Used by Create transpose
            : this()
        {
            _____id_ofthisVoisin = i;
            _stamp_dtime = d;
            _ftimestamp = f;
           // _stamp_pi = p;
        }

        public int _____id_ofthisVoisin;
        public Voisin nextVoisin;

        private bool _ischecke = false;
        public bool IsCHekdforSoring
        {
            get { return _ischecke; }
            set { _ischecke = value; }
        }

        private int _stamp_dtime=0;
        public int Stamp_DTime
        {
            get { return _stamp_dtime; }
            set { _stamp_dtime = value; }
        }


        private int _ftimestamp=0;
        public int Stamp_FTime
        {
            get { return _ftimestamp; }
            set { _ftimestamp = value; }
        }

        private int _stamp_pi;
        public int Stamp_PI
        {
            get { return _stamp_pi; }
            set { _stamp_pi = value; }
        }


 
    }
}
