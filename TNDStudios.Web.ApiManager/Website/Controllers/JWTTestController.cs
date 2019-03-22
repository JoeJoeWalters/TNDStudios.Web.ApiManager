using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Website.Controllers
{
    [ApiController]
    [Route("/api/jwttest")]
    public class JWTTestController : ControllerBase
    {
        [HttpGet]
        public String[] Get()
        {
            String key = "qwertyuiopasdfghjklzxcvbnm123456";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Finally create a Token
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload("TNDStudios", "Audience", new List<Claim>() { new Claim("Roles", "Admin"), new Claim("Roles", "Finance") }, DateTime.Now, DateTime.Now  );
            //
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);
            
            // And finally when  you received token from client
            // you can  either validate it or try to  read
            var token = handler.ReadJwtToken(tokenString);

            String decoded = token.Payload.SerializeToJson();

            return new String[] { tokenString, decoded};
        }
    }
}
