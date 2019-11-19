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

        /// <summary>
        /// Set to true if you wish to use a german optimized soundex (cologne phonetics)
        /// </summary>
        [Category("Input")]
        public InArgument<bool> UseGernan { get; set; } = false;

        /// <summary>
        /// Result SoundEx Code
        /// </summary>
        [Category("Output")]
        public OutArgument<string> Code { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // ToDo..
            SoundExTranslate translation = new SoundExTranslate();
            translation.UseGerman = UseGernan.Get(context);

            var result = translation.Generate(Phrase.Get(context));
            Code.Set(context, result);
        }
    }
}
