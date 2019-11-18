using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;

namespace SoundExActivity
{
    public class SoundExCustomActivity: CodeActivity
    {

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Phrase { get; set; }

        [Category("Input")]
        public InArgument<bool> UseGernan { get; set; } = false;

        [Category("Output")]
        public OutArgument<string> Code { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // ToDo..
        }
    }
}
