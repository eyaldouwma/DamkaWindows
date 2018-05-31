using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;

namespace DamkaUI
{
    class DelayTimer : Timer
    {
        public DelayTimer(ISynchronizeInvoke i_MainForm, int i_Delay)
        {
            this.AutoReset = false;
            this.SynchronizingObject = i_MainForm;
            this.Interval = i_Delay;
        }
    }
}
