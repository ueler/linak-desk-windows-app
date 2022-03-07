using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LinakDeskController
{
    internal class LinakDeskCommandCoordinator
    {
        private Subject<LinakDeskCommand<short>> commandQueue = new Subject<LinakDeskCommand<short>>();
        private Subject<short> heightSubject = new Subject<short>();

        private short targetHeight = -1;
        private short height = -1;
        private readonly short tolerance = 50;

        public LinakDeskCommandCoordinator()
        {
            commandQueue.Select(c =>
            {
                return Observable.FromAsync((_) => c.command).Do((r) =>
                {
                    c.resultAction.Invoke(r);
                });
            })
            .Concat()
            .Subscribe();

            var timer = Observable
               .Timer(
                   TimeSpan.FromSeconds(0),
                   TimeSpan.FromMilliseconds(400)
               );
            timer.Subscribe(_ =>
            {
                var getHeightCommand = createGetHeightCommand();
                commandQueue.OnNext(getHeightCommand);

                heightSubject.OnNext(this.height);

                if (this.height > this.targetHeight + tolerance || this.height < this.targetHeight - tolerance)
                {
                    var setHeightCommand = createSetHeightCommand(this.targetHeight);
                    commandQueue.OnNext(setHeightCommand);
                }
            });
        }

        public void setDeskTargetHeight(short height)
        {
            this.targetHeight = height;
        }

        public Subject<short> getHeightSubject()
        {
            return this.heightSubject;
        }

        private LinakDeskCommand<short> createGetHeightCommand()
        {
            return new LinakDeskCommand<short>(
                LinakDeskHID.getDeskHeight(),
                (height) => {
                    if (this.targetHeight == -1)
                    {
                        this.height = height;
                        this.targetHeight = height;
                    }
                    else
                    {
                        this.height = height;
                    }
                }
            );
        }

        private LinakDeskCommand<short> createSetHeightCommand(short height)
        {
            return new LinakDeskCommand<short>(
                LinakDeskHID.setDeskHeight(height),
                (res) => { }
            );
        }
     }
}
