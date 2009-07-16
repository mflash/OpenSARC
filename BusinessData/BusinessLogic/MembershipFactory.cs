using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using BusinessData.DataAccess;

namespace BusinessData.BusinessLogic
{
    public class MembershipFactory
    {
        private static MembershipFactory fabricaMembership = null;
        ProfessoresBO professorBO;
        SecretariosBO secretarioBO;

        public MembershipFactory()
        {
            professorBO = new ProfessoresBO();
            secretarioBO = new SecretariosBO();
        }

        public static MembershipFactory GetInstance()
        {
            if (fabricaMembership == null) fabricaMembership = new MembershipFactory();
            return fabricaMembership;
        }

        public PessoaBaseBO CreatePessoaBase(Object entidade)
        {
            Professor p = null;

            if (entidade.GetType().IsInstanceOfType(p))
            {
                return professorBO;
            }

            return secretarioBO;
        }
    }
}
