using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoundExActivity
{
    public class SoundExTranslate
    {
        public const string Empty = "0000";
        private static readonly Regex Sanitiser = new Regex(@"[^A-Z]", RegexOptions.Compiled);
        private static readonly Regex CollapseRepeatedNumbers = new Regex(@"(\d)?\1*[WH]*\1*", RegexOptions.Compiled);
        private static readonly Regex RemoveVowelSounds = new Regex(@"[AEIOUY]", RegexOptions.Compiled);
        private static readonly Regex GermanCAnlaut = new Regex(@"[AHKLOQRUX]", RegexOptions.Compiled);
        private static readonly Regex GermanC = new Regex(@"[AHKOQUX]", RegexOptions.Compiled);

        public bool UseGerman { get; set; } = false;

        /// <summary>
        /// Generate entry point - main function
        /// </summary>
        /// <param name="Phrase"></param>
        /// <returns></returns>
        public string Generate(string Phrase)
        {
            string convertedPhrase = String.Empty;

            if (this.UseGerman == true)
            {
                convertedPhrase = this.GenerateGerman(Phrase);
            }
            if (this.UseGerman == false)
            {
                convertedPhrase = this.GenerateSoundex(Phrase);
            }

            return convertedPhrase;
        }


        /// <summary>
        /// Generate with standard soundex for english languages
        /// </summary>
        /// <param name="Phrase"></param>
        /// <returns></returns>
        private string GenerateSoundex(string Phrase)
        {
            // Remove non-alphas
            Phrase = Sanitiser.Replace((Phrase ?? string.Empty).ToUpper(), string.Empty);

            // Nothing to soundex, return empty
            if (string.IsNullOrEmpty(Phrase))
                return Empty;

            // Convert consonants to numerical representation
            var Numified = this.Numify(Phrase);

            // Remove repeated numberics (characters of the same sound class), even if separated by H or W
            Numified = CollapseRepeatedNumbers.Replace(Numified, @"$1");

            if (Numified.Length > 0 && Numified[0] == Numify(Phrase[0]))
            {
                // Remove first numeric as first letter in same class as subsequent letters
                Numified = Numified.Substring(1);
            }

            // Remove vowels (Vokale)
            Numified = RemoveVowelSounds.Replace(Numified, string.Empty);

            // Concatenate, pad and trim to ensure X### format.
            return string.Format("{0}{1}", Phrase[0], Numified).PadRight(4, '0').Substring(0, 4);
        }

        /// <summary>
        /// Numify whole phrase / string
        /// </summary>
        /// <param name="Phrase"></param>
        /// <returns></returns>
        private string Numify(string Phrase)
        {
            var charArray = Phrase.ToCharArray();
            char[] retVal = null;

            // Select = each element in array
            retVal = charArray.Select(Numify).ToArray();

            return new string(retVal);
        }

        /// <summary>
        /// Standard soundex character conversion - numify single char
        /// </summary>
        /// <param name="Character"></param>
        /// <returns></returns>
        private static char Numify(char Character)
        {
            switch (Character)
            {
                case 'B':
                case 'F':
                case 'P':
                case 'V':
                    return '1';
                case 'C':
                case 'G':
                case 'J':
                case 'K':
                case 'Q':
                case 'S':
                case 'X':
                case 'Z':
                    return '2';
                case 'D':
                case 'T':
                    return '3';
                case 'L':
                    return '4';
                case 'M':
                case 'N':
                    return '5';
                case 'R':
                    return '6';
                default:
                    return Character;
            }
        }

        /// <summary>
        /// Cologne phonetic for german language
        /// </summary>
        /// <param name="Character"></param>
        /// <param name="nextChar"></param>
        /// <returns></returns>
        private string GenerateGerman(string Phrase)
        {

            string Numified = "";
            char curLetter = ' ';
            char prevLetter = ' ';
            char nextLetter = ' ';

            // Remove non-alphas
            Phrase = Sanitiser.Replace((Phrase ?? string.Empty).ToUpper(), string.Empty);

            // Nothing to soundex, return empty
            if (string.IsNullOrEmpty(Phrase))
                return Empty;

            // Phrase = RemoveVowelSounds.Replace(Phrase, string.Empty);

            var curLength = Phrase.Length;

            for (int i = 0; i < curLength; i++)
            {
                curLetter = Phrase[i];

                if (i == 0 && i < (curLength - 1))
                {
                    nextLetter = Phrase[i + 1];
                }

                if (i > 0)
                {
                    prevLetter = Phrase[i - 1];
                }

                Numified = Numified + this.ConvertSoundexgerman(prevLetter, curLetter, nextLetter);
            }


            Console.WriteLine(Phrase);

            // Remove repeated numberics (characters of the same sound class), even if separated by H or W
            Numified = CollapseRepeatedNumbers.Replace(Numified, @"$1");

            Numified = NewMethod(Numified);

            if (Numified.Length > 0 && Numified[0] == Numify(Phrase[0]))
            {
                // Remove first numeric as first letter in same class as subsequent letters
                Numified = Numified.Substring(1);
            }

            // Remove vowels (Vokale)
            return Numified;
        }

        /// <summary>
        /// Remove Zeros except the first one
        /// </summary>
        /// <param name="Numified"></param>
        /// <returns></returns>
        private static string NewMethod(string Numified)
        {
            //Remove Zeros expect the first
            var backUp = Numified[0];
            Numified = Numified.Replace("0", String.Empty);
            if (backUp == '0')
            {
                Numified = "0" + Numified;
            }

            return Numified;
        }

        /// <summary>
        /// Convert a letter with cologne ponetics
        /// </summary>
        /// <param name="prevLetter"></param>
        /// <param name="curLetter"></param>
        /// <param name="nextLetter"></param>
        /// <returns></returns>
        private string ConvertSoundexgerman(char prevLetter, char curLetter, char nextLetter)
        {
            string retVal = String.Empty;

            switch (curLetter)
            {
                case ' ':
                    retVal = " ";
                    break;
                case 'A':
                case 'E':
                case 'I':
                case 'J':
                case 'O':
                case 'U':
                case 'Y':
                    retVal = "0";
                    break;
                case 'H':
                    retVal = "";
                    break;
                case 'B':
                    retVal = "1";
                    break;
                case 'F':
                    retVal = "3";
                    break;
                case 'P':
                    if (nextLetter != 'H')
                    {
                        retVal = "1";
                    }
                    else
                    {
                        retVal = "3";
                    }
                    break;
                case 'V':
                    retVal = "3";
                    break;
                case 'W':
                    retVal = "3";
                    break;
                case 'C':
                    // check Anlaut
                    if (prevLetter == ' ')
                    {
                        if (GermanCAnlaut.IsMatch(nextLetter.ToString()))
                        {
                            retVal = "4";
                        }
                        else
                        {
                            retVal = "8";
                        }
                    }

                    if (prevLetter != ' ')
                    {
                        if (GermanC.IsMatch(nextLetter.ToString()))
                        {
                            retVal = "4";
                        }
                        else
                        {
                            retVal = "8";
                        }
                    }
                    if (prevLetter == 'S' || prevLetter == 'Z')
                    {
                        retVal = "8";
                    }

                    break;
                case 'G':
                    retVal = "4";
                    break;
                case 'K':
                    retVal = "4";
                    break;
                case 'Q':
                    retVal = "4";
                    break;
                case 'S':
                    retVal = "8";
                    break;
                case 'X':
                    if (prevLetter == 'C' || prevLetter == 'K' || prevLetter == 'Q')
                    {
                        retVal = "8";
                    }
                    else
                    {
                        retVal = "48";
                    }
                    break;
                case 'Z':
                    retVal = "8";
                    break;
                case 'D':
                    if (prevLetter == 'C' || prevLetter == 'S' || prevLetter == 'Z')
                    {
                        retVal = "8";
                    }
                    else
                    {
                        retVal = "2";
                    }
                    break;
                case 'T':
                    if (prevLetter == 'C' || prevLetter == 'S' || prevLetter == 'Z')
                    {
                        retVal = "8";
                    }
                    else
                    {
                        retVal = "2";
                    }
                    break;
                case 'L':
                    retVal = "5";
                    break;
                case 'M':
                    retVal = "6";
                    break;
                case 'N':
                    retVal = "6";
                    break;
                case 'R':
                    retVal = "7";
                    break;
                default:
                    return curLetter.ToString();
            }

            return retVal;
        }
    }
}

