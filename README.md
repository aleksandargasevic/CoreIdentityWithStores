# Custom storage providers for ASP.NET Core Identity

This project is writen in Net Core 2.2 framework and includes custom storage providers for ASP.NET Core Identity and you can use it with own Schema for Database and Classes with ASP.NET Core Identity and EF Core

This sample project includes implementation of :

- IdentityRole
- IdentityUser 
- RoleStore 
- UserStore

Can help you if you have your own classes for User and Role that doesn't inherit from Identity User. 

You can use UserRole and UserStore to implement your own logic or if you want to use DbContextFactory with Identity.

## Setup / Usage / How To

1. Install MsSQL and Make Database
2. Be sure to input correct MsSQL Connection String to the AppSettings pointing to the correct Database
3. Run Commands for Migration to make tables to the MsSQL database:
```
  Add-Migration InitialMigration
```
Then:
```
  Update-Database
```


ASP.NET Core provides a number of optional interfaces you may choose implement.

IUserRoleStore
IUserClaimStore
IUserPasswordStore
IUserSecurityStampStore
IUserEmailStore
IPhoneNumberStore
IQueryableUserStore
IUserLoginStore
IUserTwoFactorStore
IUserLockoutStore

You can implement all those interfaces in same way like I implemented:
For UserStore: IUserPasswordStore, IUserEmailStore, IUserRoleStore, IUserClaimStore
For RoleStore: IRoleStore, IRoleClaimStore

You can read more at: [Custom storage providers for ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-3.1)

## FAQ / Collaboration

1. Do you loose features that comes with Identity (Password Hashing...)?
- No, you can use password hashing, cookie authentication, anti-forgery, roles, claims, and all the other features that come with Identity framework.

2. Why I would use this approach when default Identity models are rich?
- One of the main reasons for this is that you maybe need just portion of Identity features, or you maybe have existing database Schema (Users, Roles...) that you cannot easily change or migrate to. 
 

Feel free to collaborate on project and expand Stores with more 
