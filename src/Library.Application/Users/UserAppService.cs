using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Library.Authorization;
using Library.Authorization.Users;
using Library.Users.Dto;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.IdentityFramework;
using Abp.UI;
using Library.Authorization.Roles;
using Library.LibraryService;
using Library.Roles.Dto;

namespace Library.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<Role> _roleRepository;
        private readonly BookInfoManager _bookInfoManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            IPasswordHasher<User> passwordHasher,
            IRepository<Role> roleRepository,
            RoleManager roleManager, 
            BookInfoManager bookInfoManager)
            : base(repository)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _bookInfoManager = bookInfoManager;
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;

            CheckErrors(await _userManager.CreateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var notReturnBook = await _bookInfoManager.UserIsBorrowingBookAsync(input.Id);
            if (notReturnBook)
            {
                throw new UserFriendlyException("The user has books which not returned");
            }

            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task MarkUserAsLibraryManagerByUserNameOrEmail(UserNameOrEmailInput input)
        {
            var user = await _userManager.FindByNameOrEmailAsync(
                input.UserName.IsNullOrEmpty() ? input.Email : input.UserName);

            await _userManager.AddToRoleAsync(user, StaticRoleNames.Tenants.LibraryManager);
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            return await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}