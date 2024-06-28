using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.CallEngine
{
    //This will eventually become the base class for our factory. We are going to use it to set up our independant call framework for now. 
    internal class HttpCall
    {
       public async Task InitialCall(HttpCredentials creds, string path, string queryParams)
        {

        }
    }
}
