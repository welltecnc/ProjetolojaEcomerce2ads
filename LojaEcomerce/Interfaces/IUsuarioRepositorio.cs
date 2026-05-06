using LojaEcomerce.Models;
using System.Runtime.CompilerServices;

namespace LojaEcomerce.Interfaces
{
    public interface IUsuarioRepositorio
    {
        //A INTERFACE NÃO CONTÉM CONTEM CÓDIGO APENAS A PROMESSA DO QUE DEVE
        // SER FEITO ( COMO UM CONTRATO)
        LoginViewModel Validar(string email, string senha);

        //Contrato para salvar um novo usuario no banco
        void CriarConta(LoginViewModel usuario);
    }
 


}
