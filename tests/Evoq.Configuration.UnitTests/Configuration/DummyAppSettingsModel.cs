using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModel
    {
        [Required]
        public string RequiredString { get; set; }

        [Required]
        public int? RequiredNumber { get; set; } = 123;

        [MaxLength(4)]
        public string ShortString { get; set; }

        [Display(Name = "Apple")]
        public string Banana { get; set; }

        [Display(Name = "Contains.Period")]
        public string ContainsPeriod { get; set; }
    }
}
