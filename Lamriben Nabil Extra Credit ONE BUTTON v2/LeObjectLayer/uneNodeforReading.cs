using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeObjectLayer
{
    /// <summary>
    /// the basic node for created from reading the text file
    ///  2->3->4-> ... n->null
    /// </summary>
    public class uneNodeforReading
    {

       public  uneNodeforReading() { }
       public uneNodeforReading(string v) : this() { val = v; }
        public string val = string.Empty;
        public uneNodeforReading nextNodeforReading = null;



    }
}
