using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Converter
{
    public static class SenatsConverter
    {
        public static int Convert(string senat, int art) //ID: Art = 1; Sortierung: Art = 2
        {
            int output;
            switch (senat)
            {
                case "I. Zivilsenat":
                    {
				        output = art == 1 ? 1 : 1; //erste Zahl gibt die SenatsId an, die zweite Zahl gibt die Sortierungsreihenfolge an; diese beginnt pro Bereich bei 1
                        return output;
                    }
                case "II. Zivilsenat":
                    {
				        output = art == 1 ? 2 : 2;
                        return output;
                    }
                case "III. Zivilsenat":
                    {
					    output = art == 1 ? 3 : 3;
					    return output;
				    }
				case "IV. Zivilsenat":
					{
						output = art == 1 ? 4 : 4;
						return output;
					}
				case "V. Zivilsenat":
					{
						output = art == 1 ? 5 : 5;
						return output;
					}
				case "VI. Zivilsenat":
					{
						output = art == 1 ? 6 : 6;
						return output;
					}
				case "VII. Zivilsenat":
					{
						output = art == 1 ? 7 : 8;
						return output;
					}
				case "VIII. Zivilsenat":
					{
						output = art == 1 ? 8 : 8;
						return output;
					}

				case "IX. Zivilsenat":
					{
						output = art == 1 ? 9 : 10;
						return output;
					}
				case "X. Zivilsenat":
					{
						output = art == 1 ? 10 : 11;
						return output;
					}
				case "XI. Zivilsenat":
					{
						output = art == 1 ? 11 : 12;
						return output;
					}
				case "XII. Zivilsenat":
					{
						output = art == 1 ? 12 : 13;
						return output;
					}
				case "XIII. Zivilsenat":
					{
						output = art == 1 ? 13 : 14;
						return output;
					}
				case "VIa. Zivilsenat":
					{
						output = art == 1 ? 14 : 7;
						return output;
					}
				case "1. Strafsenat":
					{
						output = art == 1 ? 15 : 1;
						return output;
					}
				case "2. Strafsenat":
					{
						output = art == 1 ? 16 : 2;
						return output;
					}
				case "3. Strafsenat":
					{
						output = art == 1 ? 17 : 3;
						return output;
					}
				case "4. Strafsenat":
					{
						output = art == 1 ? 18 : 4;
						return output;
					}
				case "5. Strafsenat":
					{
						output = art == 1 ? 19 : 5;
						return output;
					}
				case "6. Strafsenat":
					{
						output = art == 1 ? 20 : 6;
						return output;
					}
				case "Anwaltssenat":
					{
						output = art == 1 ? 21 : 1;
						return output;
					}
				case "Notarsenat":
					{
						output = art == 1 ? 22 : 2;
						return output;
					}
				case "Landwirtschaftssenat":
					{
						output = art == 1 ? 23 : 3;
						return output;
					}
				case "Steuerberatersenat":
					{
						output = art == 1 ? 24 : 4;
						return output;
					}
				case "Sonstiges":
					{
						output = art == 1 ? 24 : 5;
						return output;
					}


				default: return 0;
            }
        }


        //public static string ConvertInt(int senat)
        //{
        //    switch (senat)
        //    {
        //        case 1:
        //            {
        //                return "Zivilsenat1";
        //            }
        //        case 2:
        //            {
        //                return "Zivilsenat2";
        //            }
        //        case 3:
        //            {
        //                return "Zivilsenat3";
        //            }
        //        case 4:
        //            {
        //                return "Zivilsenat4";
        //            }
        //        case 5:
        //            {
        //                return "Zivilsenat5";
        //            }
        //        case 6:
        //            {
        //                return "Zivilsenat6";
        //            }
        //        case 7:
        //            {
        //                return "Zivilsenat7";
        //            }
        //        case 8:
        //            {
        //                return "Zivilsenat8";
        //            }
        //        case 9:
        //            {
        //                return "Zivilsenat9";
        //            }
        //        case 10:
        //            {
        //                return "Zivilsenat10";
        //            }
        //        case 11:
        //            {
        //                return "Zivilsenat11";
        //            }
        //        case 12:
        //            {
        //                return "Zivilsenat12";
        //            }
        //        case 13:
        //            {
        //                return "Zivilsenat13";
        //            }
        //        case 14:
        //            {
        //                return "Zivilsenat6a";
        //            }
        //        default: return "";

        //    }
        //}
    }
}
