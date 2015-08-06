using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeObjectLayer
{
    public class uneLinkedListForReading
    {
        public uneLinkedListForReading() { }
        public uneNodeforReading Head = null;
        public uneNodeforReading curr = null;
        public void AddaReadnode(string s)
        {
            uneNodeforReading addnode = new uneNodeforReading(s);
            if (Head == null) { Head = addnode; curr = Head; }
            else { 
                curr.nextNodeforReading = addnode;
                curr = curr.nextNodeforReading;
            }
            
        }
    }
}




