using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using Data.Contracts;
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
        public async Task<IEnumerable<User>> Get(CancellationToken cancellationToken)
        {
            var users= await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;
        }

        // GET: api/User/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task Create([FromBody] User user, CancellationToken cancellationToken)
        {
            await userRepository.AddAsync(user, cancellationToken);
        }

        // PUT: api/User/5
        [HttpPut]
        //اینجا اگه یوزر رو مستقیم به متد آپدیت پاس میدادیم آپدیتی انجام نمیشد
        //چون یوزر توسط dbContext ترک نشده
        public async Task<IActionResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.UserName = user.UserName;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
    }
}
