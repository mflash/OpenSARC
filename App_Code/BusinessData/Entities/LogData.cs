using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public class LogData
    {
        public LogData(DateTime horario, string acao, string usuario, string recurso, string unidadeCurso, string tipoUsuario, string matricula)
        {
            this.Horario = horario;
            this.Acao = acao;
            this.Usuario = usuario;
            this.Recurso = recurso;
            this.UnidadeCurso = unidadeCurso;
            this.TipoUsuario = tipoUsuario;
            this.Matricula = matricula;
        }

        public DateTime Horario { get; set; }
        public string Acao { get; set; }
        public string Usuario { get; set;}
        public string Recurso { get; set;}
        public string UnidadeCurso { get; set; }
        public string TipoUsuario { get; set;}
        public string Matricula { get; set;  }
    }
}