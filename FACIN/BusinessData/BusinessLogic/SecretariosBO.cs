using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess;
using BusinessData.Entities;
using System.Web.Security;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Security;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;

namespace BusinessData.BusinessLogic
{
    public class SecretariosBO : PessoaBaseBO
    {
        private SecretarioDAO dao;
        private Usuario usr;

        public SecretariosBO()
        {
            try
            {
                dao = new SecretarioDAO();
            }
            catch (DataAccessException )
            {
                throw;
            }

            usr = new Usuario();
        }

        public override void DeletePessoa(PessoaBase secretario)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.DeleteSecretario(secretario.Id);
                    Membership.DeleteUser(secretario.Matricula);
                    
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Secretario: " + secretario.Nome + "; Id: " + secretario.Id+"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Secretario";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
                }
                catch (ArgumentNullException ex)
                {
                    throw new DataAccessException("Informe o secretário a ser deletado.", ex);
                }

                catch (ArgumentException ex)
                {
                    throw new DataAccessException("Erro ao deletar secretário", ex);
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

        public override void InsertPessoa(PessoaBase secretario, string senha, string perguntaSecreta, string respostaSecreta)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    MembershipCreateStatus status;
                    Membership.CreateUser(
                        secretario.Matricula,     //insere a matricula no lugar do nome da API
                        senha,                  //para que esta fique como login
                        secretario.Email,
                        perguntaSecreta,
                        respostaSecreta,
                        true,
                        secretario.Id,
                        out status
                        );

                    if (status != MembershipCreateStatus.Success)
                    {
                        throw new DataAccessException(status.ToString());
                    }

                    Roles.AddUserToRole(secretario.Matricula, "Secretario");
                    dao.InsertSecretario((Secretario)secretario);
                    //LOG
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Secretario: " + secretario.Nome + "; Id: " + secretario.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Secretario";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);

                    MembershipUser muser = Membership.GetUser(secretario.Id);
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

        public override void InsertPessoa(PessoaBase secretario,string perguntaSecreta,string respostaSecreta)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    string senha = Membership.GeneratePassword(8, 3);
                    MembershipCreateStatus status;
                    Membership.CreateUser(
                        secretario.Matricula,     //insere a matricula no lugar do nome da API
                        senha,                  //para que esta fique como login
                        secretario.Email,
                        perguntaSecreta,
                        respostaSecreta,
                        true,
                        secretario.Id,
                        out status
                        );
                    
                    if (status != MembershipCreateStatus.Success)
                    {
                        throw new DataAccessException(status.ToString());
                    }

                    Roles.AddUserToRole(secretario.Matricula, "Secretario");
                    dao.InsertSecretario((Secretario)secretario);

                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Secretario: " + secretario.Nome + "; Id: " + secretario.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Secretario";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);

                    MembershipUser muser = Membership.GetUser(secretario.Id);
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
                return dao.GetSecretario(id);
            }

            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Secretario> GetSecretarios()
        {
            try
            {
                return dao.GetSecretarios();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
    }    
  }