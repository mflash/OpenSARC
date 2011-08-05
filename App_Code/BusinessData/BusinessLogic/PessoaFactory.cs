using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using BusinessData.DataAccess;

namespace BusinessData.BusinessLogic
{
    public class PessoaFactory
    {
        private static PessoaFactory fabricaPessoas = null;
        ProfessoresBO professorBO;
        SecretariosBO secretarioBO;

        private PessoaFactory()
        {
            professorBO = new ProfessoresBO();
            secretarioBO = new SecretariosBO();
        }

        public static PessoaFactory GetInstance()
        {
            if (fabricaPessoas == null) 
                fabricaPessoas = new PessoaFactory();
            return fabricaPessoas;
        }

        public PessoaBase CreatePessoa(Guid id)
        {
            if (id != null)
            {
                PessoaBase pessoa = null;

                pessoa = (Professor)professorBO.GetPessoaById(id);

                if (pessoa != null)
                {
                    return pessoa;
                }
                pessoa = (Secretario)secretarioBO.GetPessoaById(id);

                return pessoa;
            }
            else return null;
        }
    }
}
