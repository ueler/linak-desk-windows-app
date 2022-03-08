using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace LinakDeskController
{
    internal class LinakDeskCommandCoordinator
    {
        private bool running = true;

        private Subject<short> heightSubject = new Subject<short>();

        private short targetHeight = -1;
        private short height = -1;
        private readonly short tolerance = 50;

        public LinakDeskCommandCoordinator()
        {
        }

        public void run()
        {
            Thread thread = new Thread(readAndSet);
            thread.Start();
        }

        private async void readAndSet()
        {
            this.height = await LinakDeskHID.getDeskHeight();
            this.targetHeight = this.height;

            while (running)
            {
                this.height = await LinakDeskHID.getDeskHeight();
                this.heightSubject.OnNext(this.height);

                if (this.height > this.targetHeight + tolerance || this.height < this.targetHeight - tolerance)
                {
                    await LinakDeskHID.setDeskHeight(this.targetHeight);
                    await Task.Delay(200);
                }
                else
                {
                    await Task.Delay(2000);
                }
            }
        }

        public void setDeskTargetHeight(short height)
        {
            this.targetHeight = height;
        }

        public Subject<short> getHeightSubject()
        {
            return this.heightSubject;
        }
     }
}
