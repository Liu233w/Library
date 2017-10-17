using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Runtime.Session;
using Abp.TestBase;
using Library.Authorization.Roles;
using Library.Authorization.Users;
using Library.BookManage;
using Library.EntityFrameworkCore;
using Library.EntityFrameworkCore.Seed.Host;
using Library.EntityFrameworkCore.Seed.Tenants;
using Library.LibraryService;
using Library.MultiTenancy;
using Library.Users;
using Library.Users.Dto;
using Microsoft.EntityFrameworkCore;


namespace Library.Tests
{
    public abstract class LibraryTestBase : AbpIntegratedTestBase<LibraryTestModule>
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;

        protected LibraryTestBase()
        {
            void NormalizeDbContext(LibraryDbContext context)
            {
                context.EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
                context.EventBus = NullEventBus.Instance;
                context.SuppressAutoSetTenantId = true;
            }

            //Seed initial data for host
            AbpSession.TenantId = null;
            UsingDbContext(context =>
            {
                NormalizeDbContext(context);
                new InitialHostDbBuilder(context).Create();
                new DefaultTenantBuilder(context).Create();
            });

            //Seed initial data for default tenant
            AbpSession.TenantId = 1;
            UsingDbContext(context =>
            {
                NormalizeDbContext(context);
                new TenantRoleAndUserBuilder(context, 1).Create();
            });

            //Inject Service
            _userAppService = Resolve<IUserAppService>();
            _userManager = Resolve<UserManager>();

            LoginAsDefaultTenantAdmin();
        }

        #region UsingDbContext

        protected IDisposable UsingTenantId(int? tenantId)
        {
            var previousTenantId = AbpSession.TenantId;
            AbpSession.TenantId = tenantId;
            return new DisposeAction(() => AbpSession.TenantId = previousTenantId);
        }

        protected void UsingDbContext(Action<LibraryDbContext> action)
        {
            UsingDbContext(AbpSession.TenantId, action);
        }

        protected Task UsingDbContextAsync(Func<LibraryDbContext, Task> action)
        {
            return UsingDbContextAsync(AbpSession.TenantId, action);
        }

        protected T UsingDbContext<T>(Func<LibraryDbContext, T> func)
        {
            return UsingDbContext(AbpSession.TenantId, func);
        }

        protected Task<T> UsingDbContextAsync<T>(Func<LibraryDbContext, Task<T>> func)
        {
            return UsingDbContextAsync(AbpSession.TenantId, func);
        }

        protected void UsingDbContext(int? tenantId, Action<LibraryDbContext> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<LibraryDbContext>())
                {
                    action(context);
                    context.SaveChanges();
                }
            }
        }

        protected async Task UsingDbContextAsync(int? tenantId, Func<LibraryDbContext, Task> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<LibraryDbContext>())
                {
                    await action(context);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected T UsingDbContext<T>(int? tenantId, Func<LibraryDbContext, T> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<LibraryDbContext>())
                {
                    result = func(context);
                    context.SaveChanges();
                }
            }

            return result;
        }

        protected async Task<T> UsingDbContextAsync<T>(int? tenantId, Func<LibraryDbContext, Task<T>> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<LibraryDbContext>())
                {
                    result = await func(context);
                    await context.SaveChangesAsync();
                }
            }

            return result;
        }

        #endregion

        #region Login

        protected void LoginAsHostAdmin()
        {
            LoginAsHost(AbpUserBase.AdminUserName);
        }

        protected void LoginAsDefaultTenantAdmin()
        {
            LoginAsTenant(Tenant.DefaultTenantName, AbpUserBase.AdminUserName);
        }

        protected void LoginAsHost(string userName)
        {
            AbpSession.TenantId = null;

            var user =
                UsingDbContext(
                    context =>
                        context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for host.");
            }

            AbpSession.UserId = user.Id;
        }

        protected void LoginAsTenant(string tenancyName, string userName)
        {
            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant == null)
            {
                throw new Exception("There is no tenant: " + tenancyName);
            }

            AbpSession.TenantId = tenant.Id;

            var user =
                UsingDbContext(
                    context =>
                        context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
            }

            AbpSession.UserId = user.Id;
        }

        #endregion

        /// <summary>
        /// Gets current user if <see cref="IAbpSession.UserId"/> is not null.
        /// Throws exception if it's null.
        /// </summary>
        protected async Task<User> GetCurrentUserAsync()
        {
            var userId = AbpSession.GetUserId();
            return await UsingDbContext(context => context.Users.SingleAsync(u => u.Id == userId));
        }

        /// <summary>
        /// Gets current tenant if <see cref="IAbpSession.TenantId"/> is not null.
        /// Throws exception if there is no current tenant.
        /// </summary>
        protected async Task<Tenant> GetCurrentTenantAsync()
        {
            var tenantId = AbpSession.GetTenantId();
            return await UsingDbContext(context => context.Tenants.SingleAsync(t => t.Id == tenantId));
        }

        #region Mocks

        protected Book Book1 => new Book
        {
            Id = 1,
            Isbn = "1234567890",
            Author = "Author1",
            Description = "Book1's Description",
            Publish = "Publisher1",
            Title = "Book1"
        };

        protected Book Book2 => new Book
        {
            Id = 2,
            Isbn = "1234567892",
            Author = "Author2",
            Description = "Book2's Description",
            Publish = "Publisher2",
            Title = "Book2"
        };

        protected Copy Book1Copy1 => new Copy
        {
            Id = 1,
            BookId = 1
        };

        protected Copy Book1Copy2 => new Copy
        {
            Id = 2,
            BookId = 1
        };

        protected Copy Book2Copy1 => new Copy
        {
            Id = 3,
            BookId = 2
        };

        protected Copy Book2Copy2 => new Copy
        {
            Id = 4,
            BookId = 2
        };

        protected async Task InjectBooksDataAsync()
        {
            await UsingDbContextAsync(async ctx =>
            {
                await ctx.Books.AddAsync(Book1);
                await ctx.Books.AddAsync(Book2);

                await ctx.Copys.AddAsync(Book1Copy1);
                await ctx.Copys.AddAsync(Book1Copy2);
                await ctx.Copys.AddAsync(Book2Copy1);
                await ctx.Copys.AddAsync(Book2Copy2);
            });
        }

        protected async Task InjectTestUser()
        {
            await _userAppService.Create(
                new CreateUserDto
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = "john.nash"
                });
        }

        protected async Task MarkTestUserAsReader()
        {
            var john = await FindJohnAsync();

            await UsingDbContextAsync(async ctx =>
            {
                var role = await ctx.Roles.FirstAsync(
                    item => item.Name == StaticRoleNames.Tenants.Reader);
                ctx.UserRoles.Add(new UserRole
                {
                    UserId = john.Id,
                    RoleId = role.Id,
                    TenantId = AbpSession.TenantId
                });
            });
        }

        protected async Task<User> FindJohnAsync()
        {
            return await UsingDbContextAsync(async ctx =>
            {
                return await ctx.Users.FirstOrDefaultAsync(item => item.UserName == "john.nash");
            });
        }

        protected async Task<BorrowRecord> InjectBorrowRecord1AndGetAsync(int renewTime, bool outdated = false)
        {
            var now = DateTime.Now;
            var dateTime = outdated
                ? now - LibraryConsts.UserMaxBorrowDuration - new TimeSpan(1, 0, 0, 0)
                : now;

            return await UsingDbContextAsync(async ctx =>
            {
                var record = new BorrowRecord
                {
                    Id = 1,
                    CopyId = Book1Copy1.Id,
                    BorrowerUserId = (await FindJohnAsync()).Id,
                    CreationTime = dateTime,
                    CreatorUserId = 1,
                    RenewTime = renewTime
                };
                await ctx.BorrowRecords.AddAsync(record);

                var newBook1Copy1 = Book1Copy1;
                newBook1Copy1.BorrowRecordId = 1;

                ctx.Copys.Update(newBook1Copy1);

                return record;
            });
        }

        #endregion
    }
}
