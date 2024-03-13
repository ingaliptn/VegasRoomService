using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WebUi.Lib
{
    public static class ProfileLib
    {
        public static string GetUrl(int number)
        {
            const string json = @"[{""EscortName"":""Cindy"",""EscortId"":""3""},
{ ""EscortName"":""Crily"",""EscortId"":""4""},
{ ""EscortName"":""Ella"",""EscortId"":""5""},
{ ""EscortName"":""Kumi"",""EscortId"":""6""},
{ ""EscortName"":""Suan"",""EscortId"":""7""},
{ ""EscortName"":""Camille"",""EscortId"":""49""},
{ ""EscortName"":""Chloe"",""EscortId"":""50""},
{ ""EscortName"":""Millisa"",""EscortId"":""51""},
{ ""EscortName"":""Natasa"",""EscortId"":""52""},
{ ""EscortName"":""Sich"",""EscortId"":""53""},
{ ""EscortName"":""Aneta"",""EscortId"":""13""},
{ ""EscortName"":""Angela"",""EscortId"":""14""},
{ ""EscortName"":""Anna"",""EscortId"":""15""},
{ ""EscortName"":""Caprice"",""EscortId"":""16""},
{ ""EscortName"":""Cookie"",""EscortId"":""17""},
{ ""EscortName"":""Elle"",""EscortId"":""18""},
{ ""EscortName"":""Elvira"",""EscortId"":""19""},
{ ""EscortName"":""Agnessa"",""EscortId"":""20""},
{ ""EscortName"":""Alisa"",""EscortId"":""21""},
{ ""EscortName"":""Katya"",""EscortId"":""22""},
{ ""EscortName"":""Nastya"",""EscortId"":""23""},
{ ""EscortName"":""Rita"",""EscortId"":""24""},
{ ""EscortName"":""Evanna"",""EscortId"":""25""},
{ ""EscortName"":""Felicia"",""EscortId"":""26""},
{ ""EscortName"":""Fiona"",""EscortId"":""27""},
{ ""EscortName"":""Flavia"",""EscortId"":""28""},
{ ""EscortName"":""Gia"",""EscortId"":""29""},
{ ""EscortName"":""Ivone"",""EscortId"":""30""},
{ ""EscortName"":""Jakelyn"",""EscortId"":""31""},
{ ""EscortName"":""Jolie"",""EscortId"":""32""},
{ ""EscortName"":""Laurette"",""EscortId"":""33""},
{ ""EscortName"":""Liliya"",""EscortId"":""34""},
{ ""EscortName"":""Maria"",""EscortId"":""35""},
{ ""EscortName"":""Missy"",""EscortId"":""36""},
{ ""EscortName"":""Nataly"",""EscortId"":""37""},
{ ""EscortName"":""Nella"",""EscortId"":""38""},
{ ""EscortName"":""Lisa"",""EscortId"":""39""},
{ ""EscortName"":""Mia"",""EscortId"":""40""},
{ ""EscortName"":""Alexa"",""EscortId"":""41""},
{ ""EscortName"":""Priscila"",""EscortId"":""42""},
{ ""EscortName"":""Rayne"",""EscortId"":""43""},
{ ""EscortName"":""Serena"",""EscortId"":""44""},
{ ""EscortName"":""Suzanne"",""EscortId"":""45""},
{ ""EscortName"":""Tasia"",""EscortId"":""54""},
{ ""EscortName"":""Vlada"",""EscortId"":""47""},
{ ""EscortName"":""Zoe"",""EscortId"":""48""}]";

            var model = JsonConvert.DeserializeObject<List<ProfileName>>(json);
            var name = model.Where(z => z.EscortId == number.ToString()).Select(z => z.EscortName).First().ToLower();
            return $"/escorts/{name}.php";
        }
    }

    public class ProfileName
    {
        public string EscortName { get; set; }
        public string EscortId { get; set; }
    }
}
