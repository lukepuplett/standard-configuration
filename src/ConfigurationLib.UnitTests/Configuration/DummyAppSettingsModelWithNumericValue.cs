﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithNumericValue
    {
        [Required]
        public int Number { get; set; }
    }
}