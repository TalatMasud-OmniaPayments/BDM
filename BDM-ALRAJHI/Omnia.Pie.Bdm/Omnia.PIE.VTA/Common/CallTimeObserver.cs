using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Windows.Forms;

namespace Omnia.PIE.VTA.Common
{
    public class CallTimeObserver
    {
        public event EventHandler<EventArgs> CallTimerEvent;
        public Timer ObserverTimer = new Timer();
        public DateTime StartTime = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallTimeObserver"/> class.
        /// </summary>
        public CallTimeObserver()
        {
            ObserverTimer.Interval = 1000;
            ObserverTimer.Tick += ObserverTimer_Tick;
        }

        /// <summary>
        /// Handles the Tick event of the ObserverTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ObserverTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var timeSinceStartTime = DateTime.Now - StartTime;
                timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours, timeSinceStartTime.Minutes, timeSinceStartTime.Seconds);
                CallTimerEvent?.Invoke(this, null);
            }
            catch (Exception ex)
            {
				Logger.Writer.Exception(ex);
            }
        }

        /// <summary>
        /// Stops the observer.
        /// </summary>
        public void StopObserver()
        {
            ObserverTimer.Stop();
            ObserverTimer.Dispose();
        }
    }
}
