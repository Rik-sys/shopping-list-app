﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CompleteOrderRequestDto
    {
        public Guid SessionId { get; set; }
        public string? Notes { get; set; }

       
    }
}
