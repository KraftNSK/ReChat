﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReChat.Models
{
    public class Log
    {
        public int Id { get; set; }
        public User User { get; set; }
        public System.DateTime DT { get; set; }
        public string Text { get; set; }
    }
}
