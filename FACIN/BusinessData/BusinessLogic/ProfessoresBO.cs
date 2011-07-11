using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Net.Mail;
using BusinessData.Entities;
using BusinessData.DataAccess;
using System.Configuration;
using System.IO;

//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;


namespace BusinessData.BusinessLogic
{
    public class ProfessoresBO : PessoaBaseBO
    {
        private DataAccess.ProfessorDAO dao;
        private Usuario usr;

        public ProfessoresBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.ProfessorDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }

            usr = new Usuario();
        }

        public override void DeletePessoa(PessoaBase professor)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Membership.DeleteUser(professor.Matricula);
                    dao.DeletePofessor(professor.Id);

                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Professor: " + professor.Nome + "; Id: " + professor.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Delete Professor";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (ArgumentNullException ex)
                {
                   
                    throw new DataAccess.DataAccessException("Informe o professor a ser deletado.", ex);
                }

                catch (ArgumentException ex)
                {
                    
                    throw new DataAccess.DataAccessException("Erro ao deletar professor", ex);
                }
          
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
            }
                else
                    {
                        throw new SecurityException("Acesso Negado.");
                     }
        }

        public override void InsertPessoa(PessoaBase professor, string senha, string perguntaSecreta, string respostaSecreta)
        {
        }

        public override void InsertPessoa(PessoaBase professor,string perguntaSecreta,string respostaSecreta)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    string senha = Membership.GeneratePassword(8, 3);
                    MembershipCreateStatus status;
                    Membership.CreateUser(
                        professor.Matricula,     //insere a matricula no lugar do nome da API
                        senha,                  //para que esta fique como login
                        professor.Email,
                        perguntaSecreta,
                        respostaSecreta,
                        true,
                        professor.Id,
                        out status
                        );
                    
                    if (status != MembershipCreateStatus.Success)
                    {
                        throw new DataAccessException("Usuário já cadastrado.");
                    }

                    Roles.AddUserToRole(professor.Matricula, "Professor");
                    dao.InsertProfessor((Professor)professor);
                    
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Professor: " + professor.Nome + "; Id: " + professor.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Insert Professor";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);

                    MembershipUser muser = Membership.GetUser(professor.Id);
                    try
                    {
                        this.SendNewPessoa(muser, senha);
                    }
                    catch (Exception )
                    {
                        string logs = ConfigurationManager.AppSettings["PahtLog"] + "\\" + String.Format("{0:dd-mm-yyyy}.log", DateTime.Now);
                        try
                        {
                            StreamWriter sr = new StreamWriter(logs, true);
                            sr.WriteLine(muser.Email + " " + senha);
                            sr.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                catch (DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public override PessoaBase GetPessoaById(Guid id)
        {
            try
            {
                return dao.GetProfessor(id);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Professor> GetProfessores()
        {
            try
            {
                return dao.GetProfessores();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public void UpdateEmail(Professor prof, string email)
        {
            Regex validaEmail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            if (!validaEmail.IsMatch(email))
            {
                throw new ArgumentException("Email inválido");
            }
            MembershipUser professor = Membership.GetUser(prof.Id);
            professor.Email = email;
            Membership.UpdateUser(professor);

            //MembershipUser user = Membership.GetUser();
            //LogEntry log = new LogEntry();
            //log.Message = "Professor: " + prof.Nome + "; Id: " + prof.Id + "; Administrador: " + user.UserName;
            //log.TimeStamp = DateTime.Now;
            //log.Severity = TraceEventType.Information;
            //log.Title = "Update Email Professor";
            //log.MachineName = Dns.GetHostName();
            //Logger.Write(log);
        }

        public List<PessoaBase> GetProfessoresESecretarios()
        {
            SecretarioDAO sBO = new SecretarioDAO();
            ProfessorDAO pBO = new ProfessorDAO();
            SecretariosBO a = new SecretariosBO();

            List<Professor> lista1 = pBO.GetProfessores();
            List<Secretario> lista2 = sBO.GetSecretarios();
            List<PessoaBase> pessoas = new List<PessoaBase>();
            foreach (Professor p in lista1)
            {
                pessoas.Add(p);
            }

            foreach (Secretario s in lista2)
            {
                pessoas.Add(s);
            }

            return pessoas;
        }

        public void PersonificarProfessor(string matricula)
        {
            string usuario = Membership.GetUser().UserName;

            FormsAuthentication.SetAuthCookie(matricula, false);
            //LogEntry log = new LogEntry();
            //log.Message = usuario + " foi personificou o professor c/ matricula " + matricula;
            //log.TimeStamp = DateTime.Now;
            //log.Severity = TraceEventType.Information;
            //log.Title = "Personificação";
            //log.MachineName = Dns.GetHostName();
            //Logger.Write(log);
        }
    }    
  }

