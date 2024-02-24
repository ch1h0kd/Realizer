using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Models
{
    public class VisitDateGroup : ObservableCollection<Visit>
    {
        public DateTime Date { get; private set; }//header

        private SortedSet<DateTime> ActiveDates { get; set; }//a list of days that have at least one visit

        public VisitDateGroup(DateTime date, ObservableCollection<Visit> visits) : base(visits)
        {
            Date = date;
        }
    }
}
