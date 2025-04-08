using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Black_Magic_Backend {
    public class AuthSystem {
        private readonly DatabaseContext _dbContext;

        public AuthSystem() {
            _dbContext = new DatabaseContext();
            _dbContext.Database.EnsureCreated();
        }

        public bool RegisterUser(string data) {
            JObject json = JObject.Parse(data);
            string username = json["username"].ToString();
            string password = json["password"].ToString();

            if (_dbContext.Users.Any(u => u.Username == username)) {
                Console.WriteLine("Usuário já existe!");
                return false;
            }

            var newUser = new Models.User();
            newUser.Username = username;
            newUser.SetPassword(password);

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            Console.WriteLine("Usuário cadastrado com sucesso!");
            return true;
        }

        public bool AuthenticateUser(string data) {
            JObject json = JObject.Parse(data);
            string username = json["username"].ToString();
            string password = json["password"].ToString();

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || !user.ValidatePassword(password)) {
                Console.WriteLine("Usuário ou senha incorretos!");
                return false;
            }

            Console.WriteLine("Autenticação bem-sucedida!");
            return true;
        }
    }
}
