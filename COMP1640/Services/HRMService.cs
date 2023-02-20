﻿#nullable disable

using COMP1640.ViewModels.HRM.Requests;
using COMP1640.ViewModels.HRM.Responses;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace COMP1640.Services
{
    public class HRMService
    {
        private readonly IUserRepository _userRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUnitOfWork _unitOfWork;

        public HRMService(IUserRepository userRepo
            , IUnitOfWork unitOfWork
            , IDepartmentRepository departmentRepo
            , IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _departmentRepo = departmentRepo;
            _roleRepo = roleRepo;
        }

        public async Task<List<UserBasicInfoResponse>> GetListUserAsync(GetListUserRequest request)
        {
            return await _userRepo
                .GetQuery(request.Filter())
                .Select(new UserBasicInfoResponse().GetSelection())
                .ToListAsync();
        }

        public async Task<UserBasicInfoResponse> GetUserInfoDetailsAsync(int userId)
        {
            return await _userRepo
                .GetById(userId)
                .Select(new UserBasicInfoResponse().GetSelection())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateUserAsync(CreateUserRequest request)
        {
            var existedEmail = await _userRepo.AnyAsync(_ => _.Email == request.Email);
            if (existedEmail)
                return false;

            var role = await _roleRepo.GetAsync(request.Role);
            if (role == null)
                return false;

            var department = await _departmentRepo.GetAsync(request.DepartmentId);
            if (department == null)
                return false;

            var user = new User(request.Name
                , request.Email
                , request.Birthday
                , request.Gender
                , role
                , department);

            await _userRepo.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public Task EditUserInfoAsync(EditUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetById(id).FirstOrDefaultAsync();
            if (user == null)
                return false;

            await _userRepo.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<SelectPropertyForCreateAccountResponse> GetRolesForCreateAccountAsync()
        {
            var roles = await _roleRepo
                .GetQuery(r => r.Id != (int)RoleTypeEnum.Admin)
                .Select(_ => new DropDownListBaseResponse()
                {
                    Id = _.Id,
                    Name = _.Name
                })
                .AsNoTracking()
                .ToListAsync();

            var departments = await _departmentRepo.GetAll()
                .Select(d => new DropDownListBaseResponse()
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .AsNoTracking()
                .ToListAsync();

            return new SelectPropertyForCreateAccountResponse()
            {
                Roles = roles,
                Departments = departments
            };
        }
    }
}
