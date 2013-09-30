using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SEN
{
    class Simulator
    {
        Simulation simulation;
        Thread simThread;

        public Simulator()
        {
            simulation = new Simulation();
            simThread = new Thread(new ThreadStart(simulation.Run));
        }

        public void Start()
        {
            if (!simThread.IsAlive || simThread != null)
            {
                simThread.Start();
            }
        }

        public void Stop()
        {
            if (simThread != null || simThread.IsAlive)
            {
                Thread.Sleep(1);
                simThread.Abort();
                simThread.Join();
            }
        }

        public void Restart()
        {
            if (!simThread.IsAlive || simThread == null)
            {
                simThread = new Thread(new ThreadStart(simulation.Run));
                this.Start();
            }
            else
            {
                this.Stop();
                this.Restart();
            }
        }
    }
}
