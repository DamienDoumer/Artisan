using System;
using Artisan.MVVMShared;
using Dao;
using Dao.Entities;

namespace Artisan.ViewModels
{
    public class MainMenuViewModel : BindableBase
    {
        WorkingSessionDao wrkDao;
        EventDao evtDao;

        public WorkingSession ClossestWorkingSession { get; set; }
        public Event ClossestEvent { get; set; }
        public string EvtTime { get; set; }

        public MainMenuViewModel()
        {
            wrkDao = new WorkingSessionDao("WorkingSession");
            evtDao = new EventDao("Event");

            ClossestEvent = evtDao.RetrieveClosestEvent();

            ///If there is no clossest event i.e new user scenario
            if(ClossestEvent.ID == 0)
            {
                ClossestEvent = null;
            }

            try
            {
                ClossestWorkingSession = wrkDao.RetrieveClosestWorkingSession();
            }
            catch
            {
                ClossestWorkingSession = new WorkingSession();
                ClossestWorkingSession.TimeNarative = "";
            }
        }
    }
}
