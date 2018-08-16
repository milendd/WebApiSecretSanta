using AutoMapper.QueryableExtensions;
using SantaSystem.Data.Repositories;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using System.Collections.Generic;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly IGenericRepository<User> userRepository;

        public ValuesController(IGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET api/values
        public IEnumerable<UserDTO> Get()
        {
            var users = this.userRepository.GetAll().ProjectTo<UserDTO>();
            return users;
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
