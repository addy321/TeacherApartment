using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Room
    {
        public int id { get; set; }
        public string roomNumber { get; set; }
        public int Types { get; set; } = -1;
        public int status { get; set; } = -1;
    }
}
