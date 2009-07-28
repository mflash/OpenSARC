namespace BusinessData.DataAccess {
    
    
    public class ErroMessages 
    {
        public static string GetErrorMessage(int number)
        {
            switch (number)
            {
                case 4060:
                    // Invalid Database
                    return "Banco de dados inexistente.";
                case 18456:
                    // Login Failed
                    return "Seu usuário não possui acesso ao banco de dados.";
                case 547:
                    // ForeignKey Violation
                    return "Impossível deletar dado pois está sendo utilizado em outro local.";            
                case 2627:
                    return "Impossível inserir. Código já existe.";
                case 2601:
                    // Unique Index/Constriant Violation
                    return "Impossível inserir. Código já existe.";
                default:
                    return "Ocorreu um erro no Servidor";
            }
        }
    }

}
