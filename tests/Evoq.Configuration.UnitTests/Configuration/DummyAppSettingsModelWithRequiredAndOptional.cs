﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithRequiredAndOptional
    {
        [Required]
        public string Needed { get; set; }

        public string MissingButNotNeeded { get; set; }
    }
}
