using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithWrongType
    {
        [Required]
        public int Needed { get; set; }
    }
}
