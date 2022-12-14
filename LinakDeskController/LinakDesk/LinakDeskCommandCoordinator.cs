using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using LinakDeskController.LinakDesk;

namespace LinakDeskController
{
    public class LinakDeskCommandCoordinator
    {
        private const short Tolerance = 50;
        private readonly bool _running = true;
        private bool _targetReached = true;

        private readonly Subject<short> _heightSubject = new();

        private short _targetHeight = -1;
        private short _height = -1;

        public void Run()
        {
            Thread thread = new Thread(ReadAndSet);
            thread.Start();
        }

        private async void ReadAndSet()
        {
            _height = await LinakDeskHid.GetDeskHeight();
            _targetHeight = _height;

            while (_running)
            {
                if (_targetReached)
                {
                    await Task.Delay(2000);
                    continue;
                }

                _height = await LinakDeskHid.GetDeskHeight();
                _heightSubject.OnNext(_height);

                if (_height > _targetHeight + Tolerance || _height < _targetHeight - Tolerance)
                {
                    await LinakDeskHid.SetDeskHeight(_targetHeight);
                    await Task.Delay(200);
                }
                else
                {
                    _targetReached = true;
                }
            }
        }

        public void MoveToStandingHeight(LinakDeskControllerSettings settings)
        {
            SetDeskTargetHeight((short)(settings.StandingHeight * 100));
            _targetReached = false;
        }

        public void MoveToSittingHeight(LinakDeskControllerSettings settings)
        {
            SetDeskTargetHeight((short)(settings.SittingHeight * 100));
            _targetReached = false;
        }

        private void SetDeskTargetHeight(short height)
        {
            _targetHeight = height;
        }

        public DeskStatus GetDeskStatus(LinakDeskControllerSettings settings)
        {
            if (_height < (settings.SittingHeight * 100) + Tolerance)
            {
                return DeskStatus.OnSittingHeight;
            }

            if (_height > (settings.StandingHeight * 100) - Tolerance)
            {
                return DeskStatus.OnStandingHeight;
            }

            return DeskStatus.OnOtherHeight;
        }

        public Subject<short> GetHeightSubject()
        {
            return _heightSubject;
        }
    }
}