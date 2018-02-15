using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithDifferentName
    {
        [Required]
        [Display(Name = "Needed")]
        public string DifferentName { get; set; }

        [Required]
        [Display(Name = "Contains.Period")]
        public string ContainsPeriod { get; set; }
    }
}
