using LojaEcomerce.Interfaces;
using LojaEcomerce.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using BCrypt.Net;

namespace LojaEcomerce.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //variavel privada e somente leitura para
        //receber a conexão do banco de dados
        private readonly string _connectionString;

        //construtor para inicializar a conexão do banco
        public UsuarioRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Conexao");
        }
        //metodo para validar o login
        public LoginViewModel Validar(string email, string senha)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM tb_Usuario WHERE Email= @email";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@email", email);
      

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string senhaBanco = reader["Senha"].ToString()!;

                if (BCrypt.Net.BCrypt.Verify(senha,senhaBanco))
                {
                    return new LoginViewModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString()!,
                        Email = reader["Email"].ToString()!,
                        Nivel = reader["Nivel"].ToString()!
                    };
                }
                
            }
            return null;

        }

        public void CriarConta(LoginViewModel usuario)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                //Criptografando a senha antes de enviar para o MySql (banco)
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

                    var sql = "INSERT INTO tb_Usuario(Nome,Email,Senha,Nivel) VALUES (@nome,@email,@senha,@nivel)";
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@senha", senhaHash);
                cmd.Parameters.AddWithValue("@nivel", "Usuario");
                cmd.ExecuteNonQuery();

            }

        }



    }
}
