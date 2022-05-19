using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.Navigation;
using System;
using System.Windows.Input;

namespace MyDbtDiaryCard.ViewModels
{
    internal class BaseOverviewViewModel : BaseViewModel
    {
        private bool isEnoughEntries;
        public bool IsEnoughEntries
        {
            get { return isEnoughEntries; }
            set { SetProperty(ref isEnoughEntries, value); }
        }

        private int daysRange = 7;
        public virtual int DaysRange
        {
            get { return daysRange; }
            set
            {
                int oldValue = daysRange;
                SetProperty(ref daysRange, value);
                EndDate = EndDate.AddDays(value - oldValue);
            }
        }

        private DateTime startDate;
        public virtual DateTime StartDate
        {
            get => startDate;
            set
            {
                if (value > DateTime.Today)
                    throw new Exception("Start date must not be from future");

                SetProperty(ref startDate, value);
                EndDate = value.AddDays(DaysRange);

            }
        }

        private DateTime endDate;
        public virtual DateTime EndDate
        {
            get => endDate;
            set
            {
                if (value > DateTime.Today)
                    value = DateTime.Today;

                SetProperty(ref endDate, value);
            }
        }

        public ICommand GetPreviousRangeCommand { get; }
        public ICommand GetNextRangeCommand { get; }

        public BaseOverviewViewModel(INavigationService navigation) : base(navigation)
        {
            StartDate = DateTime.Today.AddDays(-DaysRange);

            GetNextRangeCommand = new DateCommand(() => StartDate = StartDate.AddDays(DaysRange));
            GetPreviousRangeCommand = new ActionCommand(GetPreviousRange);
        }

        private void GetPreviousRange()
        {
            var date = StartDate.Ticks;
            var days = TimeSpan.FromDays(DaysRange).Ticks;

            if (date - days < DateTime.MinValue.Ticks)
            {
                StartDate = DateTime.MinValue;
                return;
            }

            StartDate = StartDate.AddDays(-DaysRange);

        }
    }
}
