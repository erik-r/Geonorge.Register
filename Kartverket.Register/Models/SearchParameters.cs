﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.Register.Models
{
    public class SearchParameters
    {
        public SearchParameters()
        {
            Offset = 1;
            Limit = 10;
        }

        public string Text { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

    }
}