﻿using System;
using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Library.Authorization;
using Library.Authorization.Roles;
using Library.Authorization.Users;
using Library.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Library.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly LibraryDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(LibraryDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();

                //Grant all permissions to admin role
                var permissions = PermissionFinder
                    .GetAllPermissions(new LibraryAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();

                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        });
                }

                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                // Subscribe Notifications
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(
                    Guid.NewGuid(), _tenantId, adminUser.Id, NotificationType.BroadcastNotification));
            }

            // Library roles

            var libraryManagerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.LibraryManager);
            if (libraryManagerRole == null)
            {
                libraryManagerRole = _context.Roles.Add(
                    new Role(_tenantId, StaticRoleNames.Tenants.LibraryManager, StaticRoleNames.Tenants.LibraryManager)
                    {
                        IsStatic = true,
                        IsDefault = false
                    }).Entity;

                _context.SaveChanges();

                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = PermissionNames.Pages_BookManage,
                        IsGranted = true,
                        RoleId = libraryManagerRole.Id
                    });

                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = PermissionNames.Pages_LibraryManage,
                        IsGranted = true,
                        RoleId = libraryManagerRole.Id
                    });
            }

            var readerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Reader);
            if (readerRole == null)
            {
                readerRole = _context.Roles.Add(
                    new Role(_tenantId, StaticRoleNames.Tenants.Reader, StaticRoleNames.Tenants.Reader)
                    {
                        IsStatic = true,
                        IsDefault = false
                    }).Entity;

                _context.SaveChanges();

                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = PermissionNames.Pages_Library,
                        IsGranted = true,
                        RoleId = readerRole.Id
                    });
            }

        }
    }
}
