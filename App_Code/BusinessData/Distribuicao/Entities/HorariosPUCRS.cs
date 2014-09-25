using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{
    public class Horarios
    {
        public enum HorariosPUCRS
        {
            AB,
            CD,
            EE,
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
                case "EE": return HorariosPUCRS.EE;
                case "FG": return HorariosPUCRS.FG;
                case "HI": return HorariosPUCRS.HI;
                case "JK": return HorariosPUCRS.JK;
                case "LM": return HorariosPUCRS.LM;
                case "NP": return HorariosPUCRS.NP;
            }
            throw new Exception("String em formato invalido");
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
                    data = data.AddMinutes(50);
                    return data;
                case "EE":
                    data = data.AddHours(11);
                    data = data.AddMinutes(35);
                    return data;
                case "FG":
                    data = data.AddHours(14);
                    return data;
                case "HI":
                    data = data.AddHours(15);
                    data = data.AddMinutes(50);
                    return data;
                case "JK":
                    data = data.AddHours(17);
                    data = data.AddMinutes(35);
                    return data;
                case "LM":
                    data = data.AddHours(19);
                    data = data.AddMinutes(30);
                    return data;
                case "NP":
                    data = data.AddHours(21);
                    data = data.AddMinutes(15);
                    return data;
            }
            throw new Exception("String em formato invalido");
        }
    }
}