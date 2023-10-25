using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.DataAccess.EntityFramework;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IUserService : IService<UserModel, User, BitirmeProjesiContext>
    {

    }

    public class UserService : IUserService
    {
        public RepoBase<User, BitirmeProjesiContext> Repo { get; set; } = new Repo<User, BitirmeProjesiContext>();

        public Result Add(UserModel model)
        {
            if (Repo.EntityExists(u => u.Username == model.Username))
                return new ErrorResult("A record with the entered username already exists!");
            if (Repo.EntityExists(u => u.UserDetail.Email.ToLower() == model.Email.ToLower().Trim()))
                return new ErrorResult("A record with the entered email already exists!");

            User entity = new User()
            {
                IsActive = model.IsActive,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Username = model.Username,
                RoleId = model.RoleId,
                Password = model.Password,
                UserDetail = new UserDetail()
                {
                    Address = model.Address,
                    Gender = model.Gender,
                    Email = model.Email,
                }
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<UserModel> Query()
        {
            return Repo.Query("Role").Select(e => new UserModel()
            {
                IsActive = e.IsActive,
                Id = e.Id,
                Firstname = e.Firstname,
                Lastname = e.Lastname,
                Username = e.Username,
                RoleId = e.RoleId,
                Password = e.Password,
                RoleNameDisplay = e.Role.Name
            });
        }

        public Result Update(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}