using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Evoq.Configuration
{
    class DummyAppSettingsModelWithEmptyNotRequired
    {
        public string Empty { get; set; }

        public string Nonexistent { get; set;  }
    }
}
