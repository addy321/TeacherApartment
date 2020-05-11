using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Checkin
    {
        public int id { get; set; }
        public string teacherAccount { get; set; }
        public int roomid { get; set; }
        public string enterTime { get; set; }
        public int prove { get; set; }
        public string scheduledTime { get; set; }
        public string roomNummber { get; set; }
        public int islive { get; set; }
        public string liveTime { get; set; }
    }
}
