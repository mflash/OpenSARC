using System;
namespace BusinessData.Entities
{
    public interface ICategoria
    {
        string Descricao { get; set; }
        Guid Id { get; set; }
        string ToString();
    }
}
