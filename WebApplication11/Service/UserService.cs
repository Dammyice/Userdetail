using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication11.Helpers;
using WebApplication11.Models;

namespace WebApplication11.Service
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        bool Adduser(UserProfile model);
        UserProfile GetUser(int id);
    }
    public class UserService : IUserService
    {
        protected readonly UserDBContext Context;
        private readonly AppSettings _appSettings;



        public UserService(IOptions<AppSettings> appSettings, UserDBContext context)
        {
            _appSettings = appSettings.Value;
            Context = context;

        }

        public User Authenticate(string email, string password)
        {
            var user = Context.Set<UserProfile>().Where(x => x.Email == email && x.Password == password).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;

            var resp = new User();
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            resp.Token = tokenHandler.WriteToken(token);
            resp.FirstName = user.FirstName;
            resp.LastName = user.LastName;
            resp.Email = user.Email;

            return resp ;
        }

        public bool Adduser(UserProfile model)
        {
            try
            {
                var user = Context.Set<UserProfile>().Where(x => x.Email == model.Email).FirstOrDefault();

                if(user != null)
                {
                    return false;
                }

                Context.Add(model);
                Context.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                throw;
            }           
        }

        public UserProfile GetUser(int id)
        {
            try
            {
                var user = Context.Set<UserProfile>().Find(id);

                return user;
            }
            catch (Exception)
            {

                throw;
            }
            
        }


    }
}

