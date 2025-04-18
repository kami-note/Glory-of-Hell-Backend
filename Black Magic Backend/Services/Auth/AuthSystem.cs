﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using BlackMagicBackend.DataTypes;

namespace Black_Magic_Backend
{
    public class AuthSystem
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthSystem()
        {
            _dbContext = new ApplicationDbContext();
            _dbContext.Database.EnsureCreated();
        }

        public bool RegisterUser(string data)
        {
            try
            {
                JObject json = JObject.Parse(data);
                JToken? usernameToken = json["username"];
                JToken? passwordToken = json["password"];

                if (usernameToken == null || passwordToken == null)
                {
                    throw new Exception("Username or password missing from JSON");
                }

                string username = usernameToken.ToString();
                string password = passwordToken.ToString();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    PrettyConsole.LogWarning("Username or password cannot be empty!");
                    return false;
                }

                if (_dbContext.Users.Any(u => u.Username == username))
                {
                    PrettyConsole.LogWarning("Username already exists!");
                    return false;
                }

                var newUser = new Models.User();
                newUser.Username = username;
                newUser.SetPassword(password);

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                var newCharacter = new Models.Character();
                newCharacter.UserId = newUser.Id;
                newCharacter.User = newUser;
                newCharacter.Name = username;
                newCharacter.Position = new Vector3(0, 0, 0);

                _dbContext.Characters.Add(newCharacter);
                _dbContext.SaveChanges();

                PrettyConsole.LogInfo($"User {username} registered successfully!");
                PrettyConsole.LogInfo($"Character {newCharacter.Name} created successfully!");

                return true;
            }
            catch (Exception ex)
            {
                PrettyConsole.LogError($"Error during registration: {ex.Message}");
                return false;
            }
        }


        public bool AuthenticateUser(string data)
        {
            try
            {
                JObject json = JObject.Parse(data);
                JToken? usernameToken = json["username"];
                JToken? passwordToken = json["password"];

                if (usernameToken == null || passwordToken == null)
                {
                    throw new Exception("Username or password missing from JSON");
                }

                string username = usernameToken.ToString();
                string password = passwordToken.ToString();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    PrettyConsole.LogWarning("Username or password cannot be empty!");
                    return false;
                }

                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
                if (user == null || !user.ValidatePassword(password))
                {
                    PrettyConsole.LogWarning("Invalid username or password!");
                    return false;
                }

                PrettyConsole.LogInfo($"User {username} authenticated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                PrettyConsole.LogError($"Error during authentication: {ex.Message}");
                return false;
            }
        }
    }
}
