using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeObjectLayer
{
    public class UneLinkedListDeVoisins
    {
        public UneLinkedListDeVoisins() { }
        public Voisin Head = null;
        public Voisin curr = null;
        public int NodeCount = 0;
        public void AddaVoisin(Voisin thisguy)
        {
            //unVoisin addnode = new unVoisin(s);
            if (Head == null) { Head = thisguy ; curr = Head; }
            else { 
                curr.nextVoisin = thisguy;
                curr = curr.nextVoisin;
            }
            _numberOfVoisins++;
            
        }

        public void AddSlipVoisin(Voisin slip) {
            if (Head == null) { Head = slip; curr = Head; }
            else {
                curr = Head;
                Head = slip;
                slip.nextVoisin = curr;              
            }
        }

        public void addVoisin__LAAST(Voisin voiss)
        {
            voiss.nextVoisin = null;

            if (Head == null) { Head = voiss; curr = Head; }
            else
            {
                while (curr.nextVoisin != null) { curr = curr.nextVoisin; }
            }
            curr.nextVoisin = voiss;    
        }


        public void AddaVoisinAtStart(Voisin thisdude) {

            if (Head == null) { Head = thisdude; curr = Head; }
            else
            {


                for (curr = Head; curr != null; curr = curr.nextVoisin)
                {
                    if (curr.nextVoisin == null) curr.nextVoisin = thisdude;
                
                }
            }
        
        }

        private int _numberOfVoisins=0;
        public int NumberOfVoisins
        {
            get { return _numberOfVoisins; }
            set { _numberOfVoisins = value; }
        }



    }
}
