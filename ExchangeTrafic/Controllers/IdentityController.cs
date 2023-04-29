using ExchangeTrafic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeTrafic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly RatesContext _ratesContext;

        public IdentityController(RatesContext ratesContext)
        {
            _ratesContext = ratesContext;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            var dbuser = new User();
            dbuser.FirstName = user.FirstName;
            dbuser.LastName = user.LastName;
            dbuser.Age = user.Age;
            dbuser.Email = user.Email;
            dbuser.Password = user.Password;
            dbuser.PhoneNumber = user.PhoneNumber;
            _ratesContext.Users.Add(dbuser);
            _ratesContext.SaveChanges();
            var users = _ratesContext.Users.ToList();
            return Ok(users);
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> ValidateUser(string email, string password)
        {
            var userlist = _ratesContext.Users.ToList();
            var validemail = userlist.Where(x=>x.Email == email && x.Password==password);
            if (validemail.IsNullOrEmpty())
                return NotFound("You are not registered");
            return Ok("I found you");
        }
    }
}
