using System;
namespace BusinessData.Entities
{
    public interface ICalendario
    {
        int Ano { get; set; }
        System.Collections.Generic.List<Data> Datas { get; }
        DateTime FimG2 { get; set; }
        Guid Id { get; set; }
        DateTime InicioG1 { get; set; }
        DateTime InicioG2 { get; set; }
        int Semestre { get; set; }
        string ToString();
    }
}
