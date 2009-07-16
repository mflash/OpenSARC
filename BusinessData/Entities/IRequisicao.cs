using System;
namespace BusinessData.Entities
{
    public interface IRequisicao
    {
        Aula Aula { get; set; }
        CategoriaRecurso CategoriaRecurso { get; set; }
        Guid IdRequisicao { get; }
        int Prioridade { get; set; }
    }
}
