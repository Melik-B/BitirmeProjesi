using AppCore.Business.Models.Results;
using Business.Enums;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IAccountService
    {
        Result<UserModel> Login(UserLoginModel model);
        Result Register(UserRegistrationModel model);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result<UserModel> Login(UserLoginModel model)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.IsActive);
            if (user == null)
                return new ErrorResult<UserModel>("Invalid username and password!");

            return new SuccessResult<UserModel>(user);
        }

        public Result Register(UserRegistrationModel model)
        {
            UserModel user = new UserModel()
            {
                IsActive = true,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Username = model.Username,
                RoleId = (int)Roles.User,
                Password = model.Password,
                Address = model.Address.Trim(),
                Email = model.Email,
                Gender = model.Gender
            };

            return _userService.Add(user);
        }
    }
}
