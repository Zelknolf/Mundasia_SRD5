using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Mundasia.Communication
{
    public partial class ServiceConsumer
    {
        public static int SessionId;
        public static string UserId;
        public static BackgroundWorker Worker;
        public static int UpdateDelay = 250;

        static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(Worker.CancellationPending)
            {
                return;
            }

            Update(UserId, SessionId);

            if (Worker.CancellationPending)
            {
                return;
            }

            Thread.Sleep(UpdateDelay);
        }

        static void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(Worker.CancellationPending)
            {
                return;
            }

            Worker.RunWorkerAsync();
        }
    }
}
