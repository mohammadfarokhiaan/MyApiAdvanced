using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            var users= await userRepository.TableNoTracking.ToListAsync();
            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await userRepository.GetByIdAsync(CancellationToken.None, id);
            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task Create([FromBody] User user)
        {
            await userRepository.AddAsync(user, CancellationToken.None);
        }
    }
}
