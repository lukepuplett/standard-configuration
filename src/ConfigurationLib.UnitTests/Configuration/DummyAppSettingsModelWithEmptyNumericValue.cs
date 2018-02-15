using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithEmptyNumericValue
    {
        [Required]
        public int Empty { get; set; }
    }
}
