using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{
    public class Horarios
    {
        public static string[] horariosPUCRS = { "AB", "CD", "EX", "FG", "HI", "JK", "LM", "NP" };

        public enum HorariosPUCRS
        {
            AB,
            CD,
            EX,
            FG,
            HI,
            JK,
            LM,
            NP,
        };
        public static HorariosPUCRS Parse(string s)
        {
            string aux = s.ToUpper();
            switch (aux)
            {
                case "AB": return HorariosPUCRS.AB;
                case "CD": return HorariosPUCRS.CD;
                case "EX": return HorariosPUCRS.EX;
                case "FG": return HorariosPUCRS.FG;
                case "HI": return HorariosPUCRS.HI;
                case "JK": return HorariosPUCRS.JK;
                case "LM": return HorariosPUCRS.LM;
                case "NP": return HorariosPUCRS.NP;
            }
            throw new Exception("String em formato invalido: "+ aux);
        }
        public static DateTime ParseToDateTime(string s)
        {
            string aux = s.ToUpper();
            DateTime data = new DateTime();
            switch (aux)
            {
                case "AB":
                    data = data.AddHours(8);
                    return data;
                case "CD":
                    data = data.AddHours(9);
                    data = data.AddMinutes(45);
                    return data;
                case "EX":
                    data = data.AddHours(11);
                    data = data.AddMinutes(30);
                    return data;
                case "FG":
                    data = data.AddHours(14);
                    return data;
                case "HI":
                    data = data.AddHours(15);
                    data = data.AddMinutes(45);
                    return data;
                case "JK":
                    data = data.AddHours(17);
                    data = data.AddMinutes(30);
                    return data;
                case "LM":
                    data = data.AddHours(19);
                    data = data.AddMinutes(15);
                    return data;
                case "NP":
                    data = data.AddHours(21);
                    data = data.AddMinutes(0);
                    return data;
            }
            throw new Exception("String em formato invalido:" + aux);
        }
    }
}