using System;
using System.Diagnostics;
using System.Timers;

namespace BuildsAppReborn.Infrastructure
{
    public class ProgressTimer
    {
        public ProgressTimer()
        {
            this.mainTimer = new Timer();
            this.progressTimer = new Timer();
            this.stopWatch = new Stopwatch();

            this.mainTimer.Elapsed += (sender, args) => this.stopWatch.Restart();
        }

        public Double CurrentInterval => this.stopWatch.ElapsedMilliseconds;

        public Double Interval
        {
            get { return this.mainTimer.Interval; }
            set { this.mainTimer.Interval = value; }
        }

        public void Start()
        {
            this.mainTimer.Start();
            this.progressTimer.Start();
            this.stopWatch.Start();
        }

        public void Stop()
        {
            this.mainTimer.Stop();
            this.progressTimer.Stop();
            this.stopWatch.Stop();
        }

        public event ElapsedEventHandler Elapsed
        {
            add { this.mainTimer.Elapsed += value; }
            remove { this.mainTimer.Elapsed -= value; }
        }

        public event ElapsedEventHandler ProgressElapsed
        {
            add { this.progressTimer.Elapsed += value; }
            remove { this.progressTimer.Elapsed -= value; }
        }

        private readonly Timer mainTimer;
        private readonly Timer progressTimer;
        private readonly Stopwatch stopWatch;
    }
}