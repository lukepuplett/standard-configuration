using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithMaxLength
    {
        [Required]
        [Display(Name = "Contains.Period")]
        [MaxLength(4)]
        public string ContainsPeriod { get; set; }
    }
}
