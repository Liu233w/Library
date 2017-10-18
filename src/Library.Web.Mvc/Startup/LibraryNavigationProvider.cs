using Abp.Application.Navigation;
using Abp.Localization;
using Library.Authorization;

namespace Library.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class LibraryNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "home"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Tenants,
                        L("Tenants"),
                        url: "Tenants",
                        icon: "business",
                        requiredPermissionName: PermissionNames.Pages_Tenants
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "people",
                        requiredPermissionName: PermissionNames.Pages_Users
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "local_offer",
                        requiredPermissionName: PermissionNames.Pages_Roles
                    )
                 ).AddItem(
                    new MenuItemDefinition(
                        PageNames.AnnounceMessage,
                        L("AnnounceMessage"),
                        url: "AnnounceMessage",
                        icon: "send",
                        requiredPermissionName: PermissionNames.Pages_LibraryManage
                    )
                 ).AddItem(
                    new MenuItemDefinition(
                        PageNames.GetMessage,
                        L("GetMessage"),
                        url: "GetMessage",
                        icon: "message",
                        requiredPermissionName: PermissionNames.Pages_Library
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.BookManage,
                        L("BookManage"),
                        url: "BookManage",
                        icon: "description",
                        requiredPermissionName: PermissionNames.Pages_BookManage
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.SearchBooks,
                        L("SearchBooks"),
                        url: "SearchBooks",
                        icon: "view_list"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.BorrowBook,
                        L("BorrowBook"),
                        url: "BorrowBook",
                        icon: "pan_tool",
                        requiredPermissionName: PermissionNames.Pages_LibraryManage
                        )
                 ).AddItem(
                    new MenuItemDefinition(
                        PageNames.ReturnBook,
                        L("ReturnBook"),
                        url: "ReturnBook",
                        icon: "assignment_return",
                        requiredPermissionName: PermissionNames.Pages_LibraryManage
                        )
#if DEBUG
               ).AddItem(
                    new MenuItemDefinition(
                        PageNames.About,
                        L("About"),
                        url: "About",
                        icon: "info"
                    )
               ).AddItem( //Menu items below is just for demonstration!
                    new MenuItemDefinition(
                        "MultiLevelMenu",
                        L("MultiLevelMenu"),
                        icon: "menu"
                    ).AddItem(
                        new MenuItemDefinition(
                            "AspNetBoilerplate",
                            new FixedLocalizableString("ASP.NET Boilerplate")
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateHome",
                                new FixedLocalizableString("Home"),
                                url: "https://aspnetboilerplate.com?ref=abptmpl"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateTemplates",
                                new FixedLocalizableString("Templates"),
                                url: "https://aspnetboilerplate.com/Templates?ref=abptmpl"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateSamples",
                                new FixedLocalizableString("Samples"),
                                url: "https://aspnetboilerplate.com/Samples?ref=abptmpl"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateDocuments",
                                new FixedLocalizableString("Documents"),
                                url: "https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl"
                            )
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            "AspNetZero",
                            new FixedLocalizableString("ASP.NET Zero")
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroHome",
                                new FixedLocalizableString("Home"),
                                url: "https://aspnetzero.com?ref=abptmpl"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroDescription",
                                new FixedLocalizableString("Description"),
                                url: "https://aspnetzero.com/?ref=abptmpl#description"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroFeatures",
                                new FixedLocalizableString("Features"),
                                url: "https://aspnetzero.com/?ref=abptmpl#features"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroPricing",
                                new FixedLocalizableString("Pricing"),
                                url: "https://aspnetzero.com/?ref=abptmpl#pricing"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroFaq",
                                new FixedLocalizableString("Faq"),
                                url: "https://aspnetzero.com/Faq?ref=abptmpl"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroDocuments",
                                new FixedLocalizableString("Documents"),
                                url: "https://aspnetzero.com/Documents?ref=abptmpl"
                            )
                        )
                    )
#endif
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LibraryConsts.LocalizationSourceName);
        }
    }
}
