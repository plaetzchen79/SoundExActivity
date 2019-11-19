using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundExActivity;
using System.Activities;
using System.Collections.Generic;

namespace SoundExTest
{
    [TestClass]
    public class SoundExUnitTest
    {
        [TestMethod]
        public void TestSoundex()
        {
            SoundExTranslate soundExtest = new SoundExTranslate();
            soundExtest.UseGerman = false;

            Assert.AreEqual(soundExtest.Generate("H"), "H000");
            Assert.AreEqual(soundExtest.Generate("Robert"), "R163");
            Assert.AreEqual(soundExtest.Generate("Rupert"), "R163");
            Assert.AreEqual(soundExtest.Generate("Rubin"), "R150");
            Assert.AreEqual(soundExtest.Generate("Ashcraft"), "A261");
            Assert.AreEqual(soundExtest.Generate("Ashcroft"), "A261");
            Assert.AreEqual(soundExtest.Generate("Tymczak"), "T522");
            Assert.AreEqual(soundExtest.Generate("Pfister"), "P236");
            Assert.AreEqual(soundExtest.Generate("Gutierrez"), "G362");
            Assert.AreEqual(soundExtest.Generate("Jackson"), "J250");
            Assert.AreEqual(soundExtest.Generate("VanDeusen"), "V532");
            Assert.AreEqual(soundExtest.Generate("Deusen"), "D250");
            Assert.AreEqual(soundExtest.Generate("Sword"), "S630");
            Assert.AreEqual(soundExtest.Generate("Sord"), "S630");
            Assert.AreEqual(soundExtest.Generate("Log-out"), "L230");
            Assert.AreEqual(soundExtest.Generate("Logout"), "L230");
            Assert.AreEqual(soundExtest.Generate("123"), SoundExTranslate.Empty);
            Assert.AreEqual(soundExtest.Generate(""), SoundExTranslate.Empty);
            Assert.AreEqual(soundExtest.Generate(null), SoundExTranslate.Empty);
        }

        [TestMethod]
        public void TestGerman()
        {
            SoundExTranslate soundExtest = new SoundExTranslate();
            soundExtest.UseGerman = true;

            Assert.AreEqual(soundExtest.Generate("Müller-Lüdenscheidt"), "65752682");
            Assert.AreEqual(soundExtest.Generate("Wikipedia"), "3412");
            //Assert.AreEqual(soundExtest.Generate("Heinz Classen"), "068 4586");
            Assert.AreEqual(soundExtest.Generate("HeinzClassen"), "068586");
            Assert.AreEqual(soundExtest.Generate("Meier"), "67");
            Assert.AreEqual(soundExtest.Generate("Meyer"), "67");
            Assert.AreEqual(soundExtest.Generate("Müller"), "657");
            Assert.AreEqual(soundExtest.Generate("Mueller"), "657");
            Assert.AreEqual(soundExtest.Generate("Mueler"), "657");
            Assert.AreEqual(soundExtest.Generate(null), SoundExTranslate.Empty);
        }


        [TestMethod]
        public void TestParameterActivity()
        {
            var activity = new SoundExCustomActivity();

            var input = new Dictionary<string, object>
            {
                { "Phrase", "Rubin" }
            };

            var output = WorkflowInvoker.Invoke(activity, input);

            Assert.AreEqual(output, "R163");
        }
    }
}
