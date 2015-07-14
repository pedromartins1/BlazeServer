using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    class RedirectorComponent
    {
        public static void HandleGetServerInstanceRequest(FireFrame fireframe)
        {
            Log.Info("got request for getServerInstance");
        }

        public static void HandleRequest(FireFrame fireframe)
        {
            switch (fireframe.GetCommand())
            {
                case 0x1:
                    HandleGetServerInstanceRequest(fireframe);
                    break;

                default:
                    Log.Warn(string.Format("Received an unhandled request: {0}::{1}.", fireframe.GetComponent(), fireframe.GetCommand()));
                    break;
            }
        }
    }
}
